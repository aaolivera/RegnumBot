using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Nodo
    {
        public Nodo()
        {

        }

        public Nodo(int x, int y)
        {
            X = x;
            Y = y;
            NodosAsociados = new List<Nodo>();
        }

        public static int RadioGeneral { get; set; }
        public int Radio { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public List<Nodo> NodosAsociados { get; set; }

        public double Peso { get; set; }
        public bool Marcado { get; set; }
        public Nodo Anterior { get; set; }

        public bool HayInterseccion(Point p)
        {
            return Distancia(p) <= RadioGeneral * 2;
        }

        public double Distancia(Point p)
        {
            return Math.Sqrt((X - p.X) * (X - p.X) + (Y - p.Y) * (Y - p.Y));
        }

        public double Distancia(Nodo p)
        {
            return Distancia(new Point(p.X, p.Y));
        }

        public void AgregarAsociado(Nodo nodo)
        {
            if (!NodosAsociados.Contains(nodo)) NodosAsociados.Add(nodo);
        }

        public override string ToString()
        {
            return $"({X},{Y})";
        }
    }
}
