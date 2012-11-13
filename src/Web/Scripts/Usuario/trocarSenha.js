$().ready(function () {
    $("#hddId").val(getFromQueryString("id"));
    $("#hddToken").val(getFromQueryString("token"));

    $("#btnSend").live("click", function () {
        if ($("#frmSenha").valid()) {
            $(this).button('loading');

            $.post("/Usuario/TrocarSenha", $("#frmSenha").serialize())
            .success(function (data, status, xhr) {
                if (xhr.status == 206) {
                    $("#frmSenha").replaceWith(xhr.responseText);
                }
                else {
                    $("#message").html("Sua senha foi trocada com sucesso. <a href='/Login'>Clique aqui para fazer o login</a>");
                    $(".alert").removeClass().addClass("alert alert-success").show();
                }
            })
            .error(function (data) {
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
        return false;
    });
});

