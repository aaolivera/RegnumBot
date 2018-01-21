namespace Dominio
{
    public class Aventura
    {
        public string Desde { get; set; }
        public string Hasta { get; set; }

        public override string ToString()
        {
            return $"Aventura encontrada Desde:{Desde}, Hasta:{Hasta}";
        }
    }
}
