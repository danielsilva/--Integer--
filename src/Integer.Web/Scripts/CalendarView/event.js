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
            hourGrid: 2,
            minuteGrid: 10,
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

function configureEventForm() {
    $("#frmEvent").removeAttr("novalidate");
    
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
                required: true
            }
        },
        submitHandler: function(form) {
            if (validateReservedLocalsCount()) {
                $('#btnSave').button('loading');

                $.post("/Calendario/Salvar", $(form).serialize())
                .success(function(){ 
                    $('#btnSave').button('reset');
                    $("#msgSuccess").css("visibility", "visible");
                });
            }
        }
    });
    $("#btnSave").click(function() { 
        validateReservedLocalsCount();
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

    var i = 0;
    $("#btnReserveLocal").click(function () {
        $('#reservedLocals .mCSB_container')
            .append('<div class="row reserved-local" style="position:relative;"> \
                    <span title="Remover" class="pull-left removeLocal" aria-hidden="true" data-icon="&#x2612;" style="cursor:pointer; position:absolute; top:50%; font-size:2em; color:rgba(191, 64, 64, 1);"></span> \
                    <div class="span3"> \
                        <label for="ddlLocal' + i + '">Local</label> \
                        <select id="ddlLocal' + i + '" class="span3 localId"> </select> \
                    </div> \
                    <div class="span1" style="width:95px;"> \
                        <label for="txtDate' + i + '">Dia</label> \
                        <input id="txtDate' + i + '" type="text" class="dateField span1 localDate" /> \
                    </div> \
                    <div id="timeContainer" class="span2" style="margin-left:10px;width:130px;"> \
                        <label for="ddlTime' + i + '" style="width:100%;font-size:12px;">&nbsp;</label> \
                        <select id="ddlTime' + i + '" class="timeSelection" multiple="multiple" style="display:none;" > \
                            <option value="1"></option> \
                            <option value="2"></option> \
                            <option value="3"></option> \
                        </select> \
                        <div class="btn-group clearfix" data-toggle="buttons-checkbox"> \
                            <button type="button" id="timeSelectMorning' + i + '" data-timeSelect="1" class="btn btn-small">Manhã</button> \
                            <button type="button" id="timeSelectAfternoon' + i + '" data-timeSelect="2" class="btn btn-small">Tarde</button> \
                            <button type="button" id="timeSelectEvening' + i + '" data-timeSelect="3" class="btn btn-small">Noite</button> \
                        </div> \
                    </div> \
                </div>')
                .show('slow');
        validateReservedLocalsCount();
        $("select[id='ddlLocal" + i + "']").html(existingLocals.join(''));

        $(".removeLocal").click(function () {
            $(this).closest("div").remove();
            configureReservedLocalsScrollBar();
            configureReservedLocalsFields();
        });

        configureReservedLocalsFields();
        configureReservedLocalsValidation();
        configureReservedLocalsScrollBar();
        
        i++;
    });
}

function configureReservedLocalsValidation() {
    $(".localId, .localDate").each(function() {
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
            .attr("name", "Reservas[" + i + "].Data")
            .datepicker({
                prevText: '',
                nextText: ''
            });

        $(this).find("label[for^=ddlTime]").attr("for", "ddlTime" + i);
        $(this).find("span[for^=ddlTime]").attr("for", "ddlTime" + i);
        $(this).find("select[id^=ddlTime]").attr("id", "ddlTime" + i).attr("name", "Reservas[" + i + "].Hora");

        $(this).find("button[id^=timeSelectMorning]")
            .attr("id", "timeSelectMorning" + i)
            .unbind()
            .tooltip({ placement: 'top', title: '6h às 12h' });
        $(this).find("button[id^=timeSelectAfternoon]")
            .attr("id", "timeSelectAfternoon" + i)
            .unbind()
            .tooltip({ placement: 'top', title: '12h às 18h' });
        $(this).find("button[id^=timeSelectEvening]")
            .attr("id", "timeSelectEvening" + i)
            .unbind()
            .tooltip({ placement: 'top', title: '18h às 22h' });
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
    $.getJSON("/Paroquia/Locals", function (data) {
        existingLocals.push('<option value="">Selecione</option>');
        $.each(data, function (index, item) {
            existingLocals.push('<option value="' + item.Id + '">' + item.Name + '</option>');
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