using Microsoft.EntityFrameworkCore;
using Pedidos.Aplicacion.Repositorios;
using Pedidos.Dominio.Models;
using Pedidos.Infraestructura.Data;

namespace Pedidos.Infraestructura.Repositorios
{
    public class PedidoRepositorio : IPedidoRepositorio
    {
        private readonly PedidoDbContext _context;

        public PedidoRepositorio(PedidoDbContext context)
        {
            _context = context;
        }

        public async Task CrearAsync(Pedido pedido)
        {
            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();
        }
        public async Task<Pedido> ObtenerPorIdAsync(int id)
        {
            return await _context.Pedidos
                .Include(p => p.Detalles)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task<IEnumerable<Pedido>> ObtenerPorClienteAsync(int clienteId)
        {
            return await _context.Pedidos
                .Include(p => p.Detalles)
                .Where(p => p.ClienteId == clienteId)
                .ToListAsync();
        }
        public async Task ActualizarAsync(Pedido pedido)
        {
            _context.Pedidos.Update(pedido);
            await _context.SaveChangesAsync();
        }
        public async Task EliminarAsync(int id)
        {
            var pedido = await ObtenerPorIdAsync(id);

            if (pedido != null)
            {
                _context.Pedidos.Remove(pedido);
                await _context.SaveChangesAsync();
            }
        }
    }
}
