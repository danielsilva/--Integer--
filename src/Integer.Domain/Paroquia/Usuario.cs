using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Infrastructure.Criptografia;

namespace Integer.Domain.Paroquia
{
    public class Usuario
    {
        public Usuario()
        {
            Ativo = true;
            DataCriacao = DateTime.Now;
        }

        public string Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public bool Ativo { get; private set; }

        public DateTime DataCriacao { get; private set; }

        public bool ValidaSenha(string senha)
        {
            return Encryptor.Encrypt(senha) == this.Senha;
        }
    }
}
