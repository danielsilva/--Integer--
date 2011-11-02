using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Domain.Paroquia;
using Integer.Infrastructure.Validation;
using DbC;
using Integer.Infrastructure.DateAndTime;
using Integer.Infrastructure.Events;
using System.Collections;

namespace Integer.Domain.Agenda
{
    public class Evento
    {
        private const short NUMERO_MAXIMO_DE_CARACTERES_PRO_NOME = 50;
        private const short NUMERO_MAXIMO_DE_CARACTERES_PRA_DESCRICAO = 150;

        public virtual string Nome { get; protected set; }
        public EstadoEventoEnum Estado { get; private set; }
        public string Descricao { get; private set; }
        public DateTime DataInicio { get; private set; }
        public DateTime DataFim { get; private set; }
        public Grupo Grupo { get; private set; }
        public TipoEventoEnum Tipo { get; private set; }
        public DateTime DataCadastro { get; private set; }
        public IEnumerable<Conflito> Conflitos { get; private set; }
        public IEnumerable<Reserva> Reservas { get; private set; }

        public virtual Horario Horario
        {
            get
            {
                return new Horario(this.DataInicio, this.DataFim);
            }
        }

        protected Evento() { }

        public Evento(string nome, string descricao, DateTime dataInicioEvento, DateTime dataFimEvento, Grupo grupo, TipoEventoEnum tipoDoEvento)
        {
            Preencher(nome, descricao, dataInicioEvento, dataFimEvento, grupo, tipoDoEvento);
            DataCadastro = SystemTime.Now();
            Estado = EstadoEventoEnum.Agendado;
            Conflitos = new List<Conflito>();
            Reservas = new List<Reserva>();
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
            #region pré-condição
            var nomeFoiInformado = Assertion.That(!String.IsNullOrWhiteSpace(nome))
                                            .WhenNot("Necessário informar o nome do evento.");
            var nomePossuiQuantidadeDeCaracteresValida = Assertion.That(nome != null && nome.Trim().Length <= NUMERO_MAXIMO_DE_CARACTERES_PRO_NOME)
                                                                  .WhenNot(String.Format("O nome do evento não pode ultrapassar o tamanho de {0} caracteres.",
                                                                            NUMERO_MAXIMO_DE_CARACTERES_PRO_NOME));
            #endregion
            (nomeFoiInformado && nomePossuiQuantidadeDeCaracteresValida).Validate();

            Nome = nome.Trim();
        }

        private void PreencherDescricao(string descricao)
        {
            if (String.IsNullOrEmpty(descricao))
                return;

            #region pré-condição
            var descricaoPossuiQuantidadeValidaDeCaracteres 
                = Assertion.That(descricao.Trim().Length <= NUMERO_MAXIMO_DE_CARACTERES_PRA_DESCRICAO)
                           .WhenNot(String.Format("A descrição do evento não pode ultrapassar o tamanho de {0} caracteres.",
                                NUMERO_MAXIMO_DE_CARACTERES_PRA_DESCRICAO));
            #endregion
            descricaoPossuiQuantidadeValidaDeCaracteres.Validate();

            Descricao = descricao.Trim();
        }

        private void PreencherDatas(DateTime dataInicio, DateTime dataFim)
        {
            #region pré-condição
            var dataInicioFoiInformada = Assertion.That(dataInicio != default(DateTime))
                                                  .WhenNot("Necessário informar a <Data Início> do evento.");
            var dataFimFoiInformada = Assertion.That(dataFim != default(DateTime))
                                                  .WhenNot("Necessário informar a <Data Fim> do evento.");
            var dataInicioPrecisaSerAnteriorADataFim = Assertion.That(dataInicio < dataFim)
                                                                .WhenNot("A <Data Fim> do evento deve ser posterior à data de início.");
            #endregion
            ((dataInicioFoiInformada & dataFimFoiInformada) & dataInicioPrecisaSerAnteriorADataFim).Validate();            
            
            DataInicio = dataInicio;
            DataFim = dataFim;
        }

        private void PreencherGrupo(Grupo grupo)
        {
            #region pré-condição
            var grupoFoiInformado = Assertion.That(grupo != null).WhenNot("Necessário informar o grupo que promoverá o evento.");
            #endregion
            grupoFoiInformado.Validate();

            Grupo = grupo;
        }

        private void PreencherTipo(TipoEventoEnum tipoDoEvento)
        {
            #region pré-condição
            var tipoFoiInformado = Assertion.That(tipoDoEvento != default(TipoEventoEnum))
                                            .WhenNot("Necessário classificar o tipo do evento.");
            #endregion
            tipoFoiInformado.Validate();

            Tipo = tipoDoEvento;
        }

        public void AdicionarConflito(Evento outroEvento, MotivoConflitoEnum motivo)
        {
            #region pré-condição

            var outroEventoNaoEhNulo = Assertion.That(outroEvento != null).WhenNot("Erro ao tentar adicionar conflito com evento nulo.");

            #endregion
            outroEventoNaoEhNulo.Validate();

            this.Estado = EstadoEventoEnum.NaoAgendado;

            if (Conflitos == null)
                Conflitos = new List<Conflito>();

            int quantidadeDeConflitosAntes = Conflitos.Count();

            var conflitosAux = Conflitos.ToList();
            conflitosAux.Add(new Conflito(outroEvento, motivo));
            Conflitos = conflitosAux;

            #region pós-condição

            var aumentouAQuantidadeDeConflitos = Assertion.That(quantidadeDeConflitosAntes + 1 == Conflitos.Count())
                                                          .WhenNot("Erro ao adicionar conflitos ao evento. Quantidade não foi incrementada.");

            #endregion
            aumentouAQuantidadeDeConflitos.Validate();
        }

        public void Reservar(Local local, DateTime dataInicio, DateTime dataFim)
        {
            #region pré-condição

            var horarioDesejado = new Horario(dataInicio, dataFim);
            var reservaComMesmoHorarioParaOLocal = Reservas.FirstOrDefault(r => r.Local == local 
                                                                                && r.Horario.VerificarConcorrencia(horarioDesejado));

            var naoExisteReservaSemelhante = Assertion.That(reservaComMesmoHorarioParaOLocal == null)
                                                      .WhenNot(String.Format(@"O local '{0}' foi reservado mais de uma vez para um mesmo horário. 
                                                                                Verifique se o horário {1} está coincidindo com outra reserva para este local neste evento.", 
                                                                                local.Nome, horarioDesejado.ToString()));
            
            #endregion
            naoExisteReservaSemelhante.Validate();

            var reserva = new Reserva(local, dataInicio, dataFim);

            var reservasAux = Reservas.ToList();
            reservasAux.Add(reserva);
            Reservas = reservasAux;
        }

        public void AlterarReservasDeLocais(IDictionary<Local, Horario> reservasNovas) 
        {
            foreach (var reservaDoEvento in this.Reservas)
            {
                bool reservaDoEventoFoiAlterada = reservasNovas.ContainsKey(reservaDoEvento.Local);
                if (reservaDoEventoFoiAlterada) 
                {
                    Horario novoHorario = reservasNovas[reservaDoEvento.Local];
                    if (novoHorario != reservaDoEvento.Horario) 
                    {
                        reservaDoEvento.AlterarHorario(novoHorario);
                        DomainEvents.Raise<HorarioDeReservaDeLocalAlteradoEvent>(new HorarioDeReservaDeLocalAlteradoEvent(this, reservaDoEvento));
                    }
                }
            }
        }

        public bool PossuiPrioridadeSobre(Evento outroEvento)
        {
            bool esteFoiCadastradoAntes = DateTime.Compare(this.DataCadastro, outroEvento.DataCadastro) == -1;

            return this.Tipo.NivelDePrioridadeNaAgenda() > outroEvento.Tipo.NivelDePrioridadeNaAgenda()
                    || (this.Tipo.NivelDePrioridadeNaAgenda() == outroEvento.Tipo.NivelDePrioridadeNaAgenda() && esteFoiCadastradoAntes);
        }

        public void CancelarAgendamento()
        {
            this.Estado = EstadoEventoEnum.Cancelado;
            DomainEvents.Raise<EventoCanceladoEvent>(new EventoCanceladoEvent(this));
        }

        public void RemoverConflitoCom(Evento outroEvento)
        {
            #region pré-condição
            Assertion outroEventNaoEhNulo = Assertion.That(outroEvento != null).WhenNot("Não foi possível remover conflitos referentes ao evento. Referência nula.");
            #endregion
            outroEventNaoEhNulo.Validate();

            IEnumerable<Conflito> conflitosReferentesAoEvento = this.Conflitos.Where(c => c.Evento == outroEvento);
            if (conflitosReferentesAoEvento != null)
            {
                foreach (Conflito conflito in conflitosReferentesAoEvento)
                {
                    IList<Conflito> ConflitosAux = this.Conflitos.ToList();
                    ConflitosAux.Remove(conflito);

                    this.Conflitos = ConflitosAux;
                }
            }
            if (this.Conflitos.Count() == 0)
            {
                this.Estado = EstadoEventoEnum.Agendado;
                // TODO: enviar e-mail avisando que o evento voltou a ficar agendado
            }
        }
    }
}
