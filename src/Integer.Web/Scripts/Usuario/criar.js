$().ready(function () {
    $("#btnCreateUser").live("click", function () {
        if ($("#frmUser").valid()) {
            $(this).button('loading');

            $.post("/Usuario/Criar", $("#frmUser").serialize())
            .success(function (data) {
                window.location = "/";
            })
            .error(function (data) {
                if (data.Error)
                    $("#frmUser").parent().html(data.responseText);
                else
                    $("#frmUser").parent().html(data.responseText);
            })
            .complete(function () {
                $('#btnCreateUser').button('reset');
            });
        }
    });
});

