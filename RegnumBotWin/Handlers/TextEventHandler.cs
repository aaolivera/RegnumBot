using System.Drawing;
using System.Windows.Forms;
using Dominio.Handlers;

namespace RegnumBotWin.Handlers
{
    public class TextEventHandler : IFrameEventHandler
    {
        private TextBox _textBox;

        public TextEventHandler(TextBox textBox)
        {
            _textBox = textBox;
        }

        public void Ejecutar(object obj)
        {
            _textBox.Text = (string)obj;
        }
    }
}