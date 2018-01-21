using System.Drawing;

namespace Dominio
{
    public class Coordenada
    {
        public Point Posicion { get; set; }        
        public decimal Direccion { get; set; }

        public override string ToString()
        {
            return $"Coordenada actual Posicion:{Posicion}, Direccion:{Direccion}";
        }
    }
}
