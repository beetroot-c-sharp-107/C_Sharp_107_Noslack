#define DEEP_DEBUG

using AutoMapper;
using backend;
using backend.DTO.ChatControllerDTO;
using backend.DTO.UserControllerDTO;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ChatDbContext>((services, options) => {
    var configuration = services.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetSection("ConnectionStrings:Postgres").Value;
    options.UseNpgsql(connectionString);
});

#if DEEP_DEBUG
builder.Services.AddAutoMapper(config => {
    config.CreateMap<User, GetUserDTO>();
    config.CreateMap<CreateUserDTO, User>();
    config.CreateMap<Chat, GetChatDTO>();
    config.CreateMap<CreateChatDTO, Chat>();
    config.CreateMap<Message, MessageDTO>();
    config.CreateMap<MessageDTO, Message>();
});
#endif

builder.Services
    .AddHealthChecks()
    .AddCheck<MyCustomHealthCheckService>(nameof(MyCustomHealthCheckService))
    .AddNpgSql(services => {
        var configuration = services.GetRequiredService<IConfiguration>();
        return configuration.GetSection("ConnectionStrings:Postgres").Value;
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health_check");

app.Run();
