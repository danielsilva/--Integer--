using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Database;
using Raven.Client.Indexes;
using System.Reflection;
using Raven.Client;
using Raven.Client.Embedded;
using Raven.Database.Config;

namespace Integer.Infrastructure.Repository.DataAccess
{
    public static class Bootstrapper
    {
        public static void Startup()
        {
            IDocumentStore documentStore = new EmbeddableDocumentStore
            { 
                DataDirectory = "App_Data\\RavenDB"
            };
            documentStore.Initialize();


            IndexCreation.CreateIndexes(Assembly.GetExecutingAssembly(), documentStore);
        }
    }
}
