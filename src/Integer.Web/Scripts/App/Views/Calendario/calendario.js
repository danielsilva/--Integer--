define([
  'jquery',
  'underscore',
  'backbone',
  'collections/eventos',
  'text!templates/calendario/calendarioView.html'
], function ($, _, Backbone, eventoCollection, calendarioTemplate) {
    var calendarioView = Backbone.View.extend({
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