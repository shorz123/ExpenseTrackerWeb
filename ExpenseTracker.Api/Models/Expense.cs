using System.ComponentModel.DataAnnotations;
namespace ExpenseTracker.Api.Models;

public class Expense
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    [Range(.01, 1000000)]
    public decimal Amount { get; set; }

    public DateTime Date { get; set; }
}