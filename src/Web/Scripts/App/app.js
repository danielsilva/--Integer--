define([
  'jquery',
  'underscore',
  'backbone',
  'router',
  'views/acesso/loginView'
], function ($, _, Backbone, Router, LoginView) {
    var initialize = function () {
        Router.initialize();
        new LoginView().render();
    }

    return {
        initialize: initialize
    };
});