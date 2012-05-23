$(document).ready(function () {
    $("#txtCpf").live("mascara", function () {
        $(this).setMask("999.999.999-99");
    });
    $("#txtCpf").live("keydown", function () {
        $(this).trigger("mascara");
    });

    $("#lnkEnviar").live("click", function () {
        $.ajax({
            //async: false, // comentado por causa do blockUI plugin
            data: $("form").serialize(),
            type: $("form").attr('method'),
            url: "Login",
            complete: function (XMLHttpRequest, textStatus) {
                if (XMLHttpRequest.status == 200) {
                    if (XMLHttpRequest.getResponseHeader("Location") != null
                    && XMLHttpRequest.getResponseHeader("Location") != "") {
                        window.location = XMLHttpRequest.getResponseHeader("Location");
                    }
                    else {
                        $("#divFormulario").html(XMLHttpRequest.responseText);
                    }
                }
                else if (XMLHttpRequest.status == 201) {
                    $("#divFormulario").html(XMLHttpRequest.responseText);
                }
            }
        });
    });

    $("#lnkEnviarNovaSenha").live("click", function () {
        $.ajax({
            data: $("form").serialize(),
            type: "POST",
            url: "TrocarSenha",
            complete: function (XMLHttpRequest, textStatus) {
                if (XMLHttpRequest.status == 200) {
                    if (XMLHttpRequest.getResponseHeader("Location") !== null
                        && XMLHttpRequest.getResponseHeader("Location") != "") {
                        ExibeAlerta("Sucesso", "Senha alterada com sucesso.", function () {
                            window.location = XMLHttpRequest.getResponseHeader("Location");
                        });
                    }
                }
                else if (XMLHttpRequest.status == 201) {
                    ExibeAlerta("Ops!", XMLHttpRequest.responseText);
                }
            }
        });
    });

    $("#lnkEsqueciSenha").live("click", function () {
        $.ajax({
            type: "GET",
            url: "LembrarSenha",
            complete: function (XMLHttpRequest, textStatus) {
                if (XMLHttpRequest.status == 200 || XMLHttpRequest.status == 201) {
                    $("#divFormulario").html(XMLHttpRequest.responseText);
                }
            }
        });
    });

    $("#lnkEnviarLembreteSenha").live("click", function () {
        $.ajax({
            data: $("form").serialize(),
            type: $("form").attr('method'),
            url: "LembrarSenha",
            complete: function (XMLHttpRequest, textStatus) {
                if (XMLHttpRequest.status == 200) {
                    ExibeAlerta("Sucesso", "Enviamos para seu e-mail uma mensagem de lembrete contendo sua senha.", function () {
                        $(document).html(XMLHttpRequest.responseText);
                    });
                }
                else if (XMLHttpRequest.status == 201) {
                    $("#divFormulario").html(XMLHttpRequest.responseText);
                }
            }
        });
    });

    $("#lnkCancelar").live("click", function () {
        $.ajax({
            type: "GET",
            url: 'Login',
            success: function (response) {
                $("#divFormulario").html(response);
            }
        });
    });
});