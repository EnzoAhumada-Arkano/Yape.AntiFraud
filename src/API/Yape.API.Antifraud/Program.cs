using Yape.API.AntiFraud.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Add Database context
DatabaseConfiguration.AddDatabaseConfiguration(builder);
// Add services to the container.
ServiceConfiguration.AddServiceConfiguration(builder.Services);
MediatRConfiguration.AddMediatRService(builder.Services);
KafkaConfiguration.AddKafkaConfiguration(builder.Services);
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

await app.RunAsync();
