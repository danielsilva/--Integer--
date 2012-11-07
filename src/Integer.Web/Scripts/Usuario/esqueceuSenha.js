﻿$().ready(function () {
    $("#btnSend").live("click", function () {
        if ($("#frmEmail").valid()) {
            $(this).button('loading');

            $.post("/Usuario/EsqueceuSenha", $("#frmEmail").serialize())
            .success(function (data) {
                $("#message").text("Acabamos de lhe enviar um e-mail contendo as instruções para a troca de senha.");
                $(".alert").removeClass().addClass("alert alert-success").show();
            })
            .error(function (data) {
                console.log(data);
                var responseMessage = data.responseText;
                try {
                    errorMessage = JSON.parse(responseMessage).ErrorMessage;
                    $("#message").text(errorMessage);
                } catch (e) { }
                $(".alert").removeClass().addClass("alert alert-error").show();
            })
            .complete(function () {
                $('#btnSend').button('reset');
            });
        }
    });
});

