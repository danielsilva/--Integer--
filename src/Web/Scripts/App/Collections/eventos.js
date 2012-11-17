define([
  'underscore',
  'backbone',
  'models/evento'
], function (_, Backbone, EventoModel) {
    var EventoCollection = Backbone.Collection.extend({
        model: EventoModel,
        url: "/api/Evento"
    });
    return EventoCollection;
});