using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SpendSmart.Data;
using SpendSmart.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace SpendSmart.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly SpendSmartDbContext _context;

        public HomeController(ILogger<HomeController> logger, SpendSmartDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Expenses()
        {
            var allExpenses = await _context.Expenses.ToListAsync();

            var totalExpenses = allExpenses.Sum(x => x.Value);

            ViewBag.Expenses = totalExpenses;

            return View(allExpenses);
        }

        public async Task<IActionResult> CreateEditExpense(int? id)
        {
            if (id != null)
            {
                // editing -> load an expense by id
                var expenseInDb = await _context.Expenses.SingleOrDefaultAsync(expense => expense.Id == id);
                return View(expenseInDb);
            }
            return View();
        }

        public async Task<IActionResult> DeleteExpense(int id)
        {
            var expenseInDb = await _context.Expenses.SingleOrDefaultAsync(expense => expense.Id == id);
            _context.Expenses.Remove(expenseInDb);
            await _context.SaveChangesAsync();
            return RedirectToAction("Expenses");
        }

        public async Task<IActionResult> CreateEditExpenseForm(Expense model)
        {
            if(model.Id == 0)
            {
                // Create
                _context.Expenses.Add(model);
            }
            else
            {
                // Editing
                _context.Expenses.Update(model);
            }
                await _context.SaveChangesAsync();

            return RedirectToAction("Expenses");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
