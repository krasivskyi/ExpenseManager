using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseManager.Models
{
    [Table("expenses")]
    public class Expense
    {
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("title")]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [Column("amount")]
        public decimal Amount { get; set; }

        [Column("date")]
        public DateTime Date { get; set; } = DateTime.UtcNow;

        [Column("description")]
        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        [Column("category_id")]
        public int CategoryId { get; set; }

        public Category? Category { get; set; }
    }
}