using System.ComponentModel.DataAnnotations;

namespace ExpenseManager.Models;

public class Expences
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public decimal Amount { get; set; }

    [Required]
    public DateTime Date { get; set; }

    [Required]
    public string Category { get; set; }

    [StringLength(500)]
    public string Description { get; set; }
}