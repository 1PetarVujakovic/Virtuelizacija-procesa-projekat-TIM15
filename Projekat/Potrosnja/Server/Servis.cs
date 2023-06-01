using Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Server
{
    
    public class Servis : IServis
    {
        public List<Load> VratiLoad(string upit)
        {
            List<Load> lista = new List<Load>();
            if((lista=readBazaLoad(upit))==null)
            {
                if((lista=readXmlLoad(upit))==null)
                {
                    return null;
                }
                else
                {
                    return lista;
                }
            }
            else
            {
                return lista;
            }
            
        }

        public Audit VratiAudit(string upit)
        {
            Audit a = new Audit(BazaPodataka.bazaAudit.Count() + 1, upit, MsgType.ERROR, "U bazi podataka ne postoje podaci za datum " + upit);
            BazaPodataka.AuditToInMemory(a);
            return a;
        }

        public void AuditToXml(Audit a)
        {
            string xml = System.IO.File.ReadAllText(@"../../../Server/bin/Debug/TBL_AUDIT.xml");
            string xmlEdited = xml.Replace("</STAVKE>", string.Empty);
            using (StreamWriter sw = new StreamWriter(@"../../../Server/bin/Debug/TBL_AUDIT.xml"))
            {
                sw.WriteLine(xmlEdited);
                sw.WriteLine("    <row>");
                sw.WriteLine("        <ID>" + a.Id + "</ID>");
                sw.WriteLine("        <TIME_STAMP>" + a.TimeStamp + "</TIME_STAMP>");
                sw.WriteLine("        <MESSAGE_TYPE>" + a.MessageType + "</MESSAGE_TYPE>");
                sw.WriteLine("        <MESSAGE>" + a.Message + "</MESSAGE>");
                sw.WriteLine("    </row>");
                sw.WriteLine("</STAVKE>", "");
                sw.Close();
                var lines = File.ReadAllLines(@"../../../Server/bin/Debug/TBL_AUDIT.xml").Where(arg => !string.IsNullOrWhiteSpace(arg));
                File.WriteAllLines(@"../../../Server/bin/Debug/TBL_AUDIT.xml", lines);
            }
        }

        public List<Load> readBazaLoad(string upit)
        {
            List<Load> lista = new List<Load>();

            foreach (Load ucitani in BazaPodataka.bazaLoad.Values)
            {
                if (ucitani.TimeStamp.Contains(upit))
                {
                    lista.Add(ucitani);
                }
            }

            if(lista.Count()==0)
            {
                return null;
            }
            return lista;
        }

        public List<Load> readXmlLoad(string upit)
        {
            using (DataSet ds = new DataSet())
            {
                ds.ReadXml("TBL_LOAD.xml");
                DataTable newDataTable = ds.Tables[0];
                List<Load> lista = new List<Load>();
                foreach (DataRow dr in newDataTable.Rows)
                {

                    if (dr.ItemArray[1].ToString().Split(' ')[0] == upit)
                    {
                        Load load = new Load(Convert.ToInt32(dr.ItemArray[0]), Convert.ToString(dr.ItemArray[1]), Convert.ToDouble(dr.ItemArray[2]), Convert.ToDouble(dr.ItemArray[3]));
                        lista.Add(load);
                        BazaPodataka.bazaLoad.TryAdd(load.Id, load);
                    }
                }
                if (lista.Count > 0)
                    return lista;
                else
                    return null;
            }
        }

        public static List<Audit> readXmlAudit()
        {
            using (DataSet ds = new DataSet())
            {
                ds.ReadXml("TBL_AUDIT.xml");
                DataTable newDataTable = ds.Tables[0];
                List<Audit> lista = new List<Audit>();
                foreach (DataRow dr in newDataTable.Rows)
                {
                    Audit audit;
                    if (dr.ItemArray[2] == "INFO")
                    {
                        audit = new Audit(Convert.ToInt32(dr.ItemArray[0]), Convert.ToString(dr.ItemArray[1]), MsgType.INFO, Convert.ToString(dr.ItemArray[3]));
                    }
                    else if (dr.ItemArray[2] == "WARNING")
                    {
                        audit = new Audit(Convert.ToInt32(dr.ItemArray[0]), Convert.ToString(dr.ItemArray[1]), MsgType.WARNING, Convert.ToString(dr.ItemArray[3]));
                    }
                    else
                    {
                        audit = new Audit(Convert.ToInt32(dr.ItemArray[0]), Convert.ToString(dr.ItemArray[1]), MsgType.ERROR, Convert.ToString(dr.ItemArray[3]));
                    }
                    lista.Add(audit);
                    BazaPodataka.bazaAudit.TryAdd(audit.Id, audit);

                }
                if (lista.Count > 0)
                    return lista;
                else
                    return null;
            }
        }
    }
}
