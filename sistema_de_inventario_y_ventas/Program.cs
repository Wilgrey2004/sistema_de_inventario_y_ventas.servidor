using Microsoft.EntityFrameworkCore;
using sistema_de_inventario_y_ventas.Context;

var builder = WebApplication.CreateBuilder(args);

// --------------------- SERVICIOS ---------------------

// Agrega soporte para controladores (API REST)
builder.Services.AddControllers();

// Agrega soporte para Swagger/OpenAPI (documentaci�n de tu API)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Obtener los or�genes permitidos desde appsettings.json
// Esto permite que diferentes PCs en la red accedan a la API
var origenesPermitidos = builder.Configuration.GetValue<string>("OrigenesPermitidos")?.Split(",")
                         ?? new string[] { "http://localhost:4200" }; // fallback a localhost

// Configuraci�n CORS: permite que Angular u otros frontends accedan a la API
builder.Services.AddCors(options => {
        options.AddPolicy("AllowAngularLocalhost",
            policy => {
                    policy.WithOrigins(origenesPermitidos) // dominios permitidos
                      .AllowAnyHeader()              // permite cualquier encabezado HTTP
                      .AllowAnyMethod();             // permite cualquier m�todo HTTP (GET, POST, PUT, DELETE, etc)
            });
});

// Configuraci�n de la base de datos usando Entity Framework y SQL Server
builder.Services.AddDbContext<AppDbContext>(opciones =>
    opciones.UseSqlServer("name=DefaultConnection") // "DefaultConnection" debe estar en appsettings.json
);

// Configura la URL donde la API escuchar� peticiones
// 0.0.0.0 permite recibir peticiones desde cualquier IP de la red
builder.WebHost.UseUrls("http://0.0.0.0:5000"); // solo HTTP por ahora, evita problemas de certificados

// --------------------- APLICACI�N ---------------------

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

// Activar autorizaci�n (JWT, roles, etc) si usas Auth
app.UseAuthorization();

// Mapear los controladores (API endpoints)
app.MapControllers();

// Ejecutar la aplicaci�n
app.Run();
