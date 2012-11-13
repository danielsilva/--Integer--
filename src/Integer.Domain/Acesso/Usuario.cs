using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Infrastructure.Criptografia;

namespace Integer.Domain.Acesso
{
    public class Usuario
    {
        protected Usuario() { }

        public Usuario(string email, string senha, string grupoId)
        {
            this.Email = email;
            this.Senha = Encryptor.Encrypt(senha);
            Ativo = true;
            DataCriacao = DateTime.Now;
            this.GrupoId = grupoId;
            PrecisaTrocarSenha = true;
        }

        public string Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; private set; }
        public string Senha { get; private set; }
        public bool Ativo { get; private set; }
        public string GrupoId { get; set; }
        public bool PrecisaTrocarSenha { get; set; }

        public DateTime DataCriacao { get; private set; }

        public bool ValidaSenha(string senha)
        {
            return Encryptor.Encrypt(senha) == this.Senha;
        }

        public void TrocarSenha(string senha)
        {
            this.Senha = Encryptor.Encrypt(senha);
        }

        public void CriarSenha(string senha) 
        {
            this.Senha = Encryptor.Encrypt(senha);
            PrecisaTrocarSenha = false;
        }
    }
}
