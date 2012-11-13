define([
  'underscore',
  'backbone'
], function (_, Backbone) {
    var PerfilModel = Backbone.Model.extend({
        defaults: {
            Nome: ""
        },
        url: "/Acesso/Perfil"
    });
    return PerfilModel;
});