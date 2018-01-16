using Dominio;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Servicios.RegnumProviders
{
    public class MapaProvider : RegnumProvider
    {
        private readonly CoordenadasProvider coordenadasProvider;
        private readonly Mapa mapa;

        public MapaProvider(CoordenadasProvider coordenadasProvider) : base(null, null)
        {
            this.coordenadasProvider = coordenadasProvider;
            this.mapa = new Mapa();

            var _filePath = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory);
            List<Nodo> nodos;
            using (var streamReader = File.OpenText(_filePath + "\\Mapa\\mapa.txt"))
            {
                var lines = streamReader.ReadToEnd();
                nodos = JsonConvert.DeserializeObject<List<Nodo>>(lines);
            }

            foreach(var nodo in nodos)
            {
                if (Nodo.RadioGeneral == 0) Nodo.RadioGeneral = nodo.Radio;
                mapa.AgregarNodo(nodo);
            }
        }

        public void Mover(Point destino)
        {
            var coordenadaActual = coordenadasProvider.Obtener();
        }

        public List<Nodo> DefinirCamino(Point desde, Point destino)
        {
            return mapa.DefinirCamino(desde, destino);
        }
    }
}
