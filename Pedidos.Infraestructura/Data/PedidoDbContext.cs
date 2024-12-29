using Microsoft.EntityFrameworkCore;
using Pedidos.Dominio.Models;

namespace Pedidos.Infraestructura.Data
{
    public class PedidoDbContext : DbContext
    {
        public PedidoDbContext(DbContextOptions<PedidoDbContext> options)
            : base(options) { }

        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<DetallePedido> DetallePedidos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Clave primaria de DetallePedido es compuesta {PedidoId, ProductoId}
            modelBuilder.Entity<DetallePedido>()
                .HasKey(d => new { d.PedidoId, d.ProductoId });

            //Relacion Pedido -> DetallePedido (uno a muchos)
            modelBuilder.Entity<DetallePedido>()
                .HasOne<Pedido>()
                .WithMany(p => p.Detalles)
                .HasForeignKey(d => d.PedidoId);

            //Ejemplo de configuracion decimal
            modelBuilder.Entity<DetallePedido>()
                .Property(p => p.PrecioUnitario)
                .HasColumnType("decimal(18,2)");
        }
    }
}
