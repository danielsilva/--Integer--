using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Integer.Domain.Acesso.Exceptions
{
    public class UsuarioTokenExpiradoException : ApplicationException
    {
        public override string Message
        {
            get
            {
                return "Chave expirada. Solicite a troca de senha outra vez.";
            }
        }
    }
}
