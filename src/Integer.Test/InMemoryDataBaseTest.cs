using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Raven.Client;
using Raven.Client.Embedded;
using Raven.Client.Document;

namespace Integer.UnitTests
{
    public abstract class InMemoryDataBaseTest
    {
        IDocumentStore dataBase;
        protected IDocumentSession DataBaseSession { get; private set; }

        [TestFixtureSetUp]
        public void init() 
        {
            CriarNovoBancoDeDados();
        }

        [SetUp]
        public void TestSetUp() 
        {
            DataBaseSession = dataBase.OpenSession();
        }

        [TearDown]
        public void TestTearDown() 
        {
            DataBaseSession.Dispose();
            DataBaseSession = null;
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown() 
        {
            LimparBancoDeDados();
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

        protected void LimparBancoDeDados() 
        {
            dataBase.Dispose();
        }
    }
}
