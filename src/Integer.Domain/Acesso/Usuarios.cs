using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Infrastructure.Criptografia;
using System.Linq.Expressions;

namespace Integer.Domain.Acesso
{
    public interface Usuarios
    {
        Usuario Com(Expression<Func<Usuario, bool>> condicao);

        void Salvar(Usuario usuario);
    }
}
