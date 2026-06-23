using ExpenseTracker.Api.Data;
using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Api.Endpoints;

//create app config object
var builder = WebApplication.CreateBuilder(args);

//Gather API endpoint information for Swagger 
builder.Services.AddEndpointsApiExplorer();

//Add Swagger services
builder.Services.AddSwaggerGen();

// Register database context with SQL Server
builder.Services.AddDbContext<ExpenseDbContext>(options =>
    options.UseSqlServer(
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

