using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Linq;
//using Raven.Client;
//using Raven.Client.Document;
//using Raven.Client.Indexes;
//using Raven.Client.Embedded;
//using Raven.Database.Server;

namespace Integer.Infrastructure.Repository
{
    public class DocumentStoreHolder
    {
    	private static dynamic documentStore;

    	public static dynamic DocumentStore
    	{
    		get { return (documentStore); }
    	}

        public static void Initialize()
        {
            documentStore = CreateDocumentStore();
            CreateIndexes();
        }

    	private static dynamic CreateDocumentStore()
        {
            //NonAdminHttp.EnsureCanListenToWhenInNonAdminContext(8081);
            //var store = new EmbeddableDocumentStore
            //{
            //    DataDirectory = "App_Data\\Integer",
            //    DefaultDatabase = "Integer",
            //    UseEmbeddedHttpServer = true
            //};
            //store.Initialize();

            return null;//store;
        }

		public static void CreateIndexes()
		{
			//IndexCreation.CreateIndexes(typeof(DocumentStoreHolder).Assembly, DocumentStore);
		}
    }
}
