using System.ComponentModel.DataAnnotations;

namespace ExpenseManager.DTOs
{
    public class UpdateExpenseDto
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Amount is required")]
        [Range(0.01, 1000000, ErrorMessage = "Amount must be between 0.01 and 1,000,000")]
        public decimal Amount { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Category name is required")]
        public string CategoryName { get; set; } = string.Empty;
    }
} 