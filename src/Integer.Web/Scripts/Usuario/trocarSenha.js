$().ready(function () {
    $("#btnSend").live("click", function () {
        if ($("#frmEmail").valid()) {
            $(this).button('loading');

            $.post("/Usuario/TrocarSenha", $("#frmEmail").serialize())
            .success(function (data) {
                window.location = "/";
            })
            .error(function (data) {
                console.log(data);
                var responseMessage = data.responseText;
                try {
                    errorMessage = JSON.parse(responseMessage).ErrorMessage;
                    $("#message").text(errorMessage);
                } catch (e) { }
                $(".alert").show();
            })
            .complete(function () {
                $('#btnSend').button('reset');
            });
        }
    });
});

