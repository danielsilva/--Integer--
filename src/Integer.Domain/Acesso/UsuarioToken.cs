using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Infrastructure.Criptografia;

namespace Integer.Domain.Acesso
{
    public class UsuarioToken
    {
        protected UsuarioToken()
        {
        }

        public UsuarioToken(Usuario usuario)
        {
            this.UsuarioId = usuario.Id;
            this.Codigo = Guid.NewGuid();
            this.Validade = DateTime.Now.AddMinutes(10);
        }

        public string UsuarioId { get; private set; }
        public Guid Codigo { get; private set; }
        private DateTime Validade { get; set; }

        public bool EstaValido 
        {
            get 
            {
                return DateTime.Compare(DateTime.Now, Validade) < 0;
            }
        }

        public void Desativar()
        {
            this.Validade = DateTime.MinValue;
        }
    }
}
