// Entity Framework Core library
using Microsoft.EntityFrameworkCore;

// Import the Expense model
using ExpenseTracker.Api.Models;

// Namespace for database-related classes
namespace ExpenseTracker.Api.Data;

// Database context class that manages database access
public class ExpenseDbContext : DbContext
{
    // Constructor that receives database configuration settings
    public ExpenseDbContext(DbContextOptions<ExpenseDbContext> options)
        : base(options)
    {
    }

    // Represents the Expenses table in the database
    public DbSet<Expense> Expenses { get; set; }
}