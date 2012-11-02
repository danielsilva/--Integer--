using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Linq;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;
using Raven.Client.Embedded;
using Raven.Database.Server;
using Integer.Infrastructure.Repository.Indexes;

namespace Integer.Infrastructure.Repository
{
    public class DocumentStoreHolder
    {
        private static IDocumentStore documentStore;

        public static IDocumentStore DocumentStore
    	{
    		get { return (documentStore); }
    	}

        public static void Initialize()
        {
            documentStore = CreateDocumentStore();
            CreateIndexes();
        }

    	private static IDocumentStore CreateDocumentStore()
        {
            var store = new DocumentStore
            {
                ConnectionStringName = "Integer"
            };
            store.Conventions.SaveEnumsAsIntegers = true;
            store.Initialize();

            IndexCreation.CreateIndexes(typeof(ReservasMap).Assembly, store);

            return store;
        }

		public static void CreateIndexes()
		{
            IndexCreation.CreateIndexes(typeof(DocumentStoreHolder).Assembly, DocumentStore);
		}
    }
}
