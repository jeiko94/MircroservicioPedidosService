using Pedidos.Aplicacion.Repositorios;
using Pedidos.Dominio.Models;

namespace Pedidos.Aplicacion.Servicios
{
    public class ServicioPedido
    {
        private readonly IPedidoRepositorio _pedidoRepositorio;

        public ServicioPedido(IPedidoRepositorio pedidoRepositorio)
        {
            _pedidoRepositorio = pedidoRepositorio;
        }

        //Crear un pedido vacio (pendiente) para un cliente dado
        public async Task<int> CrearPedidoAsync(int clienteId)
        {
            var pedido = new Pedido
            {
                ClienteId = clienteId,
                FechaCreacion = DateTime.Now,
                Estado = EstadoPedido.Pendiente,
            };

            await _pedidoRepositorio.CrearAsync(pedido);
            return pedido.Id;
        }

        //Agrega un producto al pedido, estableciendo cantidad y precio unitario
        //Se asume que el precio se obtiene (o se obtiene en el futuro) del servicio catalogo
        //Al momento de añadir el producto, o se pasa como parametro
        public async Task AgregarProductoAsync(int pedidoId, int productoId, int cantidad, decimal precio)
        {
            //Obtener el pedido
            var pedido = await _pedidoRepositorio.ObtenerPorIdAsync(pedidoId);

            if(pedido == null)
                throw new InvalidOperationException("Pedido no encontrado");

            if(pedido.Estado != EstadoPedido.Pendiente)
                throw new InvalidOperationException("No se pueden agregar productos a un pedido que no esta pendiente");

            //Agregar el detalle
            var detalle = new DetallePedido
            {
                PedidoId = pedidoId,
                ProductoId = productoId,
                Cantidad = cantidad,
                PrecioUnitario = precio
            };

            pedido.Detalles.Add(detalle);

            //Actualizar el pedido
            await _pedidoRepositorio.ActualizarAsync(pedido);
        }

        public async Task<Pedido> ObtenerPedidoAsync(int id)
        {
            return await _pedidoRepositorio.ObtenerPorIdAsync(id);
        }

        public async Task<IEnumerable<Pedido>> ObteperPedidosClienteAsync(int clienteId)
        {
            return await _pedidoRepositorio.ObtenerPorClienteAsync(clienteId);
        }

        //Cambia el estado a pagado, por ejemplo, luego de un pago exitoso
        public async Task MarcarPagadoAsync(int pedidoId)
        {
            var pedido = await _pedidoRepositorio.ObtenerPorIdAsync(pedidoId);

            if (pedido == null)
                throw new InvalidOperationException("Pedido no encontrado");

            if(pedido.Estado != EstadoPedido.Pendiente)
                throw new InvalidOperationException("Solo un pedido pendiente se puede marcar como pagado.");

            pedido.Estado = EstadoPedido.Pagado;
            await _pedidoRepositorio.ActualizarAsync(pedido);
        }

        //Cambia el estado a enviado, por ejemplo, luego de que se envia el pedido
        public async Task MarcarEnviadoAsync(int pedidoId)
        {
            var pedido = await _pedidoRepositorio.ObtenerPorIdAsync(pedidoId);

            if(pedido == null)
                throw new InvalidOperationException("Pedido no encontrado");

            if(pedido.Estado != EstadoPedido.Pagado)
                throw new InvalidOperationException("Solo un pedido pagado se puede marcar como enviado.");

            pedido.Estado = EstadoPedido.Enviado;

            await _pedidoRepositorio.ActualizarAsync(pedido);
        }

        public async Task EliminarPedidoAsync(int id)
        {
            await _pedidoRepositorio.EliminarAsync(id);
        }
    }
}
