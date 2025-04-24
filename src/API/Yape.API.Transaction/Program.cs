using Domain.Repository;
using Microsoft.EntityFrameworkCore;
using Yape.Infrastructure.Postgresql.Database;
using Yape.Infrastructure.Postgresql.Repository;
using Yape.Transaction.API.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Add Database context
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));
// Add services to the container.
builder.Services.AddTransient<ITransactionRepository, TransactionRepository>();
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

app.UseHttpsRedirection();

await app.RunAsync();