using Microsoft.EntityFrameworkCore;
using sistema_de_inventario_y_ventas.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var origenesPermitidos = builder.Configuration.GetValue<string>("OrigenesPermitidos")!.Split(",");

builder.Services.AddCors(opciones => {
        opciones.AddDefaultPolicy(politica => {
                politica.WithOrigins(origenesPermitidos).AllowAnyHeader().AllowAnyMethod();
        });
});

builder.Services.AddDbContext<AppDbContext>(opciones => opciones.UseSqlServer("name=DefaultConnection"));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
