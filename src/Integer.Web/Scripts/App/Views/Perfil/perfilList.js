define([
  'jquery',
  'underscore',
  'backbone',
  'collections/perfis',
  'text!templates/perfil/perfilList.html'
], function ($, _, Backbone, perfilCollection, perfilListTemplate) {
    var PerfilListView = Backbone.View.extend({
        el: $('#container'),
        initialize: function () {
            this.collection = new PerfilCollection();
            this.collection.add({ name: "Hello Backbone World!" });
            
            var compiledTemplate = _.template(perfilListTemplate, { perfis: this.collection.models });
            this.$el.html(compiledTemplate);
        }
    });
    return PerfilListView;
});