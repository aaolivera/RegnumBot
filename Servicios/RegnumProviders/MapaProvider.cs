using Dominio;
using Dominio.Handlers;
using Servicios.InternalProviders;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Servicios.RegnumProviders
{
    public class MapaProvider : RegnumProvider
    {
        private readonly CoordenadasProvider coordenadasProvider;
        private readonly bool[][] map;
        public MapaProvider(CoordenadasProvider coordenadasProvider) : base(null, null)
        {
            var _filePath = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory);
            var lineas = new List<bool[]>();
            using (var streamReader = File.OpenText(_filePath + "\\mapa.txt"))
            {
                var lines = streamReader.ReadToEnd().Split('\n');
                foreach(var line in lines)
                {
                    lineas.Add(line.Split(',').Select(x => x == "1").ToArray());
                }
            }
            map = lineas.ToArray();
            this.coordenadasProvider = coordenadasProvider;
        }

        public void Mover(Point destino)
        {
            var coordenadaActual = coordenadasProvider.Obtener();
        }

        public void DefinirCamino(Point desde, Point destino)
        {

        }
    }
}
