namespace ExpenseTracker.Api.Dtos;

public class ExpenseDto
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public DateTime Date { get; set; }
    }

public class CreateExpenseDto
{
    public string Title { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public DateTime Date { get; set; }
}

public class UpdateExpenseDto
{
    public string Title { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public DateTime Date { get; set; }
}