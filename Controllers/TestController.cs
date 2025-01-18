using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExpenseManager.Data;

namespace ExpenseManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TestController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> TestConnection()
        {
            try
            {
                var categories = await _context.Categories.ToListAsync();
                return Ok(new 
                { 
                    message = "Connection successful!", 
                    categoriesCount = categories.Count,
                    categoriesList = categories
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new 
                { 
                    message = "Connection failed!", 
                    error = ex.Message 
                });
            }
        }
    }
} 