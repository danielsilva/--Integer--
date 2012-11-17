define([
  'jquery',
  'underscore',
  'backbone',
  'views/calendario/calendario'
], function($, _, Backbone, CalendarioView){
    var AppRouter = Backbone.Router.extend({
        routes: {
            'calendario': 'showCalendarView',
            // Default
            '*actions': 'defaultAction'
        }
    });

    var initialize = function () {
        var app_router = new AppRouter(); 
        app_router.on('route:showCalendarView', function () {
            var calView = new CalendarioView();
            calView.render();
        });
        app_router.on('defaultAction', function(actions){
            console.log('no route');
        });
        Backbone.history.start();
        app_router.navigate('calendario', true);
    };

    return {
        initialize: initialize
    };
});