using Pedidos.Aplicacion.Repositorios;
using Pedidos.Infraestructura.Repositorios;
using Pedidos.Aplicacion.Servicios;
using Pedidos.Infraestructura.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Leer la cadena de conexion
string connectionString = builder.Configuration.GetConnectionString("PedidosConnection");

//Registrar DbContext
builder.Services.AddDbContext<PedidoDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IPedidoRepositorio, PedidoRepositorio>();
builder.Services.AddScoped<ServicioPedido>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
