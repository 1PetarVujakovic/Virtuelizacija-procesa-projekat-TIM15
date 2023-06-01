using Common;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class BazaPodataka
    {
        public static ConcurrentDictionary<int,Load> bazaLoad = new ConcurrentDictionary<int,Load>();
        public static ConcurrentDictionary<int,Audit> bazaAudit = new ConcurrentDictionary<int,Audit>();

        public static void AuditToInMemory(Audit a)
        {
            bazaAudit.TryAdd(a.Id, a);
            return;
        }
    }
}
