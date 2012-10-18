﻿var calendarPanel;

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

    calendarPanel = Ext.create('Extensible.calendar.CalendarPanel', {
        eventStore: eventStore,
        startDate: startDate,
        renderTo: 'divCalendar',
        width: calendarWidth,
        height: 520,

        readOnly: true,

        showMultiWeekView: false,
        showNavJump: true,

        todayText: 'Hoje',
        jumpToText: 'Navegar:',
        goText: 'Ir',
        dayText: 'Dia',
        weekText: 'Semana',
        monthText: 'Mês',

        monthViewCfg: {
            getMoreText: function (eventCount) {
                return 'mais {0}';
            }
        },

        listeners: {
            'datechange': {
                fn: function (panel, startDate, viewSart, viewEnd) {
                    setCalendarTitle(startDate);
                },
                scope: this
            },
            'dayclick': {
                fn: function (panel, date, allDay, el) {
                    if (!calendarPanel.readOnly)
                        showFormEvent();
                    return false;
                },
                scope: this
            },
            'eventclick': {
                fn: function (cal, ev, el) {
                    if (!calendarPanel.readOnly)
                        showFormEvent();
                    else
                        console.log('popover: ' + ev.id);
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

function reloadCalendar() {
    calendarPanel.getActiveView().refresh()
}

function configureCalendarReadOnly(readOnly) {
    if (calendarPanel) 
        calendarPanel.readOnly = readOnly;
}