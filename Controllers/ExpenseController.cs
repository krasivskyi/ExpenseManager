using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExpenseManager.Data;
using ExpenseManager.Models;
using ExpenseManager.DTOs;

namespace ExpenseManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExpenseController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ExpenseController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Expense>>> GetExpenses([FromQuery] ExpenseFilterDto filter)
        {
            try
            {
                var query = _context.Expenses
                    .Include(e => e.Category)
                    .AsQueryable();

                if (filter.StartDate.HasValue)
                    query = query.Where(e => e.Date >= filter.StartDate.Value);
                    
                if (filter.EndDate.HasValue)
                    query = query.Where(e => e.Date <= filter.EndDate.Value);

                if (filter.CategoryId.HasValue)
                    query = query.Where(e => e.CategoryId == filter.CategoryId.Value);

                if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
                {
                    var searchTerm = filter.SearchTerm.ToLower();
                    query = query.Where(e => 
                        e.Title.ToLower().Contains(searchTerm) || 
                        (e.Description != null && e.Description.ToLower().Contains(searchTerm))
                    );
                }

                if (filter.MinAmount.HasValue)
                    query = query.Where(e => e.Amount >= filter.MinAmount.Value);
                    
                if (filter.MaxAmount.HasValue)
                    query = query.Where(e => e.Amount <= filter.MaxAmount.Value);

                query = filter.SortBy switch
                {
                    SortOrder.DateAsc => query.OrderBy(e => e.Date),
                    SortOrder.DateDesc => query.OrderByDescending(e => e.Date),
                    SortOrder.AmountAsc => query.OrderBy(e => e.Amount),
                    SortOrder.AmountDesc => query.OrderByDescending(e => e.Amount),
                    SortOrder.TitleAsc => query.OrderBy(e => e.Title),
                    SortOrder.TitleDesc => query.OrderByDescending(e => e.Title),
                    _ => query.OrderByDescending(e => e.Date)  // По умолчанию - новые первыми
                };

                var expenses = await query.ToListAsync();
                return Ok(expenses);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<Expense>> CreateExpense(Expense expense)
        {
            try
            {
                var category = await _context.Categories.FindAsync(expense.CategoryId);
                if (category == null)
                {
                    return BadRequest(new { message = "Category not found" });
                }

                expense.Date = DateTime.UtcNow;

                _context.Expenses.Add(expense);
                
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    var innerMessage = ex.InnerException?.Message ?? ex.Message;
                    return BadRequest(new { message = innerMessage });
                }

                return CreatedAtAction(
                    nameof(GetExpenses),
                    new { id = expense.Id },
                    expense
                );
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Expense>> GetExpense(int id)
        {
            try
            {
                var expense = await _context.Expenses
                    .Include(e => e.Category)
                    .FirstOrDefaultAsync(e => e.Id == id);

                if (expense == null)
                {
                    return NotFound(new { message = $"Expense with ID {id} not found" });
                }

                return Ok(expense);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpense(int id)
        {
            try
            {
                var expense = await _context.Expenses.FindAsync(id);
                
                if (expense == null)
                {
                    return NotFound(new { message = $"Expense with ID {id} not found" });
                }

                _context.Expenses.Remove(expense);
                await _context.SaveChangesAsync();

                return Ok(new { message = $"Expense with ID {id} was deleted" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExpense(int id, UpdateExpenseDto dto)
        {
            try
            {
                var expense = await _context.Expenses.FindAsync(id);
                if (expense == null)
                {
                    return NotFound(new { message = $"Expense with ID {id} not found" });
                }

                var category = await _context.Categories
                    .FirstOrDefaultAsync(c => c.Name.ToLower() == dto.CategoryName.ToLower());
                if (category == null)
                {
                    return BadRequest(new { message = $"Category '{dto.CategoryName}' not found" });
                }

                expense.Title = dto.Title;
                expense.Amount = dto.Amount;
                expense.Description = dto.Description;
                expense.CategoryId = category.Id;

                await _context.SaveChangesAsync();

                return Ok(expense);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}   