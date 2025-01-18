using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseManager.Models;

public class Expences
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Title is required")]
    [StringLength(100, ErrorMessage = "Title must be less than 100 characters")]
    [Display(Name = "Title")]
    public string Title { get; set; }

    [Required(ErrorMessage = "Amount is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
    [Column(TypeName = "decimal(18, 2)")]
    [Display(Name = "Amount")]
    public decimal Amount { get; set; }

    [Required(ErrorMessage = "Date is required")]
    [Display(Name = "Date")]
    public DateTime Date { get; set; }

    [StringLength(500, ErrorMessage = "Description must be less than 500 characters")]
    [Display(Name = "Description")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Category is required")]
    [Display(Name = "Category")]
    public int CategoryId { get; set; }
    public Category Category { get; set; }
}