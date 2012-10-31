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
using Integer.Infrastructure.DocumentModelling;
using Integer.Infrastructure.Email;
using Integer.Infrastructure.Enums;
using System.Linq.Expressions;

namespace Integer.Domain.Agenda
{
    public class Evento : INamedDocument
    {
        private const short NUMERO_MAXIMO_DE_CARACTERES_PRO_NOME = 50;
        private const short NUMERO_MAXIMO_DE_CARACTERES_PRA_DESCRICAO = 150;

        public string Id { get; set; }
        public virtual string Nome { get; set; }
        public EstadoEventoEnum Estado { get; private set; }
        public string Descricao { get; private set; }
        public DateTime DataInicio { get; private set; }
        public DateTime DataFim { get; private set; }
        public DenormalizedReference<Grupo> Grupo { get; private set; }
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
                                                  .WhenNot("Necessário informar a data de início do evento.");
            var dataFimFoiInformada = Assertion.That(dataFim != default(DateTime))
                                                  .WhenNot("Necessário informar a data de término do evento.");
            var dataInicioPrecisaSerAnteriorADataFim = Assertion.That(dataInicio < dataFim)
                                                                .WhenNot("A data de término do evento deve ser posterior à data de início.");
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

        public void Reservar(Local local, DateTime data, IList<HoraReservaEnum> horario)
        {
            this.Reservar(local, data, horario);
        }

        private void Reservar(DenormalizedReference<Local> local, DateTime data, IList<HoraReservaEnum> hora)
        {
            #region pré-condição

            var reservaComMesmoHorarioParaOLocal = Reservas.FirstOrDefault(r => r.Local.Equals(local)
                                                                                && r.Data.Equals(data)
                                                                                && r.Hora.Intersect(hora).Count() > 0);

            var naoExisteReservaSemelhante = Assertion.That(reservaComMesmoHorarioParaOLocal == null)
                                                      .WhenNot(String.Format(@"O local '{0}' foi reservado mais de uma vez para um mesmo horário. 
                                                                                Verifique se o horário '{1} ({2})' está coincidindo com outra reserva para este local neste evento.",
                                                                                local.Nome, data.ToString("dd/MM/yyyy"), hora.ToHoraReservaString()));

            #endregion
            naoExisteReservaSemelhante.Validate();

            var reserva = new Reserva(local, data, hora);

            var reservasAux = Reservas.ToList();
            reservasAux.Add(reserva);
            Reservas = reservasAux;
        }

        public void Alterar(string nome, string descricao, DateTime dataInicio, DateTime dataFim, Grupo grupo, TipoEventoEnum tipo)
        {
            bool dataInicioMudou = !this.DataInicio.Equals(dataInicio);
            bool dataFimMudou = !this.DataFim.Equals(dataFim);
            if (dataInicioMudou || dataFimMudou)
            {
                this.DataInicio = dataInicio;
                this.DataFim = dataFim;
                DomainEvents.Raise<HorarioDeEventoAlteradoEvent>(new HorarioDeEventoAlteradoEvent(this));
            }

            this.Nome = nome;
            this.Descricao = descricao;
            this.Grupo = grupo;
            this.Tipo = tipo; // TODO: disparar DomainEvent de tipo alterado (se o tipo diminuiu a prioridade, o handler deverá remover os conflitos dos eventos menos prioritários até então)
        }

        public void AlterarReservasDeLocais(IEnumerable<Reserva> reservasNovas) 
        {
            IList<Reserva> reservasAlteradas = new List<Reserva>();
            IList<Reserva> reservasCanceladas = new List<Reserva>();

            foreach (var reservaDoEvento in this.Reservas)
            {
                bool reservaDoEventoFoiAlterada = reservasNovas.Count(r => r.Local.Equals(reservaDoEvento.Local)) > 0;
                if (reservaDoEventoFoiAlterada)
                {
                    Reserva reservaAlterada = reservasNovas.First(r => r.Local.Equals(reservaDoEvento.Local));
                    if (reservaAlterada.Data != reservaDoEvento.Data
                        || reservaAlterada.Hora.Except(reservaDoEvento.Hora).Count() > 0)
                    {
                        reservaDoEvento.AlterarHorario(reservaAlterada.Data, reservaAlterada.Hora);
                        reservasAlteradas.Add(reservaDoEvento);
                    }
                }
                else 
                {
                    reservasCanceladas.Add(reservaDoEvento);
                }
            }
            var reservasAux = this.Reservas.ToList();
            foreach (var reservaCancelada in reservasCanceladas)
	        {
                reservasAux.Remove(reservaCancelada);
	        }
            this.Reservas = reservasAux;
            foreach (var reservaNova in reservasNovas)
            {
                var reserva = this.Reservas.SingleOrDefault(r => r.Local == reservaNova.Local);
                if (reserva == null)
                    this.Reservar(reservaNova.Local, reservaNova.Data, reservaNova.Hora);
            }

            if (reservasAlteradas.Count > 0)
                DomainEvents.Raise<HorarioDeReservaDeLocalAlteradoEvent>(new HorarioDeReservaDeLocalAlteradoEvent(this, reservasAlteradas));

            if (reservasCanceladas.Count > 0)
                DomainEvents.Raise<ReservaDeLocalCanceladaEvent>(new ReservaDeLocalCanceladaEvent(this, reservasCanceladas));
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

            IEnumerable<Conflito> conflitosReferentesAoEvento = this.Conflitos.Where(c => c.Evento.Equals(outroEvento));
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

        public bool PossuiConflitoDeHorarioCom(Evento outroEvento)
        {
            return Evento.MontarVerificacaoDeConflitoDeHorario(outroEvento.DataInicio, outroEvento.DataFim).Compile()(this);
        }

        public static Expression<Func<Evento, bool>> MontarVerificacaoDeConflitoDeHorario(DateTime inicio, DateTime fim) 
        {
            DateTime inicioComIntervaloMinimo = inicio.Subtract(Horario.INTERVALO_MINIMO_ENTRE_EVENTOS_E_RESERVAS);
            DateTime fimComIntervaloMinimo = fim.Add(Horario.INTERVALO_MINIMO_ENTRE_EVENTOS_E_RESERVAS);

            return ((e) => (e.DataInicio <= inicioComIntervaloMinimo && inicioComIntervaloMinimo <= e.DataFim)
                        || (e.DataInicio <= fimComIntervaloMinimo && fimComIntervaloMinimo <= e.DataFim)
                        || (inicioComIntervaloMinimo <= e.DataInicio && e.DataFim <= fimComIntervaloMinimo));
        }

        public bool VerificarSeReservasPossuemConflito(IEnumerable<Reserva> outrasReservas)
        {
            bool existeConflito = false;
            foreach (Reserva minhaReserva in this.Reservas)
            {
                foreach (Reserva outraReserva in outrasReservas)
                {
                    existeConflito |= minhaReserva.PossuiConflitoCom(outraReserva);
                    if (existeConflito)
                        break;
                }
            }
            return existeConflito;
        }
    }
}
