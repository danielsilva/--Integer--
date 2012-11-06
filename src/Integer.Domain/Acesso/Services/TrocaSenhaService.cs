using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Domain.Acesso.Exceptions;
using Integer.Infrastructure.Tasks;

namespace Integer.Domain.Acesso
{
    public class TrocaSenhaService
    {
        private Usuarios usuarios;
        private UsuarioTokens usuarioTokens;

        public TrocaSenhaService(Usuarios usuarios, UsuarioTokens usuarioTokens)
        {
            this.usuarios = usuarios;
            this.usuarioTokens = usuarioTokens;
        }

        public void EnviarSenha(string email) 
        {
            var usuario = ObterUsuario(email);
            string token = CriarToken(usuario);
            EnviarEmail(usuario, token);
        }

        private Usuario ObterUsuario(string email) 
        {
            var usuario = usuarios.Com(u => u.Email == email);
            if (usuario == null)
                throw new UsuarioInexistenteException(email);

            return usuario;
        }

        private string CriarToken(Usuario usuario)
        {
            var token = new UsuarioToken(usuario);
            usuarioTokens.Salvar(token);

            return token.Codigo.ToString();
        }

        private void EnviarEmail(Usuario usuario, string token)
        {
            //TaskExecutor.ExcuteLater(new SendEmailTask("Trocar senha", "TrocarSenha", usuario.Email, new { UserId = usuario.Id, Token = token }));
            TaskExecutor.ExcuteLater(new SendEmailTask("Trocar senha", "TrocarSenha", "danielsilva.rj@gmail.com", new { UserId = usuario.Id, Token = token }));
        }
    }
}
