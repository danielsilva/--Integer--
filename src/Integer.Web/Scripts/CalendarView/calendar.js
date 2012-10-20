var calendarPanel;

Ext.onReady(function () {
    var calendarWidth = 810;
    var startDate = new Date();

//    Extensible.calendar.data.EventMappings = {
//        EventId: { name: 'id', mapping: 'Id', type: 'int' },
//        CalendarId: { name:    'CalendarId', mapping: 'cid', type:    'string' },
//        Title: { name: 'nomeEvento', mapping: 'Nome' },
//        StartDate: { name: 'start', mapping: 'DataInicio', type: 'date', dateFormat: 'c' },
//        EndDate: { name: 'end', mapping: 'DataFim', type: 'date', dateFormat: 'c' },
//        RRule:  { name: 'RecurRule', mapping: 'recur_rule'},
//        Location: { name: 'locaisReservados', mapping: 'Locais' },
//        Notes: { name: 'desc', mapping: 'Descricao' },
//        Url: { name: 'LinkUrl', mapping: 'link_url'},
//        IsAllDay: { name: 'AllDay', mapping: 'all_day', type: 'boolean'},
//        Reminder: { name: 'Reminder', mapping: 'reminder'},
//        
//        Group: { name: 'grupo', mapping: 'Grupo' }
//    };
//    Extensible.calendar.data.EventModel.reconfigure();

    Extensible.calendar.data.EventMappings.Description = { name: 'Description', mapping: 'description', type: 'string' };
    Extensible.calendar.data.EventMappings.Location = { name: 'Location', mapping: 'location', type: 'string' };
    Extensible.calendar.data.EventMappings.Group = { name: 'Group', mapping: 'group', type: 'string' };
    Extensible.calendar.data.EventModel.reconfigure();

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
            },
            'eventover': {
                fn: function (cal, ev, el) {
                    var jEl = $(el.dom);

                    var elPos = jEl.offset().left;
                    var windowCenter = ($(window).width() - jEl.outerWidth()) / 2;
                    var overCenter = elPos > windowCenter;
                    var popoverPos = overCenter ? 'left' : 'right';
                    
                    jEl.popover({
                        title: ev.data.Title,
                        content: getPopoverContent(ev.data),
                        placement: popoverPos,
                        selector: el.id
                    });
                    jEl.popover('show');
                },
                scope: this
            },
            'eventout': {
                fn: function (cal, ev, el) {
                    $(el.dom).popover('hide');
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

function getPopoverContent(event) {
    var start = event.StartDate.getHours() + ":" + (event.StartDate.getMinutes() < 10 ? ('0' + event.StartDate.getMinutes()) : event.StartDate.getMinutes());
    var end = event.EndDate.getHours() + ":" + (event.EndDate.getMinutes() < 10 ? ('0' + event.EndDate.getMinutes()) : event.EndDate.getMinutes());

    var dateString = start + ' até ' + end;

    var oneDay = 1000 * 60 * 60 * 24;
    var longDaysEvent = Math.ceil((event.EndDate.getTime() - event.StartDate.getTime()) / (oneDay)) > 1;

    if (longDaysEvent) {
        start = $.datepicker.formatDate('dd/mm/yy ', event.StartDate) + start;
        end = $.datepicker.formatDate('dd/mm/yy ', event.EndDate) + end;
        dateString = start + ' até <br/>' + end;
    }

    return '<label><b>' + dateString + '</b></label> \
            <label>' + event.Description + '</label> \
            <br/> \
            <label><i>' + event.Group + '</i></label>';
}