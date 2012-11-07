$(document).ready(function () {

    $("#lnkCopiar").live("click", function() {
        $("#hddIdEvento").val(0);
        $(this).hide("highlight", null, 500);
        $("#imgInfoCopiar").hide("highlight", null, 500);
        $("#divDataCadastro").hide("highlight", null, 500);
    });

    $(".dateTimePicker").live("click", function () {
        var dataPadrao = new Date() < new Date(2011,0,1) ? new Date(2011,0,1) : new Date();
        $(this).datetimepicker({
            defaultDate: dataPadrao, 
            changeYear: false,
            minDate: new Date(dataPadrao.getFullYear(), 0, 1),
            maxDate: new Date(dataPadrao.getFullYear(), 11, 31),
            });
        $(this).datetimepicker("show");
    });

    $("input[name=DataInicio]").live("change", function() {
            if ($("input[name=DataFim]").val() == "") {
                $("input[name=DataFim]").val($(this).val());
            }
        });

    $("#lstPublicoAlvo").live("change", function () {
        OrganizaListaPublicoAlvo();
    });

    $("#lnkReservarLocais").live("click", function () {
        var indice = parseInt($("#hiddenIndiceReserva").val());
        $.ajax({
            async: false, // comentado por causa do blockUI plugin
            url: "/Calendario/AdicionarReserva",
            type: "GET",
            data: { IndiceReserva: indice,
                DataInicio: $("#Evento_DataInicio").val(),
                DataFim: $("#Evento_DataFim").val()
            },
            success: function (html) {
                $('#Reservas').append(html);
                $("#hiddenIndiceReserva").val(indice++);
            }
        });
        OrganizaListaReservas();
    });

    $("#lnkSalvar").live("click", function () {
        OrganizaListaPublicoAlvo();
        $.ajax({
            async: false,
            data: $("form").serialize(),
            type: $("form").attr('method'),
            url: "/Calendario/SalvarEvento",
            complete: function (XMLHttpRequest, textStatus) {
                if (XMLHttpRequest.status == 200) {
                    var erro = (XMLHttpRequest.getResponseHeader("ERRO") != null && XMLHttpRequest.getResponseHeader("ERRO") != "") ? XMLHttpRequest.getResponseHeader("ERRO") : "";
                    if (erro != "") {
                        ExibeAlerta("Ops! Não foi possível agendar seu evento.", erro, function () {
                        });
                    }
                    else {
                        var acao = $("#hddIdEvento").val() == 0 ? "agendado" : "alterado";
                        ExibeAlerta("Sucesso", "Evento " + acao + " com sucesso. \n", function () {
                            CalendarioAlterado();
                        });
                    }
                }
                else if (XMLHttpRequest.status == 201) {
                    $("#divEvento").html(XMLHttpRequest.responseText);
                }
            }
        });
    });

    $("#imgInfoDescricao").live("mousemove", function () {
        ExibeHelpTip($(this), '<u>Recomendado</u> para eventos abertos ao público. Podem ser descritas informações como: valor de inscrição, atrações, telefone para contato, material que a pessoa precisa trazer consigo etc.');
    });
    $("#imgInfoPublicoAlvo").live("mousemove", function () {
        ExibeHelpTip($(this), '<p>Ao marcar a opção \'Todos\', não é necessário selecionar as opções restantes.</p> <p>Caso o público-alvo seja específico, selecione as outras opções.</p><p>Para escolher mais de um público-alvo, mantenha a tecla \'Ctrl\' pressionada enquanto seleciona as opções.</p>');
    });
    $("#imgInfoDataHoraEvento").live("mousemove", function () {
        ExibeHelpTip($(this), 'São as datas de <u>divulgação</u> do evento. Caso ele dure mais do que um dia, o \'Início\' corresponde à data/hora de início do primeiro dia e o \'Fim\' corresponde à data/hora final do último dia (as datas de reserva de locais estarão dentro deste intervalo).');
    });
    $("#imgInfoReservarLocal").live("mousemove", function () {
        ExibeHelpTip($(this), 'Para reservar diversos locais, clique diversas vezes no \'link\' ao lado.');
    });
    $("#imgInfoCopiar").live("mousemove", function() {
        $(this).bt(
            '<p>Ao clicar em \'Copiar\', você indicará que deseja aproveitar as informações deste evento para agendar um <b>novo</b> com alguns dados diferentes (local, data/hora ou qualquer outro que você alterar).',
            {
                fill: 'rgba(255, 255, 255, 1)',
                strokeWidth: 2,
                strokeStyle: '#274EBD',
                trigger: ['mousemove', 'mouseout'],
                positions: ['left']
            });
    });
});

function OrganizaListaReservas() {
    var i = 0;
    $.each($("div[id^=linhaReserva]"), function () {
        $(this).attr("id", "linhaReserva" + i);
        $(this).find("[id^=ddlLocalEvento]").attr("id", "ddlLocalEvento" + i).attr("name", "Reservas[" + i + "].Local");
        $(this).find("[id^=txtDataInicio]").attr("id", "txtDataInicio" + i).attr("name", "Reservas[" + i + "].DataInicio");
        $(this).find("[id^=txtDataFim]").attr("id", "txtDataFim" + i).attr("name", "Reservas[" + i + "].DataFim");
        $(this).find("[id^=lnkCancelaReserva]").attr("id", "lnkCancelaReserva" + i);
        i++;
    });
    $("#hiddenIndiceReserva").val(i);
}

function OrganizaListaPublicoAlvo() {
    $.each($("input[id^=hddPublicoAlvo]"), function () {
        $(this).remove();
    });
    $.each($('#lstPublicoAlvo option'), function (i, data) {
        if ($(this).attr("selected")) {
            $("#divListaPublicoAlvo").append("<input type='hidden' id='hddPublicoAlvo' value='" + $(this).attr("value") + "' />");
        }
    });
    //        if (!$("input[id^=hddPublicoAlvo]").exists()) {
    //            $("#divListaPublicoAlvo").append("<input type='hidden' id='hddPublicoAlvo' value='0' />");
    //        }

    var i = 0;
    $.each($("input[id^=hddPublicoAlvo]"), function () {
        $(this).attr("id", "hddPublicoAlvo" + i).attr("name", "PublicoAlvo[" + i + "].Id");
        i++
    });
}

function CancelarEvento() {
    ExibeConfirmacao("Deseja cancelar o evento?", null, function(confirma) {
        if (confirma) {
            $.ajax({
                url: "/Calendario/CancelarEvento",
                type: "POST",
                data: { idEvento: $("#hddIdEvento").val() },
                complete: function(XMLHttpRequest, textStatus) {
                    if (XMLHttpRequest.status == 200) {
                        ExibeAlerta("Sucesso", "Evento cancelado com sucesso.", function() {
                            CalendarioAlterado();
                        });
                    }
                    else if (XMLHttpRequest.status == 201) {
                        $("#divEvento").html(XMLHttpRequest.responseText);
                    }
                }
            });
        }
    });
}

function EditarEvento(idEvento) {
    $('#divEvento').load("/Calendario/EditarEvento?id=" + idEvento).queue(function() {
        $(this).dialog("open");
        $(this).dequeue();
    });
}