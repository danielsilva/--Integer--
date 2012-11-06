using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Infrastructure.Criptografia;
using System.Linq.Expressions;

namespace Integer.Domain.Acesso
{
    public interface UsuarioTokens
    {
        UsuarioToken Com(Expression<Func<UsuarioToken, bool>> condicao);
        void Salvar(UsuarioToken usuarioToken);
    }
}
