using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Infrastructure.Validation;
using System.Diagnostics.Contracts;
using DbC;
using Integer.Infrastructure.DocumentModelling;

namespace Integer.Domain.Paroquia
{
    public class Grupo : INamedDocument
    {
        private const short NUMERO_MAXIMO_DE_CARACTERES_PRO_NOME = 50;

        public string Id { get; set; }
        public string Nome { get; set; }

        protected Grupo() { }

        public Grupo(string nome)
        {
            PreencherNome(nome);
        }

        private void PreencherNome(string nome)
        {
            #region pré-condição
            var nomeFoiInformado = Assertion.That(!String.IsNullOrWhiteSpace(nome))
                                            .WhenNot("Necessário informar o nome do grupo.");
            var nomePossuiQuantidadeDeCaracteresValida = Assertion.That(nome != null && nome.Trim().Length <= NUMERO_MAXIMO_DE_CARACTERES_PRO_NOME)
                                                                  .WhenNot(String.Format("O nome do grupo não pode ultrapassar o tamanho de {0} caracteres.", 
                                                                                         NUMERO_MAXIMO_DE_CARACTERES_PRO_NOME.ToString()));
            #endregion
            (nomeFoiInformado & nomePossuiQuantidadeDeCaracteresValida).Validate();
            
            Nome = nome.Trim();
        }
    }
}
