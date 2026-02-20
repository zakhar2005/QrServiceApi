var builder = WebApplication.CreateBuilder(args);

// Добавление сервисов
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Настройка метаданных для Swagger
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "PartyMaker API",
        Version = "v1",
        Description = "Веб-сервис для генерации QR-приглашений в комнаты мероприятий",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "zakhar13",
            Email = "shilin2005@list.ru"
        }
    });
});

var app = builder.Build();

// Настройка конвейера HTTP-запросов
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "PartyMaker API V1");
        c.RoutePrefix = string.Empty; // Открывает Swagger по корневому URL (опционально)
    });
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();