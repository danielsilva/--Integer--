$().ready(function () {
    $('#divFormEvent').modal({
        show: false,
        backdrop: 'static'
    });

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
        $('#reservedLocals .mCSB_container').append('<div class="row reserved-local" style="position:relative;"> \
                    <span class="pull-left removeLocal" aria-hidden="true" data-icon="&#x2612;" style="cursor:pointer; position:absolute; top:50%; font-size:2em; color:rgba(191, 64, 64, 1);"></span> \
                    <div class="span3"> \
                        <label>Local</label> \
                        <select class="span3"> </select> \
                    </div> \
                    <div class="span1" style="width:95px;"> \
                        <label>Dia</label> \
                        <input type="text" name="reserveDate" class="dateField span1" /> \
                    </div> \
                    <div class="span2"> \
                        <label style="width:100%;font-size:12px;">&nbsp;</label> \
                        <div class="btn-group clearfix" data-toggle="buttons-checkbox"> \
                            <button type="button" name="timeMorning" class="btn btn-small">Manhã</button> \
                            <button type="button" name="timeAfternoon" class="btn btn-small">Tarde</button> \
                            <button type="button" name="timeEvening" class="btn btn-small">Noite</button> \
                        </div> \
                    </div> \
                </div>')
                .show('slow');
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
        $('button[name="timeMorning"]').tooltip({ placement: 'top', title: '6h às 12h' });
        $('button[name="timeAfternoon"]').tooltip({ placement: 'top', title: '12h às 18h' });
        $('button[name="timeEvening"]').tooltip({ placement: 'top', title: '18h às 22h' });

        configureReservedLocalsScrollBar();
    });

    setDateTimePicker();
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