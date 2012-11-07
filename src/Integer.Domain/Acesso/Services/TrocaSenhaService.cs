using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Domain.Acesso.Exceptions;
using Integer.Infrastructure.Email.TemplateModels;
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
                throw new UsuarioInexistenteException();

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
            TaskExecutor.ExcuteLater(new SendEmailTask("Trocar senha", "TrocarSenha", usuario.Email, new TrocarSenhaModel { UserId = usuario.Id, Token = token }));
        }

        public bool ValidarToken(string usuarioId, Guid token) 
        {
            UsuarioToken usuarioToken = usuarioTokens.Com(u => u.UsuarioId == usuarioId && u.Codigo == token);
            return usuarioToken.EstaValido;
        }

        public void DesativarToken(string usuarioId, Guid token)
        {
            UsuarioToken usuarioToken = usuarioTokens.Com(u => u.UsuarioId == usuarioId && u.Codigo == token);
            usuarioToken.Desativar();
        }

        public void TrocarSenha(Guid token, string usuarioId, string senha)
        {
            UsuarioToken usuarioToken = usuarioTokens.Com(u => u.UsuarioId == usuarioId && u.Codigo == token);
            if (!usuarioToken.EstaValido)
                throw new UsuarioTokenExpiradoException();

            var usuario = usuarios.Com(u => u.Id == usuarioId);
            if (usuario == null)
                throw new UsuarioInexistenteException();

            usuario.TrocarSenha(senha);
            usuarioToken.Desativar();
        }
    }
}
