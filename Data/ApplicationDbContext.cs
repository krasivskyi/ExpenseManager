using Microsoft.EntityFrameworkCore;
using ExpenseManager.Models;

namespace ExpenseManager.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Expense> Expenses { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().ToTable("categories");
            modelBuilder.Entity<Expense>().ToTable("expenses");
        }
    }
}
