$(function () {
    Integer.Perfil = Backbone.Model.extend();

    Integer.PerfilCollection = Backbone.Collection.extend({
        model: window.Integer.Perfil,
        url: "/Acesso/Perfis"
    });
});