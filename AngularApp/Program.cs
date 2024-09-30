using AngularApp.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Cors; // Add this using directive
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure CORS to allow all origins
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

// Configure MongoDB
var mongoConnectionString = builder.Configuration.GetConnectionString("mongoConnectionString");
builder.Services.AddSingleton<MongoDbContext>(serviceProvider =>
{
    return new MongoDbContext(mongoConnectionString, "UserDB");
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

// Use CORS
app.UseCors("AllowAllOrigins"); // Apply CORS policy

app.UseAuthorization();

app.MapControllers();

app.Run();
