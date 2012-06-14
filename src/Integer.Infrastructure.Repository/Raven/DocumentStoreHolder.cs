using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Linq;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;
using Raven.Client.Embedded;
using Raven.Database.Server;

namespace Integer.Infrastructure.Repository
{
    public class DocumentStoreHolder
    {
    	private static IDocumentStore documentStore;

    	public static IDocumentStore DocumentStore
    	{
    		get { return (documentStore ?? (documentStore = CreateDocumentStore())); }
    	}

        public static void Initialize()
        {
            documentStore = CreateDocumentStore();
            CreateIndexes();
        }

    	private static IDocumentStore CreateDocumentStore()
        {
            NonAdminHttp.EnsureCanListenToWhenInNonAdminContext(8081);
            var store = new EmbeddableDocumentStore
            {
                DataDirectory = "App_Data\\Integer",
                DefaultDatabase = "Integer",
                UseEmbeddedHttpServer = true
            };
            store.Initialize();

            return store;
        }

		public static void CreateIndexes()
		{
			IndexCreation.CreateIndexes(typeof(DocumentStoreHolder).Assembly, DocumentStore);
		}
    }
}