using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Integer.Domain.Agenda;
using DbC;

namespace Integer.UnitTests.Domain.Agenda
{
    [TestFixture]
    public class HorarioTest
    {
        [Test]
        [ExpectedException(typeof(DbCException))]
        public void Construtor_VerificaSeDataInicioEhMenorQueDataFim_DisparandoExcecao() 
        {
            var dataInicio = DateTime.Now;
            var dataFim = dataInicio.AddHours(-1);

            new Horario(dataInicio, dataFim);
        }
        
        [Test]
        public void ToString_QuandoComecaETerminaNoMesmoDia_SeparaSomenteAHora() 
        {
            var dataInicio = new DateTime(2011, 2, 1, 8, 0, 0);
            var dataFim = dataInicio.AddHours(2);

            var horario = new Horario(dataInicio, dataFim);

            Assert.AreEqual("01/02/2011 08:00 até 10:00", horario.ToString());
        }

        [Test]
        public void ToString_QuandoTerminaEmOutroDia_SeparaDiaEHora() 
        {
            var dataInicio = new DateTime(2011, 2, 1, 8, 0, 0);
            var dataFim = dataInicio.AddDays(1);

            var horario = new Horario(dataInicio, dataFim);

            Assert.AreEqual("01/02/2011 08:00 até 02/02/2011 08:00", horario.ToString());
        }

        [Test]
        public void VerificarConcorrencia_QuandoHorarioSobrepoe_RetornaTrue() 
        {
            var dataInicio = DateTime.Now;
            var dataFim = dataInicio.AddHours(1);

            var horarioPrincipal = new Horario(dataInicio, dataFim);
            var outroHorario = new Horario(dataInicio.AddMinutes(10), dataFim.AddMinutes(10));

            Assert.IsTrue(horarioPrincipal.VerificarConcorrencia(outroHorario));
        }

        [Test]
        public void VerificarConcorrencia_QuandoHorarioNaoSobrepoe_RetornaFalse() 
        {
            var dataInicio = DateTime.Now;
            var dataFim = dataInicio.AddHours(1);

            var horarioPrincipal = new Horario(dataInicio, dataFim);
            var outroHorario = new Horario(dataFim.AddHours(1), dataFim.AddHours(2));

            Assert.IsFalse(horarioPrincipal.VerificarConcorrencia(outroHorario));
        }
    }
}
