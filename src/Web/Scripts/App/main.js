require.config({
    paths: {
        jquery: [
            'http://ajax.googleapis.com/ajax/libs/jquery/1.8.0/jquery.min',
            'Scripts/jquery-1.8.1.min'
        ],
        underscore: [ 
            'http://cdnjs.cloudflare.com/ajax/libs/underscore.js/1.3.3/underscore-min',
            'Scripts/underscore.min'
        ],
        backbone: [
            'http://cdnjs.cloudflare.com/ajax/libs/backbone.js/0.9.2/backbone-min',
            'Scripts/backbone.min'
        ],
        bootstrap: [
            'http://netdna.bootstrapcdn.com/twitter-bootstrap/2.1.0/js/bootstrap.min',
            'Scripts/bootstrap.min'
        ],
        text: '../text',
        templates: '/Templates'
    },
    shim: {
        "bootstrap": {
            deps: ["jquery"]
        }
    }
});

require([
  'app',
], function (App) {
    App.initialize();
});

