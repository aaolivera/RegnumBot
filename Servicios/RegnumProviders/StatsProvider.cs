using Dominio;
using Dominio.Handlers;
using Ninject.Extensions.Logging;
using Servicios.InternalProviders;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Servicios.RegnumProviders
{
    public class StatsProvider : RegnumProvider
    {
        private Rectangle _posicionStats;
        public StatsProvider(FrameProvider frameProvider, ILogger log) : base(frameProvider, null, log)
        {
            this._posicionStats = new Rectangle(0, 0, 220, 200);
        }

        private static int pixelRojoMax = -1;
        private static int pixelAzulMax = -1;
        public Stats Obtener()
        {
            var bit = _frameProvider.GetPartial(_posicionStats.X, _posicionStats.Y, _posicionStats.Width, _posicionStats.Height);

            int leidosAzul;
            int leidosRojo;
            Leer(bit, out leidosAzul, out leidosRojo);

            if ((leidosAzul == 0 && leidosRojo == 0) || (pixelAzulMax == -1 && pixelRojoMax == -1 && (leidosAzul == 0 || leidosRojo == 0)))
            {
                return null;
            }
            else if (pixelAzulMax == -1 && pixelRojoMax == -1 && leidosAzul != 0 && leidosRojo != 0)
            {
                pixelAzulMax = leidosAzul;
                pixelRojoMax = leidosRojo;
                EjecutarEvento(bit, EventType.StatsBitmap);
                EjecutarEvento($"{pixelRojoMax}-{pixelAzulMax}", EventType.StatsTexto);
                return new Stats { Mana = 100, Vida = 100 };
            }

            EjecutarEvento(bit, EventType.StatsBitmap);
            EjecutarEvento($"{leidosRojo}-{leidosAzul}", EventType.StatsTexto);

            return new Stats { Mana = decimal.ToInt32(leidosAzul * 100 / (pixelAzulMax + 1)), Vida = decimal.ToInt32(leidosRojo * 100 / (pixelRojoMax + 1)) };
        }
        
        private void Leer(Bitmap bit, out int pixelAzul, out int pixelRojo)
        {
            BitmapData data = bit.LockBits(new Rectangle(0, 0, bit.Width, bit.Height), ImageLockMode.ReadOnly, bit.PixelFormat);

            IntPtr ptr = data.Scan0;
            int length = data.Stride * bit.Height; //Cantidad de bytes
            var pixelSize = data.PixelFormat == PixelFormat.Format32bppArgb ? 4 : 3;
            // only works with 32 or 24 pixel-size bitmap!
            var padding = data.Stride - (data.Width * pixelSize);

            byte[] rgbValues = new byte[length]; // Array del tamaño del bitmap

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgbValues, 0, length);
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
                    if (r == ColorProvider.RojoVida().R && g == ColorProvider.RojoVida().G && b == ColorProvider.RojoVida().B)
                    {
                        pixelRojo++;
                    }
                    if (r == ColorProvider.AzulMana().R && g == ColorProvider.AzulMana().G && b == ColorProvider.AzulMana().B)
                    {
                        pixelAzul++;
                    }

                    index += pixelSize;
                }

                index += padding;
            }

            bit.UnlockBits(data);
        }
    }
}
