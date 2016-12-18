using System.Drawing;
using System.Windows.Forms;
using Dominio.Handlers;

namespace RegnumBotWin.Handlers
{
    public class FrameEventHandler : IFrameEventHandler
    {
        private PictureBox _pictureBox;

        public FrameEventHandler(PictureBox pictureBox)
        {
            _pictureBox = pictureBox;
        }

        public void Ejecutar(object obj)
        {
            _pictureBox.Image = (Bitmap)obj;
        }
    }
}