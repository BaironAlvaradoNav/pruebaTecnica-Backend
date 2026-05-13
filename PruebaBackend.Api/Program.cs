using PruebaBackend.Api.Application.Interfaces;
using PruebaBackend.Api.Application.Services;
using PruebaBackend.Api.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IContactRepository, InMemoryContactRepository>();
builder.Services.AddSingleton<IContactService, ContactService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.Run();

public partial class Program;
