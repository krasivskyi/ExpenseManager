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
        public async Task<ActionResult<IEnumerable<Expense>>> GetExpenses()
        {
            try
            {
                var expenses = await _context.Expenses
                    .Include(e => e.Category)
                    .ToListAsync();

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