using FinalAnalisis.Data;
using FinalAnalisis.Services;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Configuración Clásica de Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<FinalAnalisisContext>(options =>
    options.UseSqlite("Data Source=finalanalisis.db"));

builder.Services.AddScoped<IIncidenteService, IncidenteService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<FinalAnalisisContext>();
    context.Database.EnsureCreated();
}

// Activar Swagger Visual
app.UseSwagger();
app.UseSwaggerUI(c => 
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "FinalAnalisis API v1");
    c.RoutePrefix = "swagger"; // Esto define que la URL sea /swagger
});

app.UseAuthorization();
app.MapControllers();

var puerto = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Run($"http://0.0.0.0:{puerto}");