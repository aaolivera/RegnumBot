using Dominio;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Servicios.RegnumProviders
{
    public class MapaProvider : RegnumProvider
    {
        private readonly CoordenadasProvider coordenadasProvider;
        private readonly MoverPjProvider moverPjProvider;
        private readonly Mapa mapa;

        public MapaProvider(CoordenadasProvider coordenadasProvider, MoverPjProvider moverPjProvider) : base(null, null)
        {
            this.coordenadasProvider = coordenadasProvider;
            this.moverPjProvider = moverPjProvider;
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

        private void MoverANodo(Coordenada posActual, Nodo destino)
        {
            AlinearADestino(posActual, destino);
            var distancia = destino.Distancia(posActual.Posicion);
            moverPjProvider.Avanzar(distancia);
        }

        private void AlinearADestino(Coordenada posActual, Nodo destino)
        {
            var catetoOpuesto = Math.Abs(posActual.Posicion.Y - destino.Y);
            var catetoAdyasente = Math.Abs(posActual.Posicion.X - destino.X);

            var anguloEnRad = Convert.ToDecimal(Math.Atan(catetoOpuesto / catetoAdyasente) * Math.PI / 180);

            if(posActual.Direccion != anguloEnRad)
            {
                var diferencia = posActual.Direccion - anguloEnRad;
                if(diferencia <= 3)
                {
                    moverPjProvider.Girar(true, diferencia);
                }
                else
                {
                    moverPjProvider.Girar(false, diferencia);
                }
            }
        }

        public List<Nodo> DefinirCamino(Point desde, Point destino)
        {
            return mapa.DefinirCamino(desde, destino);
        }
    }
}
