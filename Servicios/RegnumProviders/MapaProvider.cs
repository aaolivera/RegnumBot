using Dominio;
using Dominio.Handlers;
using Servicios.InternalProviders;
using System.Drawing;
using System.Text.RegularExpressions;

namespace Servicios.RegnumProviders
{
    public class MapaProvider : RegnumProvider
    {
        bool[,] map = new bool[6000, 6000];
        public MapaProvider() : base(null, null)
        {
        }

        public void Obtener()
        {
            
        }
    }
}
