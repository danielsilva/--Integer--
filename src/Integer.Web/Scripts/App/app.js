define([
  'jquery',
  'underscore',
  'backbone',
  'router',
  'bootstrap'
], function ($, _, Backbone, Router) {
    var initialize = function () {
        Router.initialize(); 
    }

    return {
        initialize: initialize
    };
});