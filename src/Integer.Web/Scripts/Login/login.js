﻿$().ready(function () {
    $("#btnlogin").click(function () {
        var loginform = $("#loginform");

        if (loginform.is(':visible'))
            loginform.fadeOut('fast');
        else
            loginform.fadeIn('fast');

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
            $("#loginform").fadeOut('fast');
        });
    });

    $("#loginform").validate({
        rules: {
            email: {
                required: true
            },
            senha: {
                required: true
            }
        },
        submitHandler: function (form) {
            $('#btnDoLogin').button('loading');

            $.post("/Usuario/Login", $(form).serialize())
            .success(function () {
                configureNavBarLoggedIn();
            })
            .complete(function () {
                $('#btnDoLogin').button('reset');
            });
        }
    });
});

function configureNavBarLoggedIn() {
    $("#notLoggedIn").hide();
    $("#loggedInProfile").show();
    $("#navMenu").html('<li> \
                    <button id="btnSchedule" class="btn btn-info" data-toggle="modal" data-target="#divFormEvent">Agendar</button> \
                </li>');// TODO; get from server
}

