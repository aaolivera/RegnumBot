using Dominio;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using Ninject.Extensions.Logging;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Dominio.Helper;

namespace Servicios.RegnumProviders
{
    public class MapaProvider : RegnumProvider
    {
        private readonly CoordenadasProvider coordenadasProvider;
        private readonly MoverPjProvider moverPjProvider;
        private readonly Mapa mapa;

        public MapaProvider(CoordenadasProvider coordenadasProvider, MoverPjProvider moverPjProvider, ILogger log) : base(null, null, log)
        {
            this.coordenadasProvider = coordenadasProvider;
            this.moverPjProvider = moverPjProvider;
            this.mapa = new Mapa();

            var _filePath = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
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
            var coordenadaActual = ObtenerCoordenada();

            var nodoExistente = mapa.ObtenerNodo(coordenadaActual.Posicion);
            if(nodoExistente == null)
            {
                mapa.AgregarNodoAsociadoAlCercano(new Nodo(coordenadaActual.Posicion.X, coordenadaActual.Posicion.Y));
            }

            var camino = DefinirCamino(coordenadaActual.Posicion, destino);

            _log.Info($"Camino: {camino.ExtendedToString()}");
            foreach (var nodo in camino)
            {
                if (!nodo.HayInterseccion(coordenadaActual.Posicion))
                {
                    MoverANodo(coordenadaActual, nodo);
                }
            }
        }

        private Coordenada ObtenerCoordenada()
        {
            _log.Info("Obteniendo coordenada");
            Coordenada coordenada = null;
            while (coordenada == null)
            {
                coordenada = coordenadasProvider.Obtener();
                _log.Info("Coordenadas no encontradas");
            }
            _log.Info($"Coordenada: {coordenada}");
            return coordenada;
        }

        private void MoverANodo(Coordenada posActual, Nodo destino)
        {
            AlinearADestino(posActual, destino);
            var distancia = destino.Distancia(posActual.Posicion);
            moverPjProvider.Avanzar(distancia);

            posActual.Posicion = new Point(destino.X, destino.Y);
        }

        private void AlinearADestino(Coordenada posActual, Nodo destino)
        {
            var catetoOpuesto = Math.Abs(posActual.Posicion.Y - destino.Y);
            var catetoAdyasente = Math.Abs(posActual.Posicion.X - destino.X);

            var anguloEnRad = Convert.ToDecimal(Math.Atan(catetoOpuesto / catetoAdyasente) * Math.PI / 180);

            if(posActual.Direccion != anguloEnRad)
            {
                var diferencia = posActual.Direccion - anguloEnRad;
                moverPjProvider.Girar(diferencia);
            }

            posActual.Direccion = anguloEnRad;
        }

        public List<Nodo> DefinirCamino(Point desde, Point destino)
        {
            return mapa.DefinirCamino(desde, destino);
        }
    }
}