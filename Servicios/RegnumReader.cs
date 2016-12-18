using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Dominio;
using Dominio.Handlers;
using Tesseract;

namespace Servicios
{
    public class RegnumReader
    {
        private readonly FrameProvider _frameProvider;
        private static RegnumReader _regnumReader;
        private readonly Dictionary<EventType, List<IFrameEventHandler>> _frameEventHandlers;

        private Rectangle _posicionCoordenadas ;
        private Rectangle _posicionStats;

        private RegnumReader()
        {
            this._frameProvider = FrameProvider.GetFrameProvider("ROClientGame");
            this._posicionCoordenadas = new Rectangle(0,0,220,200);
            this._posicionStats = new Rectangle(0, 0, 220, 100);
            this._frameEventHandlers = new Dictionary<EventType, List<IFrameEventHandler>>();
        }

        public static RegnumReader GetRegnumReader()
        {
            return _regnumReader ?? (_regnumReader = new RegnumReader());
        }

        //COORDENADAS
        public Coordenada BuscarCoordenadas()
        {
            while (true)
            {
                var coord = ObtenerCoordenadas();
                if (coord != null) return coord;
                if (_posicionCoordenadas.X < 1500)
                {
                    _posicionCoordenadas.X+= 20;
                }
                else if (_posicionCoordenadas.Y < 1000)
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

        public Coordenada ObtenerCoordenadas()
        {
            var bit = _frameProvider.GetPartial(_posicionCoordenadas.X, _posicionCoordenadas.Y, _posicionCoordenadas.Width, _posicionCoordenadas.Height);
            var texto = LeerImagen(_frameProvider.ScaleByPercent(bit, 500));

            EjecutarEvento(bit, EventType.CoordenadasBitmap);
            EjecutarEvento(texto, EventType.CoordenadasTexto);

            // First we see the input string.
            Match match = Regex.Match(texto, @"pos: ([0-9]{4}\.[0-9]{2}),([0-9]{4}\.[0-9]{2})", RegexOptions.IgnoreCase);
            var x = match.Groups[1];
            var y = match.Groups[2];
            return match.Length > 1 ? new Coordenada {X = Convert.ToDecimal(x.Value.Replace('.',',')), Y = Convert.ToDecimal(y.Value.Replace('.', ',')) } : null;
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

        //STATS
        private static int pixelRojoMax = 1;
        private static int pixelAzulMax = 1;
        public Stats BuscarStats()
        {
            var bit = _frameProvider.GetPartial(_posicionStats.X, _posicionStats.Y, _posicionStats.Width, _posicionStats.Height);

            LeerStats(bit, out pixelAzulMax, out pixelRojoMax);

            EjecutarEvento(bit, EventType.StatsBitmap);
            EjecutarEvento($"{pixelRojoMax}-{pixelAzulMax}", EventType.StatsTexto);

            return pixelAzulMax == 0 && pixelRojoMax == 0 ? null : new Stats { Mana = 100, Vida = 100 };
        }

        public Stats ObtenerStats()
        {
            var bit = _frameProvider.GetPartial(_posicionStats.X, _posicionStats.Y, _posicionStats.Width, _posicionStats.Height);

            int pixelAzul;
            int pixelRojo;
            LeerStats(bit, out pixelAzul, out pixelRojo);

            EjecutarEvento(bit, EventType.StatsBitmap);
            EjecutarEvento($"{pixelRojo}-{pixelAzul}", EventType.StatsTexto);
            
            return pixelAzul == 0 && pixelRojo == 0 ? null : new Stats {Mana = decimal.ToInt32(pixelAzul*100/ (pixelAzulMax+1)), Vida = decimal.ToInt32(pixelRojo * 100 / (pixelRojoMax+1)) };
        }

        private void LeerStats(Bitmap bit, out int pixelAzul, out int pixelRojo)
        {
            BitmapData data = bit.LockBits(new Rectangle(0, 0, bit.Width, bit.Height), ImageLockMode.ReadOnly, bit.PixelFormat);

            IntPtr ptr = data.Scan0;
            int length = data.Stride*bit.Height; //Cantidad de bytes
            var pixelSize = data.PixelFormat == PixelFormat.Format32bppArgb ? 4 : 3;
                // only works with 32 or 24 pixel-size bitmap!
            var padding = data.Stride - (data.Width*pixelSize);

            byte[] rgbValues = new byte[length]; // Array del tamaño del bitmap

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgbValues, 0, length);

            int count = 0;
            pixelRojo = 0;
            pixelAzul = 0;


            var index = 0;

            for (var y = 0; y < data.Height; y++)
            {
                for (var x = 0; x < data.Width; x++)
                {
                    var r = rgbValues[index + 2];
                    var g = (rgbValues[index + 1]);
                    var b = (rgbValues[index]);
                    if (r == 253 && g == 88 && b == 53)
                    {
                        pixelRojo++;
                    }
                    if (r == 53 && g == 217 && b == 253)
                    {
                        pixelAzul++;
                    }

                    index += pixelSize;
                }

                index += padding;
            }

            bit.UnlockBits(data);
        }

        //SELECCIONAR OBJETIVO
        public void ObtenerObjetivo()
        {
            MouseProvider.GetMouseProvider("ROClientGame").PosicionarMouse(10,10);
        }

        //EVENTOS
        private void EjecutarEvento(object obj, EventType name)
        {
            if (_frameEventHandlers.ContainsKey(name) && _frameEventHandlers[name] != null)
            {
                _frameEventHandlers[name].ForEach(x => x.Ejecutar(obj));
            }
        }

        public void RegistrarHandler(EventType name, IFrameEventHandler frameEventHandler)
        {
            if (_frameEventHandlers.ContainsKey(name))
            {
                _frameEventHandlers[name].Add(frameEventHandler);
            }
            else
            {
                _frameEventHandlers.Add(name, new List<IFrameEventHandler> { frameEventHandler });
            }
        }
    }

}
