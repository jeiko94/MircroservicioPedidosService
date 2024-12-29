using Microsoft.AspNetCore.Mvc;
using Pedidos.Api.DTOs;
using Pedidos.Aplicacion.Servicios;
using Pedidos.Dominio.Models;
using System.Linq;

namespace Pedidos.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidosController : ControllerBase
    {
        private readonly ServicioPedido _pedidoServicio;

        public PedidosController(ServicioPedido pedidoServicio)
        {
            _pedidoServicio = pedidoServicio;
        }

        /// <summary>
        /// Crea un nuevo pedido en estado Pendiente para un cliente.
        /// POST /api/pedidos
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CrearPedido([FromBody] CrearPedidoDto dto)
        {
            int pedidoId = await _pedidoServicio.CrearPedidoAsync(dto.ClienteId);
            return Ok($"Pedido creado con Id = {pedidoId}");
        }

        /// <summary>
        /// Agrega un producto al pedido (estado Pendiente).
        /// POST /api/pedidos/{pedidoId}/agregar-producto
        /// </summary>
        [HttpPost("{pedidoId}/agregar-producto")]
        public async Task<IActionResult> AgregarProducto(int pedidoId, [FromBody] AgregarPedidoDto dto)
        {
            // Por ahora, asumimos que el dto incluye el precioUnitario.
            // En futuro, se puede consultar al servicio Catálogo para obtener el precio.
            try
            {
                await _pedidoServicio.AgregarProductoAsync(pedidoId, dto.ProductoId, dto.Cantidad, dto.PrecioUnitario);
                return Ok("Producto agregado al pedido.");
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        /// <summary>
        /// Obtiene un pedido por su Id.
        /// GET /api/pedidos/{pedidoId}
        /// </summary>
        [HttpGet("{pedidoId}")]
        public async Task<IActionResult> ObtenerPedido(int pedidoId)
        {
            var pedido = await _pedidoServicio.ObtenerPedidoAsync(pedidoId);
            if (pedido == null)
                return NotFound("Pedido no encontrado.");

            var pedidoDto = MapearPedidoDto(pedido);
            return Ok(pedidoDto);
        }

        /// <summary>
        /// Cambia el estado de un pedido a Pagado.
        /// POST /api/pedidos/{pedidoId}/marcar-pagado
        /// </summary>
        [HttpPost("{pedidoId}/marcar-pagado")]
        public async Task<IActionResult> MarcarPagado(int pedidoId)
        {
            try
            {
                await _pedidoServicio.MarcarPagadoAsync(pedidoId);
                return Ok("Pedido marcado como Pagado.");
            }
            catch (InvalidOperationException ex)
            {
                // Si el pedido no existe o su estado no permite el cambio, 
                // lanzamos Conflict con el mensaje de la excepción.
                return Conflict(ex.Message);
            }
        }

        /// <summary>
        /// Cambia el estado de un pedido a Enviado.
        /// POST /api/pedidos/{pedidoId}/marcar-enviado
        /// </summary>
        [HttpPost("{pedidoId}/marcar-enviado")]
        public async Task<IActionResult> MarcarEnviado(int pedidoId)
        {
            try
            {
                await _pedidoServicio.MarcarEnviadoAsync(pedidoId);
                return Ok("Pedido marcado como Enviado.");
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        // Métodos auxiliares
        private PedidoDto MapearPedidoDto(Pedido pedido)
        {
            return new PedidoDto
            {
                Id = pedido.Id,
                ClienteId = pedido.ClienteId,
                FechaCreacion = pedido.FechaCreacion,
                Estado = pedido.Estado.ToString(),
                Detalles = pedido.Detalles.Select(d => new DetallePedidoDto
                {
                    ProductoId = d.ProductoId,
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario
                }).ToList(),
                Total = pedido.CalcularTotal()
            };
        }
    }
}
