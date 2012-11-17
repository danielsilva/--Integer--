define([
  'underscore',
  'backbone'
], function (_, Backbone) {
    var EventoModel = Backbone.Model.extend({
        defaults: {
            Nome: ""
        },
        url: "/api/Evento"
    });
    return EventoModel;
});