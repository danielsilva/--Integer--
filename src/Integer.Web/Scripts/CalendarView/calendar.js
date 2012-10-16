Ext.onReady(function () {
    var calendarWidth = 810;
    var startDate = new Date();

    var eventStore = Ext.create('Extensible.calendar.data.EventStore', {
        autoLoad: true,
        proxy: {
            type: 'rest',
            url: 'Calendario/Eventos',
            noCache: false,
            reader: {
                type: 'json',
                root: 'Eventos'
            },
            listeners: {
                exception: function (proxy, response, operation, options) {
                    var msg = response.message ? response.message : Ext.decode(response.responseText).message;
                    // ideally an app would provide a less intrusive message display
                    Ext.Msg.alert('Server Error', msg);
                }
            }
        }
    });

    Ext.create('Extensible.calendar.CalendarPanel', {
        eventStore: eventStore,
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


function setCalendarTitle(date) {
    $("#calendarTitle").html(Ext.Date.format(date, 'F'));
}