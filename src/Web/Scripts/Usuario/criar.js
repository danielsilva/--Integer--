$().ready(function () {
    $("#btnCreateUser").live("click", function () {
        if ($("#frmUser").valid()) {
            $(this).button('loading');

            $.ajax({
                url: "/Usuario/Criar",
                type: "POST",
                dataType: "json",
                data: $("#frmUser").serialize(),
                success: function (data, status, xhr) {
                    window.location = "/";
                },
                error: function (data) {
                    var responseMessage = data.responseText;
                    if (data.status == 206) {
                        $("#frmUser").replaceWith(responseMessage);
                    }
                    else {
                        try {
                            errorMessage = JSON.parse(responseMessage).ErrorMessage;
                            $("#message").text(errorMessage);
                        } catch (e) { }
                        $(".alert").show();
                    }
                },
                complete: function () {
                    $('#btnCreateUser').button('reset');
                }
            });
        }
        return false;
    });
});

