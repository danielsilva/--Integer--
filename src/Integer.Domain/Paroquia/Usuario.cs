using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Infrastructure.Criptografia;

namespace Integer.Domain.Paroquia
{
    public class Usuario
    {
        public Usuario(string email, string senha)
        {
            this.Email = email;
            this.Senha = Encryptor.Encrypt(senha);
            Ativo = true;
            DataCriacao = DateTime.Now;
        }

        public string Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; private set; }
        public string Senha { get; private set; }
        public bool Ativo { get; private set; }

        public DateTime DataCriacao { get; private set; }

        public bool ValidaSenha(string senha)
        {
            return Encryptor.Encrypt(senha) == this.Senha;
        }
    }
}
