$().ready(function () {
    $("#btnlogin").click(function () {
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
    });

    $("#frmLogin").validate({
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

            $.ajax({
                url: "/Usuario/Login",
                type: 'POST',
                dataType: 'text',
                data: $(form).serialize(),
                success: function (response, textStatus, xhr) {
                    if (response) {
                        var newDoc = document.open("text/html", "replace");
                        newDoc.write(response);
                        newDoc.close();
                    }
                    else {
                        configureNavBarLoggedIn();
                        configureCalendarReadOnly(false);
                    }
                },
                error: function () {
                    $("#errorMsg").text('E-mail e/ou senha incorretos');
                },
                complete: function () {
                    $('#btnDoLogin').button('reset');
                }
            });
        }
    });
});

function configureNavBarLoggedIn() {
    $("#notLoggedIn").hide();
    $("#loggedInProfile").show();
    $("#navMenu").load('Usuario/Menu');
}

