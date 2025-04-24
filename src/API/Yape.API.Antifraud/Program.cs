using Microsoft.EntityFrameworkCore;
using Yape.Antifraud.API.Configurations;
using Yape.Infrastructure.Postgresql.Database;

var builder = WebApplication.CreateBuilder(args);

// Add Database context
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));
// Add services to the container.
MediatRConfiguration.ConfigureMediatRService(builder.Services);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapControllers();
app.UseHttpsRedirection();

await app.RunAsync();
