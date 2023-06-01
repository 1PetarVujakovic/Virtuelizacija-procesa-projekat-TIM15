using Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.CompilerServices;

namespace Potrosnja
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ChannelFactory<IServis> kanal = new ChannelFactory<IServis>("Servis");

            IServis proxy = kanal.CreateChannel();

            while(true)
            {
                string odgovor;
                Console.Write("\nOdaberite opciju:\n 1.Unosenje upita\n 2.Izlazak iz programa\n");
                odgovor = Console.ReadLine();

                if(odgovor.Equals("2"))
                {
                    Console.WriteLine("Dovidjenja!");
                    Console.ReadKey();
                    Environment.Exit(0);
                }
                else if(odgovor.Equals("1"))
                {
                    string godina;
                    Console.Write("Unesite godinu: ");
                    godina = Console.ReadLine();

                    string mesec;
                    do
                    {
                        Console.Write("Unesite mesec: ");
                        mesec = Console.ReadLine();
                    } while (validacijaMesec(mesec));

                    mesec = validacijaDuzina(mesec);

                    string dan;

                    do
                    {
                        Console.Write("Unesite dan: ");
                        dan = Console.ReadLine();
                    } while (validacijaDan(dan, mesec, godina));

                    dan = validacijaDuzina(dan);

                    string upit = godina + "-" + mesec + "-" + dan;
                    Console.Write("Datum: ");
                    Console.WriteLine(upit);

                    proveriLoad(proxy, upit);
                }
                else
                {
                    Console.Write("Nepravilan unos.\nMozete da unesete samo 1 ili 2.\n");
                }
            }
        }

        public static string validacijaDuzina(string d)
        {
            if (d.Length == 1)
            {
                d = "0" + d;
            }
            else if (d.Length > 2)
            {
                d = d.Substring(d.Length - 2);
            }

            return d;
        }

        public static bool validacijaDan(string dan, string mesec, string godina)
        {
            bool validacija = false;

            int Dan = Int32.Parse(dan);
            int Mesec = Int32.Parse(mesec);
            int Godina = Int32.Parse(godina);

            if (Mesec == 1 || Mesec == 3 || Mesec == 5 || Mesec == 7 || Mesec == 8 || Mesec == 10 || Mesec == 12)
            {
                if(Dan < 1 || Dan > 31)
                {
                    Console.WriteLine("Meseci Januar, Mart, Maj, Jul, Avgust, Oktobar I Decembar ne mogu imati manje od 0 i više od 31 dan.");
                    validacija = true;
                }  
            }

            if (Mesec == 4 || Mesec == 6 || Mesec == 9 || Mesec == 11)
            {
                if (Dan < 1 || Dan > 30)
                {
                    Console.WriteLine("Meseci April, Jun, Septembar i Novembar ne mogu imati manje od 0 i više od 30 dana.");
                    validacija = true;
                }  
            }

            if (Mesec == 2)
            {
                if(Godina % 4 == 0)
                {
                    if(Dan < 1 || Dan > 29)
                    {
                        Console.WriteLine("U prestupnoj godini Februar ne moze imati manje od 0 i više od 29 dana.");
                        validacija = true;
                    }
                }
                else
                {
                    if (Dan < 1 || Dan > 29)
                    {
                        Console.WriteLine("Februar ne moze imati manje od 0 i više od 28 dana.");
                        validacija = true;
                    }
                }
            }

            return validacija;
        }

        public static bool validacijaMesec(string mesec)
        {
            bool validacija = false;

            if (Int32.Parse(mesec) < 1 || Int32.Parse(mesec) > 12)
            {
                Console.WriteLine("Mesec ne moze biti manji od 1 ili veci od 12");
                validacija = true;
            }

            return validacija;
        }

        public static void upisiCsv(List<Load> lista, string upit)
        {
            upit = upit.Replace("-", "_");
            string putanja = @"" + ConfigurationManager.AppSettings["putanjaDoFajla"] + upit + ".csv";
            String separator = ",";
            StringBuilder output = new StringBuilder();
            String[] headings = { "ID", "Time Stamp", "Forecast Value", "Measured Value" };
            output.AppendLine(string.Join(separator, headings));

            foreach(Load l in lista)
            {
                String[] newLine = { l.Id.ToString(), l.TimeStamp, l.ForecastValue.ToString(), l.MeasuredValue.ToString() };
                output.AppendLine(string.Join(separator, newLine));
            }

            try
            {
                File.AppendAllText(putanja, output.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            Console.WriteLine("Podaci su uspesno upisani u CSV datoteku.");
        }

        public static void proveriLoad(IServis proxy, string upit)
        {
            List<Load> lista = proxy.VratiLoad(upit);

            if(lista == null)
            {
                ispisiAudit(proxy, upit);
            }
            else
            {
                foreach (Load l in lista)
                {
                    if (l.TimeStamp.Contains(upit))
                    {
                        Console.WriteLine(l);
                    }
                }
                upisiCsv(lista, upit);
            }
        }

        public static void ispisiAudit(IServis proxy, string upit)
        {
            Audit a = proxy.VratiAudit(upit);
            proxy.AuditToXml(a);
            Console.WriteLine(a.Message);
        }


    }
}
