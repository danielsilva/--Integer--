using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Client;
using Raven.Client.Embedded;
using Raven.Client.Document;

namespace Integer.UnitTests
{
    public abstract class InMemoryDataBaseTest : IDisposable
    {
        IDocumentStore dataBase;
        protected IDocumentSession DataBaseSession { get; private set; }

        public InMemoryDataBaseTest() 
        {
            CriarNovoBancoDeDados();
            DataBaseSession = dataBase.OpenSession();
        }

        public void Dispose() 
        {
            DataBaseSession.Dispose();
            DataBaseSession = null;

            dataBase.Dispose();
        }

        protected void CriarNovoBancoDeDados() 
        {
            dataBase = new EmbeddableDocumentStore()
            {
                RunInMemory = true,

            };
            dataBase.Conventions.DefaultQueryingConsistency = ConsistencyOptions.QueryYourWrites;
            dataBase.Initialize();
        }
    }
}
