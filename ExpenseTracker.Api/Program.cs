using ExpenseTracker.Api.Data;
using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Api.Endpoints;
using ExpenseTracker.Api.Services;

//create app config object
var builder = WebApplication.CreateBuilder(args);

//Gather API endpoint information for Swagger 
builder.Services.AddEndpointsApiExplorer();

//Add Swagger services
builder.Services.AddSwaggerGen();

// Register ExpenseDbContext and configure it to use the PostgreSQL Server connection string from appsettings.json
builder.Services.AddDbContext<ExpenseDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("ExpenseDatabase")));

//allow cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddScoped<ExpenseService>();

//build application
var app = builder.Build();

//allow cors
app.UseCors("AllowReactApp");

//Enable Swagger in development environment
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Redirect HTTP to HTTPS
app.UseHttpsRedirection();

app.MapExpenseEndpoints();

//Start app
app.Run();

