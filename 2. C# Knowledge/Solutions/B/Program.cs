
var TemperaturaActualCelcius = 0;
var termostato = new Termostato { TemperaturaMaxKelvin = 500 };
while (!termostato.Apagado)
{
    Console.WriteLine($"Temperatura actual Celsius: {TemperaturaActualCelcius}, Kelvin {Termostato.CelsiusAKelvin(TemperaturaActualCelcius)}"); // Método Estático
    termostato.RevisarTemperatura(Termostato.CelsiusAKelvin(TemperaturaActualCelcius)); // Método instanciado
    TemperaturaActualCelcius += 100;
}
Console.WriteLine("Termostato apagado");
public class Termostato
{
    public int TemperaturaMaxKelvin { get; set; }
    public bool Apagado { get; set; } = false;

    public static double CelsiusAKelvin(double celcius) //No depende de las propiedades de Termostato
    {
        return celcius + 273;
    }
    public static double KelvinACelcius(double kelvin)
    {
        return kelvin - 273;
    } //No depende de las propiedades de Termostato
    public bool RevisarTemperatura(double temperaturaKelvin) //Depende de las propiedades de un objeto Termostato
    {
        if (temperaturaKelvin >= TemperaturaMaxKelvin)
        {
            Apagado = true;
            Console.WriteLine($"Temperatura máxima superada {TemperaturaMaxKelvin} Kelvin ({KelvinACelcius(TemperaturaMaxKelvin)} Celcius)");
        }
        else Apagado = false;
        return Apagado;
    }
}