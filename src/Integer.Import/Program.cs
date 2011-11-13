using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Raven.Client.Embedded;
using Raven.Client;
using Raven.Database.Server;
using Integer.Infrastructure.Repository;

namespace Integer.Import
{
    public class Program
    {
        static void Main(string[] args)
        {
            using (IDocumentStore store = DocumentStoreHolder.DocumentStore)
            {
                ImportarPara(store);
            }
            Console.WriteLine("Fim da importação.");
        }

        private static void ImportarPara(IDocumentStore store)
        {
            Stopwatch sp = Stopwatch.StartNew();

            using (var agenda = new AgendaContainer())
            {
                Console.WriteLine("Começando...");

                IEnumerable<Evento> eventos = agenda.Evento
                    .Include("Grupo")
                    .Include("ListaInternaConflitos")
                    .Include("ListaInternaReservasDeLocais")
                    .OrderBy(e => e.DataInicio).ToList();

                Console.WriteLine("Para carregar eventos, demorou: {0:#,#} ms", sp.ElapsedMilliseconds);

                using (IDocumentSession s = store.OpenSession())
                {
                    foreach (Evento evento in eventos)
                    {

                    }
                }
            }
            Console.WriteLine(sp.Elapsed);
        }
    }
}
