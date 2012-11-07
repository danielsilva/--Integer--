﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Integer.Domain.Acesso.Exceptions
{
    public class UsuarioInexistenteException : ApplicationException
    {
        public override string Message
        {
            get
            {
                return "Usuário não encontrado";
            }
        }
    }
}
