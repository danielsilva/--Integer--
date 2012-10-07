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
                $('#btnSave').button('loading');​
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

    var reservedLocalsCount = 0;
    $("#btnReserveLocal").click(function () {
        $('#reservedLocals .mCSB_container')
            .append('<div class="row reserved-local" style="position:relative;"> \
                    <span title="Remover" class="pull-left removeLocal" aria-hidden="true" data-icon="&#x2612;" style="cursor:pointer; position:absolute; top:50%; font-size:2em; color:rgba(191, 64, 64, 1);"></span> \
                    <div class="span3"> \
                        <label for="ddlLocal' + reservedLocalsCount + '">Local</label> \
                        <select id="ddlLocal' + reservedLocalsCount + '" name="reservedLocal[' + reservedLocalsCount + '].LocalId" class="span3 localId"> </select> \
                    </div> \
                    <div class="span1" style="width:95px;"> \
                        <label for="txtDate' + reservedLocalsCount + '">Dia</label> \
                        <input id="txtDate' + reservedLocalsCount + '" type="text" name="reservedLocal[' + reservedLocalsCount + '].Date" class="dateField span1 localDate" /> \
                    </div> \
                    <div class="span2" style="margin-left:10px;width:130px;"> \
                        <label for="hddTime' + reservedLocalsCount + '" style="width:100%;font-size:12px;">&nbsp;</label> \
                        <input id="hddTime' + reservedLocalsCount + '" type="hidden" class="timeSelection" value="" /> \
                        <div class="btn-group clearfix" data-toggle="buttons-checkbox"> \
                            <button type="button" id="timeSelectMorning' + reservedLocalsCount + '" class="btn btn-small">Manhã</button> \
                            <button type="button" id="timeSelectAfternoon' + reservedLocalsCount + '" class="btn btn-small">Tarde</button> \
                            <button type="button" id="timeSelectEvening' + reservedLocalsCount + '" class="btn btn-small">Noite</button> \
                        </div> \
                    </div> \
                </div>')
                .show('slow');
        validateReservedLocalsCount();
        $("select[name='reservedLocal[" + reservedLocalsCount + "].LocalId']").html(existingLocals.join(''));

        $(".removeLocal").click(function () {
            $(this).effect("highlight", {}, 3000);
            $(this).closest("div").remove();
            configureReservedLocalsScrollBar();
        });
        $.each($(".dateField"), function () {
            $(this).datepicker({
                prevText: '',
                nextText: ''
            });
        });
        $('#timeSelectMorning' + reservedLocalsCount)
            .tooltip({ placement: 'top', title: '6h às 12h' })
            .click(function(){ setTimeReserved($(this)); });
        $('#timeSelectAfternoon' + reservedLocalsCount)
            .tooltip({ placement: 'top', title: '12h às 18h' })
            .click(function(){ setTimeReserved($(this)); });
        $('#timeSelectEvening' + reservedLocalsCount)
            .tooltip({ placement: 'top', title: '18h às 22h' })
            .click(function(){ setTimeReserved($(this)); });

        configureReservedLocalsValidation();
        configureReservedLocalsScrollBar();
        reservedLocalsCount++;
    });
}

function configureReservedLocalsValidation()
{
    $(".localId, .localDate, .timeSelection").each(function() {
        $(this).rules('add', {
            required: true
        })
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

function setTimeReserved(element) {
    alert(element.attr("class"));
}

function validateReservedLocalsCount() { 
    var reservedLocalsCount = $('#reservedLocals .mCSB_container').children().length;
    if (reservedLocalsCount > 0) {
        $('#reservedLocalsError').remove();
        return true;
    }
    else {
        $('<label id="reservedLocalsError" class="field-validation-error">&nbsp;&nbsp;obrigatório</label>').insertAfter("#btnReserveLocal");
        return false;
    }
}