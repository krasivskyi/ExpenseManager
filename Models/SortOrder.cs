namespace ExpenseManager.Models
{
    using System.ComponentModel.DataAnnotations;

    public enum SortOrder
    {
        [Display(Name = "Date (Oldest First)")]
        DateAsc = 0,

        [Display(Name = "Date (Newest First)")]
        DateDesc = 1,

        [Display(Name = "Amount (Low to High)")]
        AmountAsc = 2,

        [Display(Name = "Amount (High to Low)")]
        AmountDesc = 3,

        [Display(Name = "Title (A to Z)")]
        TitleAsc = 4,

        [Display(Name = "Title (Z to A)")]
        TitleDesc = 5
    }
}