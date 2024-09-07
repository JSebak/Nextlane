using D;

var corsa2000 = new Automovil { Año = 2000, Marca = "Chevrolet", Modelo = "Corsa", TipoDeCombustible = "Diesel" };
var R8 = new Automovil { Año = 2010, Marca = "Audi", Modelo = "R8", TipoDeCombustible = "Gasolina" };
Console.WriteLine($"Cosumo {corsa2000.Marca} {corsa2000.Modelo} {corsa2000.Año} es de {Automovil.CalcularEficiencia(200, 40)} Km/L");
Console.WriteLine($"Cosumo {R8.Marca} {R8.Modelo} {R8.Año} es de {Automovil.CalcularEficiencia(200, 62)} Km/L");