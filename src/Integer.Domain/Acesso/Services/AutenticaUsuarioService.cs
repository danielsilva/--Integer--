using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Integer.Domain.Paroquia;
using Integer.Infrastructure.Criptografia;

namespace Integer.Domain.Acesso.Services
{
    public class AutenticaUsuarioService
    {
        private readonly Grupos grupos;
        private readonly Usuarios usuarios;

        public AutenticaUsuarioService(Grupos grupos, Usuarios usuarios)
        {
            this.grupos = grupos;
            this.usuarios = usuarios;
        }

        public Usuario Autenticar(string email, string senha)
        {
            Usuario usuario = null;

            Grupo grupo = grupos.Com(g => g.Email == email);
            if (grupo != null && grupo.PrecisaCriarUsuario)
            {
                if (grupo.ValidarSenha(senha))
                {
                    usuario = new Usuario(grupo.Nome, email, senha, grupo.Id);
                    usuarios.Salvar(usuario);
                }
            }
            else
            {
                usuario = usuarios.Com(u => u.Email == email && u.Senha == Encryptor.Encrypt(senha));
            }
            return usuario;
        }
    }
}
