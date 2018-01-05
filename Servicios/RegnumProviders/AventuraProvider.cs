using Dominio;
using Dominio.Handlers;
using Servicios.InternalProviders;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Text.RegularExpressions;
using Tesseract;

namespace Servicios.RegnumProviders
{
    public class AventuraProvider : RegnumProvider
    {
        private Rectangle _posicionCoordenadas;
        private bool _encontradas;
        public AventuraProvider(FrameProvider frameProvider, MouseProvider mouseProvider) : base(frameProvider, mouseProvider)
        {
            this._posicionCoordenadas = new Rectangle(0, 0, 320, 90);
            this._encontradas = false;
        }

        public Aventura Obtener()
        {
            var bit = _frameProvider.GetPartial(_frameProvider.Width / 2 - (_posicionCoordenadas.Width / 2), _frameProvider.Height / 2 + _posicionCoordenadas.Height / 2, _posicionCoordenadas.Width, _posicionCoordenadas.Height);
            var texto = LeerImagen(_frameProvider.ScaleByPercent(bit, 10)).Replace('\'', 'i');

            EjecutarEvento(bit, EventType.AventuraBitmap);
            EjecutarEvento(texto, EventType.AventuraTexto);

            // First we see the input string.
            Match match = Regex.Match(texto, @"Dar Carta a ([^\n]{0,30})\nHablar con([^\n]{0,30})", RegexOptions.IgnoreCase);
            var desde = match.Groups[1];
            var hasta = match.Groups[2];
            return hasta.Length > 0 && desde.Length > 0 ? new Aventura { Desde = desde.Value, Hasta = hasta.Value } : null;
        }
    }
}
