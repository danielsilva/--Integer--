define([
  'jquery',
  'underscore',
  'backbone',
  'views/perfil/perfilList'
], function($, _, Backbone, Session, PerfilListView){
    var AppRouter = Backbone.Router.extend({
        routes: {
            '/perfil': 'showPerfis',
            // Default
            '*actions': 'defaultAction'
        }
    });

var initialize = function(){
    var app_router = new AppRouter;
    app_router.on('showPerfis', function () {
        var perfilListView = new PerfilListView();
        perfilListView.render();
    });
    app_router.on('defaultAction', function(actions){
        console.log('No route:', actions);
    });
    Backbone.history.start();
};
return {
    initialize: initialize
};
});