define([
  'jquery',
  'underscore',
  'backbone',
  'text!templates/acesso/naoLogado.html',
  'text!templates/acesso/logado.html',
  'models/login',
  'bootstrap',
  'jqueryValidate',
  'integer_jValidate'
], function ($, _, Backbone, naoLogadoTemplate, logadoTemplate, loginModel) {
    var loginView = Backbone.View.extend({

        el: $('.loginmenu'),

        model: new loginModel(),

        events: {
            "click #btnlogin": "showLoginForm",
            "click #btnDoLogin": "login"
        },

        render: function () {
            if (this.model.get('loggedIn')) {
                var template = _.template(logadoTemplate, this.model);
            } else {
                var template = _.template(naoLogadoTemplate, this.model);
            }
            $(this.el).empty().html(template);
            return this;
        },

        showLoginForm: function(){
            var loginform = $("#loginForm");

            if (loginform.is(':visible'))
                loginform.fadeOut('fast');
            else {
                loginform.fadeIn('fast');
            }

            $('body').append("<div id='mask'></div>");
            $('#mask').css({
                'position': 'absolute',
                'left': 0,
                'right': 0,
                'top': 0,
                'bottom': 0,
                'z-index': 3
            });

            $('#mask').click(function () {
                $(this).remove();
                $("#loginForm").fadeOut('fast');
            });
        },

        login: function (ev) {
            ev.preventDefault();

            $("#frmLogin").validate({
                rules: {
                    email: {
                        required: true
                    },
                    senha: {
                        required: true
                    }
                }
            });

            if ($("#frmLogin").valid()) {
                $(ev.target).button('loading');

                $.ajax({
                    url: "/api/login",
                    type: 'POST',
                    data: $("#frmLogin").serialize(),
                    success: function (response, textStatus, xhr) {
                        if (response) {
                            var newDoc = document.open("text/html", "replace");
                            newDoc.write(response);
                            newDoc.close();
                        }
                    },
                    error: function () {
                        $("#errorMsg").text('E-mail e/ou senha incorretos');
                    },
                    complete: function () {
                        $(ev.target).button('reset');
                    }
                });
            }
            return false;
        }
    });
    return loginView;
});