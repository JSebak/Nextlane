namespace D
{
    public class Vehiculo
    {
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public int Año { get; set; }
    }

    public class Automovil : Vehiculo
    {
        public string TipoDeCombustible { get; set; }

        public static double CalcularEficiencia(int distancia, double consumo)
        {
            return distancia / consumo;
        }
    }
}
