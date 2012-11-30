require.config({
    paths: {
        jquery: [
            'http://ajax.googleapis.com/ajax/libs/jquery/1.8.0/jquery.min',
            '/Scripts/jquery-1.8.1.min'
        ],
        underscore: [ 
            '//cdnjs.cloudflare.com/ajax/libs/underscore.js/1.3.1-amdjs/underscore-amd-min',
            '/Scripts/underscore-amdjs.min'
        ],
        backbone: [
            '//cdnjs.cloudflare.com/ajax/libs/backbone.js/0.9.2-amdjs/backbone-min',
            '/Scripts/backbone-amdjs.min'
        ],
        bootstrap: [
            'http://netdna.bootstrapcdn.com/twitter-bootstrap/2.1.0/js/bootstrap.min',
            '/Scripts/bootstrap.min'
        ],
        extJS: [
            'http://cdn.sencha.io/ext-4.0.7-gpl/ext-all',
            '/Scripts/plugins/ExtJS/ext-all'
        ],
        extJS_BR: [
            'http://cdn.sencha.io/ext-4.0.7-gpl/locale/ext-lang-pt_BR',
            '/Scripts/plugins/ExtJS/locale/ext-lang-pt_BR'
        ],
        jqueryValidate: [
            '//cdnjs.cloudflare.com/ajax/libs/jquery-validate/1.10.0/jquery.validate.min',
            '/Scripts/jquery.validate.min'
        ],
        integer_jValidate: '/Scripts/shared/integer.jquery.validate',
        extensCore: '/Scripts/Plugins/extCalendar/extensible-core',
        extensRecurr: '/Scripts/Plugins/extCalendar/recurrence-debug',
        extensCal: '/Scripts/Plugins/extCalendar/calendar-debug',
        extensCalLang: '/Scripts/Plugins/extCalendar/locale/ext-lang-pt_BR',
        text: '../text',
        templates: '/Templates'
    },
    shim: {
        "backbone": { deps: ["underscore", "jquery"] },
        "bootstrap": { deps: ["jquery"] },
        "extJS_BR": { deps: ["extJS"] },
        "jqueryValidate": { deps: ["jquery"] },
        "integer_jValidate": { deps: ["jqueryValidate"] },
        "extensCore": { deps: ["extJS"] },
        "extensRecurr": { deps: ["extJS"] },
        "extensCal": { deps: ["extensCore"] },
        "extensCalLang": { deps: ["extensCal"] }
    }
});

require([
  'app',
], function (App) {
    App.initialize();
});

