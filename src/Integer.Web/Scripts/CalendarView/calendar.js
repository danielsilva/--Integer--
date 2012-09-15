$().ready(function () {
    var reservedLocals = $('#reservedLocals');
    reservedLocals
        .mCustomScrollbar({ scrollButtons: { enable: true} })
        .css({ "height": reservedLocals.css("max-height") })
        .mCustomScrollbar("update");
});


function initCalendar(events) {
    Ext.onReady(function () {
        var calendarWidth = 810;
        var startDate = new Date();

        Ext.create('Extensible.calendar.CalendarPanel', {
            eventStore: Ext.create('Extensible.calendar.data.MemoryEventStore', {
                data: events
            }),
            startDate: startDate,
            renderTo: 'divCalendar',
            width: calendarWidth,
            height: 520,

            readOnly: false,

            showMultiWeekView: false,
            showNavJump: true,

            todayText: 'Hoje',
            jumpToText: 'Navegar:',
            goText: 'Ir',
            dayText: 'Dia',
            weekText: 'Semana',
            monthText: 'Mês',

            listeners: {
                'datechange': {
                    fn: function (panel, startDate, viewSart, viewEnd) {
                        setCalendarTitle(startDate);
                    },
                    scope: this
                },
                'dayclick': {
                    fn: function (panel, date, allDay, el) {
                        showFormEvent(date, el);
                        return false;
                    },
                    scope: this
                }
            }
        });

        $("#calendarTitle").width(calendarWidth);
        $("#divCalendar").width(calendarWidth);

        setCalendarTitle(startDate);
    });
}

function setCalendarTitle(date) {
    $("#calendarTitle").html(Ext.Date.format(date, 'F'));
}