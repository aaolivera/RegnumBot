using System.Drawing;

namespace Servicios.InternalProviders
{
    /// <summary>
    /// Provides functions to capture the entire screen, or a particular window, and save it to a file.
    /// </summary>
    public class ColorProvider
    {
        public static Color[] Piedra()
        {
            return new [] { Color.FromArgb(253, 88, 53), Color.FromArgb(253, 88, 53) , Color.FromArgb(253, 88, 53) };
        }

        public static Color RojoVida()
        {
            return Color.FromArgb(253, 88, 53);
        }

        public static Color AzulMana()
        {
            return Color.FromArgb(53, 217, 253);
        }
        
        public static Color VerdeMuyFacil()
        {
            return Color.FromArgb(160, 255, 140);
        }

        public static Color VerdeFacil()
        {
            return Color.FromArgb(120, 255, 120);
        }

        public static Color AzulNormal()
        {
            return Color.FromArgb(140, 160, 255);
        }
    }
}