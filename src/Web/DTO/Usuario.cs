using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.DTO
{
    public class Usuario
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public bool PrecisaTrocarSenha { get; set; }
    }
}