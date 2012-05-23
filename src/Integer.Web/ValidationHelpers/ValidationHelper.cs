using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Integer.Web.ValidationHelpers
{
    public class ValidationHelper
    {
        public static ValidationResult ValidarEmail(string email)
        {
            if (VerificarSeEmailEhValido(email))
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("E-mail inválido.");
        }

        private static bool VerificarSeEmailEhValido(string email)
        {
            if (String.IsNullOrEmpty(email))
            {
                return false;
            }

            return Regex.Match(email, @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$").Success;
        }
    }
}