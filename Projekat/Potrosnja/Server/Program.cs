using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common;

namespace Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Thread nitServis = new Thread(new ThreadStart(metodaServis));
            nitServis.Start();
            Thread nitProveraVremena = new Thread(new ThreadStart(metodaProveraVremena));
            nitProveraVremena.Start();

            Servis.readXmlAudit();

            nitServis.Join();
            nitProveraVremena.Join();
        }

        static void metodaServis()
        {
            using (ServiceHost host = new ServiceHost(typeof(Servis)))
            {
                host.Open();

                Console.WriteLine("Servis je uspesno pokrenut.");
                Console.ReadKey();
            }
        }

        private static EventArgs e;
        public static event EventHandler eventZaBrisanje;
        public delegate bool metodaVreme(Load l);
        static void metodaProveraVremena()
        {
            int vreme = Int32.Parse(ConfigurationManager.AppSettings["dataTimeout"]);

            while (true)
            {
                foreach (Load l in BazaPodataka.bazaLoad.Values)
                {
                    TimeSpan spam = DateTime.Now - l.CreationTime;
                    int vreme2 = (int)spam.TotalMilliseconds;
                    if (vreme2 > vreme)
                    {
                        metodaVreme pom = brisanjeIzBaze;
                        eventZaBrisanje(pom(l), e);
                    }
                }
                Thread.Sleep(2000);
            }
        }

        public static bool brisanjeIzBaze(Load l)
        {
            BazaPodataka.bazaLoad.TryRemove(l.Id, out _);
            return true;
        }

        
    }
}
