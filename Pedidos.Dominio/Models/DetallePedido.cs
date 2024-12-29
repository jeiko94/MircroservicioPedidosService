namespace Pedidos.Dominio.Models
{
    //Detalle que asocia un producto a un pedido con la cantidad y el precio unitario
    public class DetallePedido
    {
        public int PedidoId { get; set; }
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
    }
}
