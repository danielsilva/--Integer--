﻿$().ready(function () {
    $("#btnCreateUser").live("click", function () {
        if ($("#frmUser").valid()) {
            $(this).button('loading');

            $.post("/Usuario/Criar", $("#frmUser").serialize())
            .success(function (data) {
                window.location = "/";
            })
            .error(function (data) {
                var responseMessage = data.responseText;
                try {
                    errorMessage = JSON.parse(responseMessage).ErrorMessage;
                    $("#message").text(errorMessage);
                } catch (e) { }
                $(".alert").show();
            })
            .complete(function () {
                $('#btnCreateUser').button('reset');
            });
        }
    });
});

