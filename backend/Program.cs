#define DEEP_DEBUG

using AutoMapper;
using backend;
using backend.DTO.UserControllerDTO;
using backend.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

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
    // config.CreateMap<Chat, ChatDTO>();
    // config.CreateMap<ChatDTO, Chat>();
    // config.CreateMap<Message, MessageDTO>();
    // config.CreateMap<MessageDTO, Message>();
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

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health_check");

app.Run();
