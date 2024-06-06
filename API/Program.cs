using API.Middleware;
using API.Services;
using API.Services.Interfaces;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddSingleton(new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

builder.Services.AddHttpClient();
builder.Services.AddSingleton<IHttpClientService, HttpClientService>();

builder.Services.AddSingleton<UserService>();

builder.Services.AddControllers();

builder.Services.AddCors();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200"));

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
