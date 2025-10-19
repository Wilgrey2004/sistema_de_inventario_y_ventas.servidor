using Microsoft.EntityFrameworkCore;
using sistema_de_inventario_y_ventas.Context;

var builder = WebApplication.CreateBuilder(args);

// --------------------- SERVICIOS ---------------------

// Agrega soporte para controladores (API REST)
builder.Services.AddControllers();

// Agrega soporte para Swagger/OpenAPI (documentación de tu API)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Obtener los orígenes permitidos desde appsettings.json
// Esto permite que diferentes PCs en la red accedan a la API
var origenesPermitidos = builder.Configuration.GetValue<string>("OrigenesPermitidos")?.Split(",")
                         ?? new string[] { "http://localhost:4200" }; // fallback a localhost

// Configuración CORS: permite que Angular u otros frontends accedan a la API
builder.Services.AddCors(options => {
        options.AddPolicy("AllowAngularLocalhost",
            policy => {
                    policy.WithOrigins(origenesPermitidos) // dominios permitidos
                      .AllowAnyHeader()              // permite cualquier encabezado HTTP
                      .AllowAnyMethod();             // permite cualquier método HTTP (GET, POST, PUT, DELETE, etc)
            });
});

// Configuración de la base de datos usando Entity Framework y SQL Server
builder.Services.AddDbContext<AppDbContext>(opciones =>
    opciones.UseSqlServer("name=DefaultConnection") // "DefaultConnection" debe estar en appsettings.json
);

// Configura la URL donde la API escuchará peticiones
// 0.0.0.0 permite recibir peticiones desde cualquier IP de la red
builder.WebHost.UseUrls("http://0.0.0.0:5000"); // solo HTTP por ahora, evita problemas de certificados

// --------------------- APLICACIÓN ---------------------

var app = builder.Build();

// Activar Swagger solo en modo desarrollo
if (app.Environment.IsDevelopment())
{
        app.UseSwagger();
        app.UseSwaggerUI();
}

// --------------------- MIDDLEWARE ---------------------

// Aplicar CORS primero, antes de cualquier otro middleware que pueda bloquear las solicitudes
app.UseCors("AllowAngularLocalhost");

// Activar autorización (JWT, roles, etc) si usas Auth
app.UseAuthorization();

// Mapear los controladores (API endpoints)
app.MapControllers();

// Ejecutar la aplicación
app.Run();
