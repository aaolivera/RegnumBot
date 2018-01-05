using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dominio.Handlers;
using RegnumBotWin.Handlers;
using Servicios.RegnumProviders;

namespace RegnumBotWin
{
    public partial class Form1 : Form
    {
        private readonly CoordenadasProvider coordenadasProvider;
        private readonly StatsProvider statsProvider;
        private readonly ObjetivoProvider objetivoProvider;
        private readonly PiedraProvider piedraProvider;
        private readonly AventuraProvider aventuraProvider;

        public Form1(CoordenadasProvider coordenadasProvider, StatsProvider statsProvider, ObjetivoProvider objetivoProvider, PiedraProvider piedraProvider, AventuraProvider aventuraProvider)
        {
            InitializeComponent();
            this.coordenadasProvider = coordenadasProvider;
            this.statsProvider = statsProvider;
            this.objetivoProvider = objetivoProvider;
            this.piedraProvider = piedraProvider;
            this.aventuraProvider = aventuraProvider;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;

            coordenadasProvider.RegistrarHandler(EventType.CoordenadasBitmap, new FrameEventHandler(CoordenadasImg));
            coordenadasProvider.RegistrarHandler(EventType.CoordenadasTexto, new TextEventHandler(coordenadasText));
            aventuraProvider.RegistrarHandler(EventType.AventuraBitmap, new FrameEventHandler(CoordenadasImg));
            aventuraProvider.RegistrarHandler(EventType.AventuraTexto, new TextEventHandler(coordenadasText));
            statsProvider.RegistrarHandler(EventType.StatsBitmap, new FrameEventHandler(VidaImg));
            objetivoProvider.RegistrarHandler(EventType.ObjetivoBitmap, new FrameEventHandler(VidaImg));
            piedraProvider.RegistrarHandler(EventType.PiedraBitmap, new FrameEventHandler(pictureBox1));
        }
        
        private void ObtenerStats()
        {
            var encontrada = statsProvider.Obtener();
            Consola.Text += "Vida y Mana" + (encontrada != null ? $"{encontrada.Vida}% / {encontrada.Mana}%" : " no encontradas") + "\r\n";
        }

        private void ObtenerAventura()
        {
            var encontrada = aventuraProvider.Obtener();
            if(encontrada != null)
            {
                Consola.Text += " Entregar carta desde: " + encontrada.Desde + " hasta " + encontrada.Hasta + "\r\n";
            }            
        }

        private void ObtenerCoordenadas()
        {
            var encontrada = coordenadasProvider.Obtener();
            Consola.Text += "Coordenadas" + (encontrada != null ? $"{encontrada.X} : {encontrada.Y}" : " no encontradas") + "\r\n";
        }

        private void ObtenerPiedra()
        {
            var k = 0;
            Point? encontrada = null;
            var a = DateTime.Now;
            encontrada = piedraProvider.Obtener();
            var b = (DateTime.Now - a).Milliseconds;
            Consola.Text += " Tiempo: " + b + "///  encontradas " + (encontrada?.ToString() ?? "no") + "/// cantidad " + k + "\r\n";
        }

        private void ObtenerObjetivo()
        {
            objetivoProvider.Obtener();
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
                    Task.Run(() => ObtenerObjetivo());
                }
                if (comando == "op")
                {
                    Task.Run(() => ObtenerPiedra());
                }
                if (comando == "oa")
                {
                    Task.Run(() => ObtenerAventura());
                }
            }
        }
    }
}
