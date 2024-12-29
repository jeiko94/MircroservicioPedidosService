using Pedidos.Dominio.Models;

namespace Pedidos.Aplicacion.Repositorios
{
    public interface IPedidoRepositorio
    {
        Task CrearAsync(Pedido pedido);
        Task<Pedido> ObtenerPorIdAsync(int id);
        Task<IEnumerable<Pedido>> ObtenerPorClienteAsync(int clienteId);
        Task ActualizarAsync(Pedido pedido);
        Task EliminarAsync(int id);
    }
}
