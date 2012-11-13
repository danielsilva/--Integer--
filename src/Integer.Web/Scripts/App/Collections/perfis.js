define([
  'underscore',
  'backbone',
  'models/perfil'
], function (_, Backbone, ProjectModel) {
    var PerfilCollection = Backbone.Collection.extend({
        model: PerfilModel,
        url: "/Acesso/Perfis"
    });
    return PerfilCollection;
});