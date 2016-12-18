using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dominio.Handlers;
using RegnumBotWin.Handlers;
using Servicios;

namespace RegnumBotWin
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;

            RegnumReader.GetRegnumReader().RegistrarHandler(EventType.CoordenadasBitmap, new FrameEventHandler(CoordenadasImg));
            RegnumReader.GetRegnumReader().RegistrarHandler(EventType.CoordenadasTexto, new TextEventHandler(coordenadasText));
            RegnumReader.GetRegnumReader().RegistrarHandler(EventType.StatsBitmap, new FrameEventHandler(VidaImg));

            Consola.Text += "Iniciando Busqueda de Coordenadas y Stats..." + "\r\n";
            Task.Run(() => BuscarCoordenadas());
            Task.Run(() => BuscarStats());
        }

        private void BuscarStats()
        {
            var encontrada = RegnumReader.GetRegnumReader().BuscarStats();
            Consola.Text += "Vida y Mana" + (encontrada != null ? $" encontradas {encontrada.Vida}% / {encontrada.Mana}%" : " no encontradas") + "\r\n";
        }

        private void ObtenerStats()
        {
            var encontrada = RegnumReader.GetRegnumReader().ObtenerStats();
            Consola.Text += "Vida y Mana" + (encontrada != null ? $"{encontrada.Vida}% / {encontrada.Mana}%" : " no encontradas") + "\r\n";
        }

        private void BuscarCoordenadas()
        {
            var encontrada = RegnumReader.GetRegnumReader().BuscarCoordenadas();
            Consola.Text += "Coordenadas" + (encontrada != null ? $" encontradas {encontrada.X} : {encontrada.Y}" : " no encontradas") + "\r\n";
        }

        private void ObtenerCoordenadas()
        {
            var encontrada = RegnumReader.GetRegnumReader().ObtenerCoordenadas();
            Consola.Text += "Coordenadas" + (encontrada != null ? $"{encontrada.X} : {encontrada.Y}" : " no encontradas") + "\r\n";
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                var comando = textBox1.Text;
                textBox1.Text = "";

                if (comando == "oc")
                {
                    Task.Run(() => ObtenerCoordenadas());
                }
                if (comando == "os")
                {
                    Task.Run(() => ObtenerStats());
                }
                if (comando == "oo")
                {
                    RegnumReader.GetRegnumReader().ObtenerObjetivo();
                }
            }
        }
    }
}
