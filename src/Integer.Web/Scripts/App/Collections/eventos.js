define([
  'underscore',
  'backbone',
  'models/evento'
], function (_, Backbone, ProjectModel) {
    var EventoCollection = Backbone.Collection.extend({
        model: EventoModel,
        url: "/api/Evento"
    });
    return EventoCollection;
});