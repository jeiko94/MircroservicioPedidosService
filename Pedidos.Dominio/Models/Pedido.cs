namespace Pedidos.Dominio.Models
{
    //Representa un pedido realizado por un cliente
    public class Pedido
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }  //Asuminedo que el ID del cliente viene de otro servicio o un DB 
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public EstadoPedido Estado { get; set; } = EstadoPedido.Pendiente;

        //Detalles que indican que productos y cuantos se compraron
        public List<DetallePedido> Detalles { get; set; } = new List<DetallePedido>();

        //Calcular el total sumando cantidad * precio de cada detalle

        public decimal CalcularTotal()
        {
            decimal total = 0;
            foreach (var d in Detalles)
            {
                total += d.Cantidad * d.PrecioUnitario;
            }

            return total;
        }
    }
}
