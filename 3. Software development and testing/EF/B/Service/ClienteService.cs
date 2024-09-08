using B.Data;
using Microsoft.EntityFrameworkCore;

namespace B.Service
{
    public class ClienteService
    {
        private readonly DataContext _context;
        public ClienteService(DataContext context)
        {
            _context = context;
        }

        public async Task<Cliente> GetClienteConPedidosById(int clienteId)
        {
            if (clienteId <= 0) throw new ArgumentException();
            var clienteConPedidos = await _context.Clientes.Include(c => c.Pedidos).FirstOrDefaultAsync(c => c.Id == clienteId);
            if (clienteConPedidos == null)
            {
                throw new ArgumentException("No se encontró un cliente con el ID proporcionado.");
            }

            return clienteConPedidos;
        }
    }
}
