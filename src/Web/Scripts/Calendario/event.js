$().ready(function () {
    configureEventFormModal();
    configureEventForm();
    configureReservedLocals();
    setDateTimePicker();
    getExistingLocals();
});

function setDateTimePicker() {
    $.each($(".datetimeField"), function () {
        $(this).datetimepicker({
            hourMin: 7,
            hourMax: 22,
            prevText: '',
            nextText: ''
        });
    });
}

function configureEventFormModal() {
    $('#divFormEvent').modal({
        show: false,
        backdrop: 'static'
    });
    $('#divFormEvent').on('shown', function (e) {
        var modal = $(this);
        modal.css('margin-left', (modal.outerWidth() / 2) * -1);
        return this;
    });
}

var lastDateFormEvent = '';
function configureEventForm() {
    $(".datetimeField").blur(function(){
        lastDateFormEvent = $(this).val();
    });
    $("#txtDateBegin").blur(function () {
        $("#txtDateEnd").datetimepicker('option', { defaultDate: lastDateFormEvent });
    });
    $("#txtDateEnd").blur(function () {
        $("#txtDateBegin").datetimepicker('option', { defaultDate: lastDateFormEvent });
    });

    $("#frmEvent").validate({
        ignore: [],
        rules: {
            Nome: {
                required: true
            },
            Tipo: {
                required: true
            },
            DataInicio: {
                required: true
            },
            DataFim: {
                required: true,
                greaterThan: "#txtDateBegin"
            }
        }
    });

    $("#btnSave").live('click', function () {
        if ($("#frmEvent").valid() && validateReservedLocalsCount()) {
            $('#btnSave').button('loading');
            
            var successMessage;
            var urlPost;
            if ($("#txtIdEvento").val() == "") {
                urlPost = '/Calendario/Salvar';
                successMessage = '<div id="msgSuccess" class="alert alert-success"> \
                                        <div class="pull-left" style="line-height:30px;"><h4 class="alert-heading">Evento agendado com sucesso!</h4></div> \
                                        <div style="line-height:30px;"> \
                                            <button type="button" id="btnScheduleOther" class="btn btn-info">Agendar outro</button> \
                                        </div> \
                                    </div>';
            }
            else {
                urlPost = '/Calendario/Alterar';
                successMessage = '<div id="msgSuccess" class="alert alert-success"> \
                                        <p><h4 class="alert-heading">Evento alterado com sucesso!</h4></p> \
                                    </div>';
            }

            $.ajax({
                url: urlPost,
                type: "POST",
                data: $("#frmEvent").serialize(),
                dataType: "json",
                success: function (data, status, xhr) {
                    var txtId = $("#txtIdEvento");
                    $("#formEditTools").fadeIn("slow");

                    $("#msgPanel").html(successMessage);

                    $("#btnScheduleOther").click(function () {
                        clearFormEvent();
                    });
                    txtId.val(data.Id);
                    reloadCalendar();
                },
                error: function (xhr, status, exception) {
                    var responseMessage = xhr.responseText;
                    var errorMessage = 'Ocorreu um erro inesperado.';
                    try {
                        errorMessage = JSON.parse(responseMessage).ErrorMessage;
                    } catch (err) { }

                    $("#msgPanel").html('<div id="msgAlert" class="alert"> \
                                                <h4 class="alert-heading">Não foi possível agendar o evento</h4> \
                                                <p>' + errorMessage + '</p> \
                                            </div>');
                },
                complete: function () {
                    $('#btnSave').button('reset');
                }
            });
        }
        return false;
    });
    $("#btnModalClose").click(function () {
        clearFormEvent();
    });
    $("#btnCopy").click(function (){
        $("#txtIdEvento").val('');
        $.each($(".dateField"), function(){
            $(this).val('');
        });
        $.each($(".datetimeField"), function(){
            $(this).val('');
        });
        $("#formEditTools").fadeOut("slow");
    });
    $('#btnCancelEvent').click(function(){
        jConfirm("Deseja cancelar este evento?", "Cancelamento", function (confirm) {
            if (confirm) {
                $.post("/Calendario/Cancelar", { id: $("#txtIdEvento").val() })
                    .success(function (response) {
                        jAlert("Evento cancelado!", "Sucesso", function () {
                            reloadCalendar();
                            closeEventModal();
                        });
                    })
                    .error(function (data) {
                        alert('erro: evento cancelado');
                    });
                }
            }
        );
    });
}

function configureReservedLocals() {
    var reservedLocals = $('#reservedLocals');
    reservedLocals
    .mCustomScrollbar({
        scrollButtons: {
            enable: true,
            scrollType: 'pixels',
            scrollAmount: 80
        }
    })
    .mCustomScrollbar("update");

    $("#btnReserveLocal").click(function () {
        addReservedLocalItem();
    });
}

function addReservedLocalItem(reserve) {
    
    var selectLocal = $('<select id="ddlLocal" class="span3 localId">' + existingLocals.join('') + '</select>');
    
    var date = $('<input id="txtDate" type="text" class="dateField span1 localDate" />');
    
    var time = $('<p><button type="button" id="timeSelectMorning" data-timeSelect="1" class="btn btn-small" rel="tooltip" data-original-title="6h às 12h">Manhã</button> \
                 <button type="button" id="timeSelectAfternoon" data-timeSelect="2" class="btn btn-small" rel="tooltip" data-original-title="12h às 18h">Tarde</button> \
                 <button type="button" id="timeSelectEvening" data-timeSelect="3" class="btn btn-small" rel="tooltip" data-original-title="18h às 22h">Noite</button></p>');
    var ddlTime = $('<select id="ddlTime" class="timeSelection" multiple="multiple" style="display:none;" > \
                        <option value="1"></option> \
                        <option value="2"></option> \
                        <option value="3"></option> \
                    </select>');

    if (reserve) {
        selectLocal.find("option[value='" + reserve.LocalId + "']").attr("selected", "selected");
        date = $('<input id="txtDate" type="text" class="dateField span1 localDate" value="' + reserve.DataUtc + '" />');
        $.each(reserve.Hora, function (i, hora) {
            time.find("[data-timeSelect=" + hora + "]").addClass("active");
            ddlTime.find('option[value=' + hora + ']').attr('selected', 'selected');
        });
    }
    
    var selectLocalWrapper = $('<p></p>').html(selectLocal);
    var dateWrapper = $('<p></p>').html(date);
    var ddlTimeWrapper = $('<p></p>').html(ddlTime);

    $('#reservedLocals .mCSB_container')
            .append('<div class="row reserved-local" style="position:relative;"> \
                    <div class="pull-left"> \
                        <span title="Remover" class="removeLocal" aria-hidden="true" data-icon="&#x2612;" style="cursor:pointer; position:absolute; top:50%; font-size:2em; color:rgba(191, 64, 64, 1);"></span> \
                    </div> \
                    <div class="span3"> \
                        <label for="ddlLocal">Local</label> \
                        <div>' + selectLocalWrapper.html() + '</div> \
                    </div> \
                    <div class="span1" style="width:95px;"> \
                        <label for="txtDate">Dia</label> \
                        <div>' + dateWrapper.html() + '</div> \
                    </div> \
                    <div id="timeContainer" class="span2" style="margin-left:10px;width:130px;"> \
                        <label for="ddlTime" style="width:100%;font-size:12px;">&nbsp;</label> \
                        <div>' + ddlTimeWrapper.html() + '</div> \
                        <div class="btn-group clearfix" data-toggle="buttons-checkbox">' + time.html() + '</div> \
                    </div> \
                </div>')
                .show("slow");

    validateReservedLocalsCount();

    $(".removeLocal").click(function () {
        var divReservedLocal = $(this).closest("div.reserved-local");
        divReservedLocal.fadeOut('fast', function() {
            divReservedLocal.remove();
        });
        configureReservedLocalsScrollBar();
        configureReservedLocalsFields();
    });

    configureReservedLocalsFields();
    configureReservedLocalsValidation();
    configureReservedLocalsScrollBar();
}

function configureReservedLocalsValidation() {
    $(".localId").each(function() {
        $(this).rules('add', {
            required: true
        })
    });
    $(".localDate").each(function () {
        $(this).rules('add', {
            required: true
        })
    });
    $(".timeSelection").each(function () {
        $(this).rules('add', {
            required: true,
            minlength: 1
        })        
    });
}

function configureReservedLocalsFields() {
    var i = 0;
    $.each($("div.reserved-local"), function () {
        $(this).find("label[for^=ddlLocal]").attr("for", "ddlLocal" + i);
        $(this).find("span[for^=ddlLocal]").attr("for", "ddlLocal" + i);
        $(this).find("select[id^=ddlLocal]").attr("id", "ddlLocal" + i).attr("name", "Reservas[" + i + "].LocalId");

        $(this).find("label[for^=txtDate]").attr("for", "txtDate" + i);
        $(this).find("span[for^=txtDate]").attr("for", "txtDate" + i);
        $(this).find("input[id^=txtDate]")
            .attr("id", "txtDate" + i)
            .attr("name", "Reservas[" + i + "].Data");
        $(this).find("input[id^=txtDate]").datepicker({
            defaultDate: lastDateFormEvent,
            prevText: '',
            nextText: ''
        });

        $(this).find("label[for^=ddlTime]").attr("for", "ddlTime" + i);
        $(this).find("span[for^=ddlTime]").attr("for", "ddlTime" + i);
        $(this).find("select[id^=ddlTime]").attr("id", "ddlTime" + i).attr("name", "Reservas[" + i + "].Hora");
        
        $(this).find("button[id^=timeSelectMorning]")
            .attr("id", "timeSelectMorning" + i);
        $(this).find("button[id^=timeSelectAfternoon]")
            .attr("id", "timeSelectAfternoon" + i);
        $(this).find("button[id^=timeSelectEvening]")
            .attr("id", "timeSelectEvening" + i);
        i++;
    });

    $('button[id^="timeSelect"]').each(function () {
        $(this).unbind().click(function () {
            setTimeReserved($(this));
        });
    });
}

function configureReservedLocalsScrollBar() {
    var reservedLocalsCount = $('#reservedLocals .mCSB_container').children().length;
    if (reservedLocalsCount >= 3) {
        $('#reservedLocals').height(190);
    }
    else {
        $('#reservedLocals').css({ "height": '' });
    }
    $('#reservedLocals').mCustomScrollbar("update");
    $('#reservedLocals').mCustomScrollbar("scrollTo", "bottom");
}

var existingLocals = [];
function getExistingLocals() {
    $.getJSON("/Paroquia/Locais", function (data) {
        existingLocals.push('<option value="">Selecione</option>');
        $.each(data, function (index, item) {
            existingLocals.push('<option value="' + item.Id + '">' + item.Nome + '</option>');
        });
    });
}

function setTimeReserved(buttonTime) {
    var timeId = buttonTime.attr("data-timeSelect");
    var timeSelectField = buttonTime.closest("#timeContainer").find('select[id^="ddlTime"]');

    var timesSelected = timeSelectField.val() || [];

    var index = timesSelected.indexOf(timeId.toString());
    if (index != -1) {
        timesSelected.splice(index, 1);
    }
    else {
        timesSelected.push(timeId.toString());
    }
    timeSelectField.val(timesSelected);
}

function validateReservedLocalsCount() { 
    var reservedLocalsCount = $('#reservedLocals .mCSB_container').children().length;
    if (reservedLocalsCount > 0) {
        $('#reservedLocalsError').remove();
        return true;
    }
    else {
        if ($("#reservedLocalsError").length == 0)
            $('<label id="reservedLocalsError" class="field-validation-error">&nbsp;&nbsp;obrigatório</label>').insertAfter("#btnReserveLocal");

        return false;
    }
}

function clearFormEvent() {
    $("#frmEvent").find(':input').each(function () {
        switch(this.type) {
            case 'select-one':
            case 'text':
            case 'textarea':
            case 'hidden':
                $(this).val('');
        }
    });
    $("#formEditTools").hide();
    $(".reserved-local").remove();
    $("#msgPanel").html('');
}

function showFormEvent(event) {
    $('#divFormEvent').modal('show');
    if (event !== undefined) {
        $("#formEditTools").show();
        $.each(event.data, function (key, value) {
            var element = $("#frmEvent").find("[name='" + key + "']"); 
            if (element.hasClass('datetimeField'))
                element.datetimepicker('setDate', value);
            else
                element.val(value);
        });
        $.each(event.data.Reservas, function (i, reserve) {
            addReservedLocalItem(reserve);
        });
    }
}

function closeEventModal() {
    $('#divFormEvent').modal("hide");
    clearFormEvent();
}