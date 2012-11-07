$(document).ready(function () {

    $("#lnkSalvar").live("click", function () {
        $.ajax({
            data: $("form").serialize(),
            type: $("form").attr('method'),
            url: "Grupo/Salvar",
            complete: function (XMLHttpRequest, textStatus) 
            {
                if (XMLHttpRequest.status == 200) 
                {
                    var erro = (XMLHttpRequest.getResponseHeader("ERRO") != null
                                && XMLHttpRequest.getResponseHeader("ERRO") != "") ? XMLHttpRequest.getResponseHeader("ERRO") : "";
                    if (erro != "") 
                    {
                        ExibeAlerta("Ops!", erro, function () { });
                    }
                    else 
                    {
                        var acao = $("#hddId").val() == 0 ? "agendado" : "alterado";
                        ExibeAlerta("Sucesso", "Grupo " + acao + " com sucesso. \n", function () { });
                    }
                }
                else if (XMLHttpRequest.status == 201) {
                    $("#divFormulario").html(XMLHttpRequest.responseText);
                }
            }
        });
    });

});