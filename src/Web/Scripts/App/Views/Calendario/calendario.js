define([
  'jquery',
  'underscore',
  'backbone',
  'extJS',
  'collections/eventos',
  'text!templates/calendario/calendarioView.html'
], function ($, _, Backbone, extJS, eventoCollection, calendarioTemplate) {
    var view = Backbone.View.extend({
        el: $('#content'),
        initialize: function () {
            console.log('init calendar view');
            //this.collection = new PerfilCollection();
            //this.collection.add({ name: "Hello Backbone World!" });

            //var compiledTemplate = _.template(perfilListTemplate, { perfis: this.collection.models });
            //this.$el.html(compiledTemplate);
        },
        render: function () {
            var template = _.template(calendarioTemplate, {});
            this.$el.html(template);
        }
    });
    return view;
});