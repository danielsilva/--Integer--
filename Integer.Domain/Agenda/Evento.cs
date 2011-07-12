using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Domain.Paroquia;
using Integer.Infrastructure.Validation;

namespace Integer.Domain.Agenda
{
    public class Evento
    {
        private const short NUMERO_MAXIMO_DE_CARACTERES_PRO_NOME = 50;
        private const short NUMERO_MAXIMO_DE_CARACTERES_PRA_DESCRICAO = 150;

        public string Nome { get; private set; }
        public string Descricao { get; private set; }
        public DateTime DataInicio { get; private set; }
        public DateTime DataFim { get; private set; }
        public Grupo Grupo { get; private set; }
        public TipoEventoEnum Tipo { get; private set; }
        public DateTime DataCadastro { get; private set; }

        public Evento(string nome, string descricao, DateTime dataInicioEvento, DateTime dataFimEvento, Grupo grupo, TipoEventoEnum tipoDoEvento)
        {
            Preencher(nome, descricao, dataInicioEvento, dataFimEvento, grupo, tipoDoEvento);
            DataCadastro = DateTime.Now;
        }

        private void Preencher(string nome, string descricao, 
                                                DateTime dataInicioEvento, DateTime dataFimEvento, 
                                                Grupo grupo, TipoEventoEnum tipoDoEvento)
        {
            PreencherNome(nome);
            PreencherDescricao(descricao);
            PreencherDatas(dataInicioEvento, dataFimEvento);
            PreencherGrupo(grupo);
            PreencherTipo(tipoDoEvento);
        }

        private void PreencherNome(string nome)
        {
            if (nome == null || nome.Trim() == "")
                throw new ValidationException("Necessário informar o nome do evento.");

            if (nome.Trim().Length > NUMERO_MAXIMO_DE_CARACTERES_PRO_NOME)                
                throw new ValidationException(String.Format("O nome do evento não pode ultrapassar o tamanho de {0} caracteres.", 
                                                NUMERO_MAXIMO_DE_CARACTERES_PRO_NOME));

            Nome = nome.Trim();
        }

        private void PreencherDescricao(string descricao)
        {
            if (descricao.Trim().Length > NUMERO_MAXIMO_DE_CARACTERES_PRA_DESCRICAO)
            {
                throw new ValidationException(String.Format("A descrição do evento não pode ultrapassar o tamanho de {0} caracteres.", 
                                                NUMERO_MAXIMO_DE_CARACTERES_PRA_DESCRICAO));
            }
            Descricao = descricao.Trim();
        }

        private void PreencherDatas(DateTime dataInicioEvento, DateTime dataFimEvento)
        {
            if (dataInicioEvento == default(DateTime)) 
                throw new ValidationException("Necessário informada da <Data Início> do evento.");

            if (dataFimEvento == default(DateTime)) 
                throw new ValidationException("Necessário informada da <Data Fim> do evento.");

            if (dataFimEvento < dataInicioEvento) 
                throw new ValidationException("A <Data Fim> do evento deve ser posterior à data de início.");

            DataInicio = dataInicioEvento;
            DataFim = dataFimEvento;
        }

        private void PreencherGrupo(Grupo grupo)
        {
            if (grupo == null) 
                throw new ValidationException("Necessário informar o grupo que promoverá o evento.");

            Grupo = grupo;
        }

        private void PreencherTipo(TipoEventoEnum tipoDoEvento)
        {
            if (tipoDoEvento == default(TipoEventoEnum))
                throw new ValidationException("Necessário classificar o tipo do evento.");

            Tipo = tipoDoEvento;
        }
    }
}
