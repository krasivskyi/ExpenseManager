using System.ComponentModel.DataAnnotations;

namespace ExpenseManager.DTOs
{
    public class ExpenseFilterDto
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public int? CategoryId { get; set; }

        public string? SearchTerm { get; set; }

        public decimal? MinAmount { get; set; }
        public decimal? MaxAmount { get; set; }

        public SortOrder? SortBy { get; set; }
    }
} 