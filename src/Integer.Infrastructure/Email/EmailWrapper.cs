using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.ComponentModel;
using System.Net;

namespace Integer.Infrastructure.Email
{
    public class EmailWrapper
    {
        private Queue<MailMessage> EmailsAgendados = new Queue<MailMessage>();

        public void AgendarEmail(string destinatario, string assunto, string corpo)
        {
            AgendarEmail(new List<string> { destinatario }, assunto, corpo);
        }

        public void AgendarEmail(List<string> destinatarios, string assunto, string corpo)
        {
            var msg = MontarEmail(destinatarios, assunto, corpo);
            EmailsAgendados.Enqueue(msg);
        }

        public void EnviarEmailsAgendados()
        {
            while (EmailsAgendados.Count > 0)
            {
                EnviarEmail(EmailsAgendados.Dequeue());
            }
        }

        public static void EnviarEmail(string destinatario, string assunto, string corpo)
        {
            var msg = MontarEmail(new List<string>() { destinatario }, assunto, corpo);
            EnviarEmail(msg);
        }

        private static void EnviarEmail(MailMessage msg)
        {
            var smtp = new SmtpClient("mail.carnation.arvixe.com");
            smtp.Credentials = new NetworkCredential("no-reply@ideal.eti.br", "TrocA2@");

            smtp.SendCompleted += new SendCompletedEventHandler(SmtpClient_OnCompleted);
            smtp.SendAsync(msg, null);
        }

        private static MailMessage MontarEmail(List<string> destinatarios, string assunto, string corpo)
        {
            var msg = new MailMessage();

            msg.From = new MailAddress("{ Integer } - Divino Salvador <no-reply@ideal.eti.br>");
            foreach (string destinatario in destinatarios)
            {
                msg.To.Add(destinatario);
            }

            msg.Subject = assunto;
            msg.Body = corpo;
            msg.IsBodyHtml = true;

            msg.SubjectEncoding = Encoding.GetEncoding("ISO-8859-1");
            msg.BodyEncoding = Encoding.GetEncoding("ISO-8859-1");

            return msg;
        }

        private static void SmtpClient_OnCompleted(object sender, AsyncCompletedEventArgs e)
        { 
            // TODO tratar e-mail não enviado
        }
    }
}
