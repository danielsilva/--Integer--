using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Raven.Client.Embedded;
using Raven.Client;
using Raven.Database.Server;
using Integer.Infrastructure.Repository;
using NovaVersaoAgenda = Integer.Domain.Agenda;
using NovaVersaoParoquia = Integer.Domain.Paroquia;

namespace Integer.Import
{
    public class Program
    {
        static void Main(string[] args)
        {
            DocumentStoreHolder.Initialize();
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

                #region Locais
                //Console.WriteLine("Importando Locais...");
                //IEnumerable<Local> locais = agenda.Local.ToList();
                //Console.WriteLine("Carregar Locais demorou: {0:#,#} ms", sp.ElapsedMilliseconds);

                //var locaisRaven = new List<NovaVersaoParoquia.Local>();
                //using (IDocumentSession s = store.OpenSession())
                //{
                //    foreach (Local l in locais)
                //    {
                //        var local = new NovaVersaoParoquia.Local(l.Nome);
                //        s.Store(local);
                //        locaisRaven.Add(local);
                //    }
                //    s.SaveChanges();
                //}
                //Console.WriteLine(sp.Elapsed);
                #endregion

                #region Grupos
                Console.WriteLine("Importando Grupos...");
                IEnumerable<Grupo> grupos = agenda.Grupo.ToList();
                Console.WriteLine("Carregar Grupos demorou: {0:#,#} ms", sp.ElapsedMilliseconds);

                var gruposRaven = new List<NovaVersaoParoquia.Grupo>();
                using (IDocumentSession s = store.OpenSession())
                {
                    #region grupos                     
                    foreach (Grupo g in grupos)
                    {
                        var grupo = new NovaVersaoParoquia.Grupo(g.Nome, DeParaGrupoEmail(g.Id), null);
                        s.Store(grupo);
                        gruposRaven.Add(grupo);
                    }
                    #endregion

                    s.SaveChanges();
                }
                Console.WriteLine(sp.Elapsed);
                #endregion

                //Console.WriteLine("Importando Eventos...");
                //IEnumerable<Evento> eventos = agenda.Evento
                //    .Include("Grupo")
                //    .Include("ListaInternaConflitos")
                //    .Include("ListaInternaReservasDeLocais")
                //    .OrderBy(e => e.DataInicio).ToList();

                //Console.WriteLine("Para carregar eventos, demorou: {0:#,#} ms", sp.ElapsedMilliseconds);

                //using (IDocumentSession s = store.OpenSession())
                //{
                //    foreach (Evento ev in eventos)
                //    {
                //        //var evento = new NovaVersaoAgenda.Evento(ev.Nome, ev.DataInicio, ev.DataFim, ev.Gru
                //    }
                //    s.SaveChanges();
                //}
            }
            Console.WriteLine(sp.Elapsed);
        }

        private static string DeParaGrupoEmail(short gID)
        {
            var deParaGrupoEmail = new Dictionary<short, string> { 
                {1,"conselho@paroquiadivinosalvador.com.br"},
                {2,"sem email"},
                {3,"apostoladodaoracao@paroquiadivinosalvador.com.br"},
                {4,"caminhadarosamistica@paroquiadivinosalvador.com.br"},
                {6,"deuseamor@paroquiadivinosalvador.com.br"},
                {7,"sem email"},
                {8,"sem email"},
                {9,"ligacatolica@paroquiadivinosalvador.com.br"},
                {11,"sem email"},
                {13,"oficinadeoracao@paroquiadivinosalvador.com.br"},
                {14,"mmc@paroquiadivinosalvador.com.br"},
                {15,"circulobiblico@paroquiadivinosalvador.com.br"},
                {16,"sem email"},
                {17,"pascom@paroquiadivinosalvador.com.br"},
                {18,"bazar@paroquiadivinosalvador.com.br"},
                {20,"cestadafraternidade@paroquiadivinosalvador.com.br"},
                {21,"pastoraldasaude@paroquiadivinosalvador.com.br"},
                {23,"dizimo@paroquiadivinosalvador.com.br"},
                {24,"grupoamizade@paroquiadivinosalvador.com.br"},
                {25,"sopao@paroquiadivinosalvador.com.br"},
                {26,"pastoraldoturismo@paroquiadivinosalvador.com.br"},
                {28,"sem email"},
                {29,"acristo@paroquiadivinosalvador.com.br"},
                {30,"ecc@paroquiadivinosalvador.com.br"},
                {31,"pastoralfamiliar@paroquiadivinosalvador.com.br"},
                {32,"sem email"},
                {33,"batismo@paroquiadivinosalvador.com.br"},
                {34,"catequeseadultos@paroquiadivinosalvador.com.br"},
                {35,"catequese@paroquiadivinosalvador.com.br"},
                {36,"perseveranca@paroquiadivinosalvador.com.br"},
                {38,"crisma@paroquiadivinosalvador.com.br"},
                {39,"sem email"},
                {40,"liturgia@paroquiadivinosalvador.com.br"},
                {41,"acolhimento@paroquiadivinosalvador.com.br"},
                {42,"mesc@paroquiadivinosalvador.com.br"},
                {43,"filhosdaredencao@paroquiadivinosalvador.com.br"},
                {45,"bandalinguasdefogo@paroquiadivinosalvador.com.br"},
                {46,"mavs@paroquiadivinosalvador.com.br"},
                {47,"sem email"},
                {48,"kyrios@paroquiadivinosalvador.com.br"},
                {49,"sem email"},
                {50,"bomsamaritano@paroquiadivinosalvador.com.br"},
                {51,"kairos@paroquiadivinosalvador.com.br"},
                {52,"eac@paroquiadivinosalvador.com.br"},
                {53,"juventudesanta@paroquiadivinosalvador.com.br"},
                {54,"getsemani@paroquiadivinosalvador.com.br"},
                {55,"perfumedemirra@paroquiadivinosalvador.com.br"},
                {56,"gad@paroquiadivinosalvador.com.br"},
                {57,"fontedevida@paroquiadivinosalvador.com.br"},
                {58,"coroinhas@paroquiadivinosalvador.com.br"},
                {59,"divinosalvador@paroquiadivinosalvador.com.br"},
            };
            return deParaGrupoEmail[gID];
        }
    }
}
