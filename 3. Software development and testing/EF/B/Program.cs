// See https://aka.ms/new-console-template for more information
using B.Data;
using B.Service;

using (var context = new DataContext("MyInMemoryDb"))
{
    var clientes = new List<Cliente>
    {
        new Cliente
        {
            Nombre = "Cliente 1",
            Pedidos = new List<Pedido>
                {
                    new Pedido { Titulo = "Pedido 1", Detalle = "2 Productos 2334" },
                    new Pedido { Titulo = "Pedido 2", Detalle = "13 Productos 0084" }
                }
        },
        new Cliente
        {
            Nombre = "Cliente 2",
            Pedidos = new List<Pedido>
                {
                    new Pedido { Titulo = "Pedido 3", Detalle = "5 Productos 6890" },
                }
        }
    };

    context.Clientes.AddRange(clientes);
    await context.SaveChangesAsync();

    var clienteService = new ClienteService(context);
    var clienteConPedidos = await clienteService.GetClienteConPedidosById(clientes[0].Id);

    foreach (var pedido in clienteConPedidos.Pedidos.ToList())
    {
        Console.WriteLine($"Pedido: {pedido.Titulo}, Detalle: {pedido.Detalle}, Cliente: {clienteConPedidos.Nombre}");
    }
}
