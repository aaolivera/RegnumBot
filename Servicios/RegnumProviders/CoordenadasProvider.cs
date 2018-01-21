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

        public CoordenadasProvider(FrameProvider frameProvider, MouseProvider mouseProvider, ILogger log) : base(frameProvider, mouseProvider, log)
        {
            this._posicionCoordenadas = new Rectangle(frameProvider.Width - 140, 93, 140, 35);
        }
        
        public Coordenada Obtener()
        {
            return Leer();
        }
        
        private Coordenada Leer()
        {
            var bit = _frameProvider.GetPartial(_posicionCoordenadas.X, _posicionCoordenadas.Y, _posicionCoordenadas.Width, _posicionCoordenadas.Height);
            Color pixel;
            for (int y = 0; y < bit.Height; y++)
            {
                for (int x = 0; x < bit.Width; x++)
                {
                    pixel = bit.GetPixel(x, y);
                    double gris = pixel.R * 0.3 + pixel.G * 0.59 + pixel.B * 0.11;

                    if (gris > 127)
                        bit.SetPixel(x, y, Color.White);
                    else
                        bit.SetPixel(x, y, Color.Black);
                }
            }

            var temp = _frameProvider.ScaleByPercent(bit, 10);
            var texto = LeerImagen(temp);

            EjecutarEvento(bit, EventType.CoordenadasBitmap);
            EjecutarEvento(texto, EventType.CoordenadasTexto);

            // First we see the input string.
            Match match = Regex.Match(texto, @"pos: ([0-9]{1,4}\.[0-9]*)[.,]([0-9]{1,4}\.[0-9]*)", RegexOptions.IgnoreCase);
            Match rot = Regex.Match(texto, @"Rot: -?([0-9]{1}\.[0-9]{2})", RegexOptions.IgnoreCase);

            if (match.Groups.Count < 3 || match.Groups[1].Length == 0 || match.Groups[2].Length == 0) return null;
            if (rot.Groups.Count < 2 || rot.Groups[1].Length == 0 || match.Groups[1].Length == 0) return null;

            var pos = new Point(Convert.ToInt32(match.Groups[1].Value.Split('.')[0]), Convert.ToInt32(match.Groups[2].Value.Split('.')[0]));
            return new Coordenada { Posicion = pos, Direccion = Convert.ToDecimal(rot.Groups[1].Value.Replace('.',','))};
        }
    }
}
