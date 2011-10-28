using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Integer.Domain.Agenda
{
    public static class TipoEventoEnumExtensions
    {
        public static int NivelDePrioridadeNaAgenda(this TipoEventoEnum tipoEvento) 
        {
            Dictionary<TipoEventoEnum, int> prioridades = new Dictionary<TipoEventoEnum, int>();
            prioridades.Add(TipoEventoEnum.Comum, 0);
            prioridades.Add(TipoEventoEnum.GrandeMovimentoDePessoas, 1);
            prioridades.Add(TipoEventoEnum.Sacramento, 2);
            prioridades.Add(TipoEventoEnum.Paroquial, 3);

            if (!prioridades.ContainsKey(tipoEvento))
                throw new ApplicationException("Tipo de evento não encontrado na configuração de prioridades.");

            return prioridades[tipoEvento];
        }
    }
}