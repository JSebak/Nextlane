int id = -1;

while (id < 0)
{
    try
    {
        Console.WriteLine("Por favor ingrese un Id (número entero):");
        string? input = Console.ReadLine();

        if (!int.TryParse(input, out id))
        {

            throw new ArgumentException("El Id debe ser un número entero.");
        }
        if (id < 0)
        {
            throw new ArgumentException("El Id no puede ser nulo o menor que 0. Intente nuevamente.");
        }
        else
        {
            Console.WriteLine($"Id guardado: {id}");
        }

    }
    catch (ArgumentException ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
        Console.WriteLine("El Id no puede ser nulo o menor que 0. Intente nuevamente.");
        id = -1;
    }
}

Console.WriteLine($"ID válido ingresado: {id}");
