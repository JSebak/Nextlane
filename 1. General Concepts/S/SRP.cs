// Una clase que maneja tanto la lógica de productos como el guardado, no aplica el SRP.
public class ProductManagerNotSRP
{
    public void AddProduct(string name)
    {
        Console.WriteLine($"Producto {name} agregado.");

        SaveToDatabase(name);
    }

    private void SaveToDatabase(string name)
    {
        Console.WriteLine($"Producto {name} guardado en la base de datos.");
    }
}

// Aplicación del SRP: Separar la lógica de productos y el guardado en diferentes clases.
public class ProductManagerSRP
{
    private readonly DatabaseService _databaseService;

    public ProductManagerSRP(DatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    public void AddProduct(string name)
    {
        Console.WriteLine($"Producto {name} agregado.");
        _databaseService.SaveToDatabase(name);
    }
}

public class DatabaseService
{
    public void SaveToDatabase(string name)
    {
        Console.WriteLine($"Producto {name} guardado en la base de datos.");
    }
}
