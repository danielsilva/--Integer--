using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Integer.Domain.Acesso.Exceptions
{
    public class UsuarioInexistenteException : ApplicationException
    {
        private string email;

        public UsuarioInexistenteException(string email)
            : base()
        {
            this.email = email;
        }

        public override string Message
        {
            get
            {
                return String.Format("Não existe usuário cadastrado com o e-mail '{0}'.", email);
            }
        }
    }
}
