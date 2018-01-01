using Dominio.Handlers;
using Servicios.InternalProviders;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Servicios.RegnumProviders
{
    public class PiedraProvider : RegnumProvider
    {

        public PiedraProvider(FrameProvider frameProvider, MouseProvider mouseProvider) : base(frameProvider, mouseProvider)
        {
        }

        public Point? Obtener()
        {
            //_mouseProvider.PosicionarMouse(x, y);

            var bit = _frameProvider.PrintWindow();
            var retorno = LeerPiedra(bit).Result;
            EjecutarEvento(bit, EventType.PiedraBitmap);
            return retorno;
        }

        private async Task<Point?> LeerPiedra(Bitmap bit)
        {
            int threds = 8;
            BitmapData data = bit.LockBits(new Rectangle(0, 0, bit.Width, bit.Height), ImageLockMode.ReadOnly, bit.PixelFormat);

            IntPtr ptr = data.Scan0;
            int length = data.Stride * bit.Height; //Cantidad de bytes
            var pixelSize = data.PixelFormat == PixelFormat.Format32bppArgb ? 4 : 3;
            // only works with 32 or 24 pixel-size bitmap!
            var padding = data.Stride - (data.Width * pixelSize);



            // Copy the RGB values into the array.
            var pixelsPorThred = length / threds;


            ////
            for (int i = 0; i < threds; i += pixelsPorThred)
            {
                byte[] rgbValues = new byte[pixelsPorThred]; // Array del tamaño del bitmap
                Marshal.Copy(ptr, rgbValues, i, pixelsPorThred);
                var i1 = i;
                var val = await Task.Run(() => RecorrerImagen(rgbValues, data.Height / threds, data.Width, i1, pixelSize, padding));
                if (val != null)
                {
                    bit.UnlockBits(data);
                    return val;
                }
            }

            bit.UnlockBits(data);
            return null;
        }

        private static Point? RecorrerImagen(byte[] rgbValues, int height, int width, int deltaHeight, int pixelSize, int padding)
        {
            var index = 0;
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var r = rgbValues[index + 2];
                    var g = rgbValues[index + 1];
                    var b = rgbValues[index];
                    var color = Color.FromArgb(r, g, b);
                    if (ColorProvider.Piedra().Contains(color))
                    {
                        return new Point(x, y + deltaHeight);
                    }

                    index += pixelSize;
                }

                index += padding;
            }
            return null;
        }

    }
}
