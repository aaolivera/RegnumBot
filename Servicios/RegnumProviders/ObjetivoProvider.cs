using Dominio.Handlers;
using Ninject.Extensions.Logging;
using Servicios.InternalProviders;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Servicios.RegnumProviders
{
    public class ObjetivoProvider : RegnumProvider
    {

        public ObjetivoProvider(FrameProvider frameProvider, MouseProvider mouseProvider, ILogger log) : base(frameProvider, mouseProvider, log)
        {
        }

        public void Obtener()
        {
            var x = 32;
            var y = 32;
            while (true)
            {
                _mouseProvider.PosicionarMouse(x, y);

                var bit = _frameProvider.GetPartial((x > 30) ? x - 30 : x, (y > 30) ? y - 30 : y, 60, 60);


                if (Leer(bit))
                {
                    EjecutarEvento(bit, EventType.ObjetivoBitmap);
                    return;
                }
                EjecutarEvento(bit, EventType.ObjetivoBitmap);
                if (x < 1500)
                {
                    x += 20;
                }
                else if (y < 200)
                {
                    x = 0;
                    y += 30;
                }
                else
                {
                    return;
                }
            }
        }

        private bool Leer(Bitmap bit)
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

            var index = 0;

            for (var y = 0; y < data.Height; y++)
            {
                for (var x = 0; x < data.Width; x++)
                {
                    var r = rgbValues[index + 2];
                    var g = (rgbValues[index + 1]);
                    var b = (rgbValues[index]);
                    var color = Color.FromArgb(r, g, b);
                    if (color == ColorProvider.AzulNormal() ||
                        color == ColorProvider.VerdeFacil() ||
                        color == ColorProvider.VerdeMuyFacil()
                        )
                    {
                        bit.UnlockBits(data);
                        return true;
                    }

                    index += pixelSize;
                }

                index += padding;
            }
            bit.UnlockBits(data);

            return false;
        }
    }
}
