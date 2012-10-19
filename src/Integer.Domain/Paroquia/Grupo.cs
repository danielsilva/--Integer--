using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Infrastructure.Validation;
using System.Diagnostics.Contracts;
using DbC;
using Integer.Infrastructure.DocumentModelling;
using Integer.Infrastructure.Criptografia;

namespace Integer.Domain.Paroquia
{
    public class Grupo : INamedDocument
    {
        private const short NUMERO_MAXIMO_DE_CARACTERES_PRO_NOME = 50;

        public virtual string Id { get; set; }
        public virtual string Nome { get; set; }
        public string Email { get; private set; }
        public string Senha { get; private set; }
        public bool PrecisaCriarUsuario { get; set; }
        public DenormalizedReference<Grupo> GrupoPai { get; private set; }
        public IEnumerable<DenormalizedReference<Grupo>> GruposFilhos { get; private set; }

        public string SenhaDescriptografada
        {
            get
            {
                string senhaLimpa = "";
                if (!String.IsNullOrEmpty(Senha))
                {
                    senhaLimpa = Encryptor.Decrypt(Senha);
                }
                return senhaLimpa;
            }
        }

        protected Grupo() 
        {
            this.GruposFilhos = new List<DenormalizedReference<Grupo>>();
        }

        public Grupo(string nome, string email, Grupo grupoPai)
        {
            PreencherNome(nome);
            PreencherGrupoPai(grupoPai);

            // TODO validar email
            this.Email = email;

            CriarSenhaPadrao();
        }

        public void PreencherNome(string nome)
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

        private void PreencherGrupoPai(Grupo grupo)
        {
            this.GrupoPai = grupo;
        }

        private void CriarSenhaPadrao()
        {
            this.Senha = Encryptor.Encrypt("calendario2013");
            PrecisaCriarUsuario = true;
        }

        public void TrocarSenha(string novaSenha)
        {
            #region pré-condição

            Assertion novaSenhaPrecisaSerDiferente = Assertion.That(!this.SenhaDescriptografada.Equals(novaSenha))
                                                                .WhenNot("A nova senha não pode ser igual à anterior.");

            #endregion
            novaSenhaPrecisaSerDiferente.Validate();

            this.Senha = Encryptor.Encrypt(novaSenha);
            this.PrecisaCriarUsuario = false;

            #region pós-condição

            string senhaAlterada = Encryptor.Decrypt(this.Senha);
            Assertion senhaFoiAlterada = Assertion.That(SenhaDescriptografada == novaSenha).WhenNot("ERRO: A senha não pode ser alterada.");
            Assertion marcouQueNaoPrecisaTrocarSenha = Assertion.That(!this.PrecisaCriarUsuario).WhenNot("ERRO: Houve um problema ao atualizar a senha.");

            #endregion
            (senhaFoiAlterada & marcouQueNaoPrecisaTrocarSenha).Validate();
        }

        public bool ValidarSenha(string senha)
        {
            return SenhaDescriptografada == senha;
        }

        public void Alterar(string nome, string email, Grupo grupoPai)
        {
            PreencherNome(nome);
            PreencherGrupoPai(grupoPai);
            this.Email = email;
        }

//        public string ObterMensagemBoasVindas()
//        {
//            return @"Informamos que seu acesso ao calendário está disponível.<br/><br/>
//                     Seus dados para acesso são:<br/>
//                    
//                    <b>Login: </b>" + this.Email + @"<br/>
//                    <b>Senha: </b>" + this.SenhaDescriptografada + @"<br/><br/>
//
//                    As informações de acesso são sigilosas.<br/>
//                    Portanto, ressaltamos a importância de não compartilhar pessoas que não sejam agentes de seu grupo/pastoral.<br/><br/>
//
//                    Atenciosamente,<br/>
//                    Pastoral da Comunicação";
//        }
    }
}
