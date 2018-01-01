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
    public class CoordenadasProvider : RegnumProvider
    {
        private Rectangle _posicionCoordenadas;
        private bool _encontradas;
        public CoordenadasProvider(FrameProvider frameProvider, MouseProvider mouseProvider) : base(frameProvider, mouseProvider)
        {
            this._posicionCoordenadas = new Rectangle(0, 0, 220, 200);
            this._encontradas = false;
        }

        public Coordenada Obtener()
        {
            while (true)
            {
                var coord = Leer();
                if (coord != null){
                    _encontradas = true;
                    return coord;
                }
                if (_posicionCoordenadas.X < 1500 && !_encontradas)
                {
                    _posicionCoordenadas.X += 20;
                }
                else if (_posicionCoordenadas.Y < 1000 && !_encontradas)
                {
                    _posicionCoordenadas.X = 0;
                    _posicionCoordenadas.Y += 30;
                }
                else
                {
                    return null;
                }

            }
        }

        private Coordenada Leer()
        {
            var bit = _frameProvider.GetPartial(_posicionCoordenadas.X, _posicionCoordenadas.Y, _posicionCoordenadas.Width, _posicionCoordenadas.Height);
            var texto = LeerImagen(_frameProvider.ScaleByPercent(bit, 500));

            EjecutarEvento(bit, EventType.CoordenadasBitmap);
            EjecutarEvento(texto, EventType.CoordenadasTexto);

            // First we see the input string.
            Match match = Regex.Match(texto, @"pos: ([0-9]{4}\.[0-9]{2}),([0-9]{4}\.[0-9]{2})", RegexOptions.IgnoreCase);
            var x = match.Groups[1];
            var y = match.Groups[2];
            return match.Length > 1 ? new Coordenada { X = Convert.ToDecimal(x.Value.Replace('.', ',')), Y = Convert.ToDecimal(y.Value.Replace('.', ',')) } : null;
        }

        private string LeerImagen(Bitmap bit)
        {
            string text = "";

            try
            {
                using (var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default))
                {
                    using (var page = engine.Process(bit))
                    {
                        text = page.GetText();
                    }
                }
            }
            catch (Exception e)
            {
                Trace.TraceError(e.ToString());
                Console.WriteLine("Unexpected Error: " + e.Message);
                Console.WriteLine("Details: ");
                Console.WriteLine(e.ToString());
            }
            return text;
        }
    }
}
