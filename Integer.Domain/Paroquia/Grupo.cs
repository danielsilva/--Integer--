using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Infrastructure.Validation;

namespace Integer.Domain.Paroquia
{
    public class Grupo
    {
        private const short NUMERO_MAXIMO_DE_CARACTERES_PRO_NOME = 50;

        public short Id { get; private set; }
        public string Nome { get; private set; }

        public Grupo(string nome)
        {
            PreencherNome(nome);
        }

        private void PreencherNome(string nome)
        {
            if (nome == null || nome.Trim() == "")
                throw new ValidationException("Necessário informar o nome do grupo.");

            if (nome.Trim().Length > NUMERO_MAXIMO_DE_CARACTERES_PRO_NOME)
                throw new ValidationException(String.Format("O nome do grupo não pode ultrapassar o tamanho de {0} caracteres.", 
                                                NUMERO_MAXIMO_DE_CARACTERES_PRO_NOME));

            Nome = nome.Trim();
        }
    }
}
