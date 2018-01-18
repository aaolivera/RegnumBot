using Dominio;
using Dominio.Handlers;
using Ninject.Extensions.Logging;
using Servicios.InternalProviders;
using System;
using System.Drawing;
using System.Text.RegularExpressions;
using Tesseract;

namespace Servicios.RegnumProviders
{
    public class CoordenadasProvider : RegnumProvider
    {
        private Rectangle _posicionCoordenadas;
        private bool _encontradas;
        private int desplazamientoX = 20;
        private int desplazamientoY = 30;

        public CoordenadasProvider(FrameProvider frameProvider, MouseProvider mouseProvider, ILogger log) : base(frameProvider, mouseProvider, log)
        {
            this._posicionCoordenadas = new Rectangle(0, 32, 220, 140);
            this._encontradas = false;
        }
        
        public Coordenada Obtener()
        {
            while (true)
            {
                var coord = Leer();
                if (coord != null){
                    if(_encontradas == false)
                    {
                        Enfocar();
                        Enfocar();
                    }
                    _encontradas = true;
                    return coord;
                }
                if (_posicionCoordenadas.X < (_frameProvider.Width - _posicionCoordenadas.Width) && !_encontradas)
                {
                    _posicionCoordenadas.X += desplazamientoX;
                }
                else if (_posicionCoordenadas.Y < (_frameProvider.Height - _posicionCoordenadas.Height) && !_encontradas)
                {
                    _posicionCoordenadas.X = 0;
                    _posicionCoordenadas.Y += desplazamientoY;
                }
                else
                {
                    return null;
                }

            }
        }

        private void Enfocar()
        {
            desplazamientoX = desplazamientoX / 2;
            desplazamientoY = desplazamientoY / 2;
            while (Leer() != null)
            {
                _posicionCoordenadas.X += desplazamientoX;
            }
            _posicionCoordenadas.X -= desplazamientoX * 2;
            
            while (Leer() != null)
            {
                _posicionCoordenadas.Y += desplazamientoY ;
            }
            _posicionCoordenadas.Y-= desplazamientoY * 2;

            while (Leer() != null)
            {
                _posicionCoordenadas.Width -= desplazamientoX;
            }
            _posicionCoordenadas.Width += desplazamientoX * 2;

            while (Leer() != null)
            {
                _posicionCoordenadas.Height -= desplazamientoY;
            }
            _posicionCoordenadas.Height += desplazamientoY * 2;
        }

        private Coordenada Leer()
        {
            _log.Debug("1");
            var bit = _frameProvider.GetPartial(_posicionCoordenadas.X, _posicionCoordenadas.Y, _posicionCoordenadas.Width, _posicionCoordenadas.Height);

            var temp = _frameProvider.ScaleByPercent(bit, 6);
            var texto = LeerImagen(temp);
            _log.Debug("2");
            EjecutarEvento(bit, EventType.CoordenadasBitmap);
            EjecutarEvento(texto, EventType.CoordenadasTexto);

            // First we see the input string.
            Match match = Regex.Match(texto, @"pos: ([0-9]{4}\.[0-9]*),([0-9]{4}\.[0-9]*)", RegexOptions.IgnoreCase);
            Match rot = Regex.Match(texto, @"Rot: (-?[0-9]{1}\.[0-9]{2})", RegexOptions.IgnoreCase);

            if (match.Groups.Count < 3 || match.Groups[1].Length == 0 || match.Groups[2].Length == 0) return null;
            if (rot.Groups.Count < 2 || rot.Groups[1].Length == 0 || match.Groups[1].Length == 0) return null;

            var pos = new Point(Convert.ToInt32(match.Groups[1].Value.Substring(0,4)), Convert.ToInt32(match.Groups[2].Value.Substring(0, 4)));
            return new Coordenada { Posicion = pos, Direccion = Convert.ToDecimal(rot.Groups[1].Value)};
        }
    }
}
