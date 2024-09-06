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

        public double CalcularEficiencia(int distancia, double consumo)
        {
            if (TipoDeCombustible == "diesel") consumo = consumo * 0.8;
            return distancia / consumo;
        }
    }
}
