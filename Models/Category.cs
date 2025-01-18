using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseManager.Models 
{
    [Table("categories")]
    public class Category
    {
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("name")]
        [StringLength(50)]
        public string Name { get; set; }

        [Column("description")]
        [StringLength(200)]
        public string? Description { get; set; }

        public List<Expense> Expenses { get; set; } = new List<Expense>();
    }
}