using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Integer.Domain.Paroquia
{
    public class UsuarioExistenteException : ApplicationException
    {
        private Usuario usuario;

        public UsuarioExistenteException(Usuario usuario) : base()
        {
            this.usuario = usuario;
        }

        public override string Message
        {
            get
            {
                return String.Format("Já existe um usuário cadastrado com o e-mail '{0}'.", usuario.Email);
            }
        }
    }
}
