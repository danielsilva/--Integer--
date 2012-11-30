define([
  'jquery',
  'underscore',
  'backbone',
  'collections/eventos',
  'text!templates/calendario/calendarioView.html',
  'bootstrap',
  'extJS',
  'extensCore',
  'extensRecurr',
  'extensCal'
], function ($, _, Backbone, eventoCollection, calendarioTemplate) {
    var view = Backbone.View.extend({
        el: $('#content'),
        render: function () {
            var template = _.template(calendarioTemplate, {});
            this.$el.html(template);

            loadCss("/Content/extCalendar/css/extensible-all.css");
            loadCss("/Content/calendario.css");
            renderCalendar();
        }
    });
    return view;
});


var calendarPanel;
function renderCalendar() {
    Ext.onReady(function () {
        var calendarWidth = 810;
        var startDate = new Date();

        Extensible.calendar.data.EventMappings = {
            EventId: { name: 'Id', mapping: 'Id' },
            CalendarId: { name: 'AgendaId', mapping: 'AgendaId', type: 'int' },
            Title: { name: 'Nome', mapping: 'Nome' },
            StartDate: { name: 'DataInicio', mapping: 'DataInicio', type: 'date', dateFormat: 'c' },
            EndDate: { name: 'DataFim', mapping: 'DataFim', type: 'date', dateFormat: 'c' },
            RRule: { name: 'RecurRule', mapping: 'recur_rule' },
            Location: { name: 'Locais', mapping: 'Locais' },
            Notes: { name: 'Desc', mapping: 'full_desc' },
            Url: { name: 'LinkUrl', mapping: 'link_url' },
            IsAllDay: { name: 'AllDay', mapping: 'all_day', type: 'boolean' },
            Reminder: { name: 'Reminder', mapping: 'reminder' },

            Description: { name: 'Descricao', mapping: 'Descricao' },
            Group: { name: 'Grupo', mapping: 'Grupo' },
            GroupId: { name: 'GrupoId', mapping: 'GrupoId' },
            TypeId: { name: 'Tipo', mapping: 'TipoId' },
            Reserves: { name: 'Reservas', mapping: 'Reservas' }
        };
        Extensible.calendar.data.EventModel.reconfigure();

        var calendarStore = new Ext.data.JsonStore({
            storeId: 'calendarStore',
            autoLoad: true,
            proxy: {
                type: 'rest',
                url: 'api/Agenda',
                noCache: false,
                reader: new Ext.data.JsonReader({
                    type: 'json',
                    root: 'Agendas',
                    fields: ['id', 'title', 'desc', 'color', 'hidden']
                }),
                listeners: {
                    exception: function (proxy, response, operation, options) {
                        var msg = response.message ? response.message : Ext.decode(response.responseText).message;
                        // ideally an app would provide a less intrusive message display
                        Ext.Msg.alert('Server Error', msg);
                    }
                }
            }
        });

        var eventStore = Ext.create('Extensible.calendar.data.EventStore', {
            autoLoad: true,
            proxy: {
                type: 'rest',
                url: 'api/Evento',
                noCache: false,
                reader: {
                    type: 'json',
                    root: 'Eventos'
                }
            }
        });

        calendarPanel = Ext.create('Extensible.calendar.CalendarPanel', {
            calendarStore: calendarStore,
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
                        if (!calendarPanel.readOnly && MD5(ev.data.GrupoId).toLowerCase() == $.cookies.get("gid").toLowerCase())
                            showFormEvent(ev);
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
                            title: ev.data.Nome,
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
}

function setCalendarTitle(date) {
    $("#calendarTitle").html(Ext.Date.format(date, 'F'));
}

function reloadCalendar() {
    var reloadEvents = true;
    calendarPanel.getActiveView().refresh(reloadEvents);
}

function configureCalendarReadOnly(readOnly) {
    if (calendarPanel)
        calendarPanel.readOnly = readOnly;
}

function getPopoverContent(event) {
    var start = event.DataInicio.getHours() + ":" + (event.DataInicio.getMinutes() < 10 ? ('0' + event.DataInicio.getMinutes()) : event.DataInicio.getMinutes());
    var end = event.DataFim.getHours() + ":" + (event.DataFim.getMinutes() < 10 ? ('0' + event.DataFim.getMinutes()) : event.DataFim.getMinutes());

    var dateString = start + ' até ' + end;

    var oneDay = 1000 * 60 * 60 * 24;
    var longDaysEvent = Math.ceil((event.DataFim.getTime() - event.DataInicio.getTime()) / (oneDay)) > 1;

    if (longDaysEvent) {
        start = $.datepicker.formatDate('dd/mm/yy ', event.DataInicio) + start;
        end = $.datepicker.formatDate('dd/mm/yy ', event.DataFim) + end;
        dateString = start + ' até <br/>' + end;
    }

    return '<label><b>' + dateString + '</b></label> \
            <label>' + (event.Descricao != null ? event.Descricao : '') + '</label> \
            <label>' + (event.Locais != '' ? '<br/>Local: ' + event.Locais : '') + '</label> \
            <br/> \
            <label><i>' + event.Grupo + '</i></label>';
}