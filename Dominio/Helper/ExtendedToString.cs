using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Helper
{
    public static class Extend
    {
        public static string ExtendedToString(this List<Nodo> list)
        {
            return string.Join(",", list);
        }
    }
}
