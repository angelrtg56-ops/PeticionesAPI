using Microsoft.EntityFrameworkCore;
using PeticionesAPI.Data;
using PeticionesAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// Añade el servicio de controladores, que es el que necesita esta API
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Añade el contexto de la base de datos
builder.Services.AddDbContext<PeticionesDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configura CORS para permitir peticiones
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

// Configura el pipeline de peticiones
app.UseHttpsRedirection();

// Activa el servicio CORS
app.UseCors();

app.UseAuthorization();

// Mapea los controladores
app.MapControllers();

app.Run();