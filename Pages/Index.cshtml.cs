using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using MyFirstWebApp.Models;
using System.IO;

namespace MyFirstWebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        [BindProperty]
        public Transaction NewTransaction { get; set; } = new Transaction();

        public List<Transaction> Transactions { get; set; } = new List<Transaction>();
        public Dictionary<Category, decimal> CategorySums { get; set; } = new();
        public decimal TotalIncome { get; set; }
        public decimal TotalExpenses { get; set; }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            LoadTransactionsFromSession();
            RecalculateSums();
        }

        public IActionResult OnPostAdd()
        {
            LoadTransactionsFromSession();

            if (NewTransaction != null)
            {
                NewTransaction.Date = DateTime.Now;
                Transactions.Add(NewTransaction);
                SaveTransactionsToSession();
            }

            return RedirectToPage(); // PRG pattern to prevent form resubmission
        }

        public IActionResult OnPostExportCsv()
        {
            LoadTransactionsFromSession();

            var csv = new StringBuilder();
            csv.AppendLine("Description,Amount,Category,Date");

            foreach (var transaction in Transactions)
            {
                csv.AppendLine($"{transaction.Description},{transaction.Amount},{transaction.Category},{transaction.Date.ToShortDateString()}");
            }

            var bytes = Encoding.UTF8.GetBytes(csv.ToString());
            return File(new MemoryStream(bytes), "text/csv", "transactions.csv");
        }

        private void LoadTransactionsFromSession()
        {
            var json = HttpContext.Session.GetString("Transactions");
            if (!string.IsNullOrEmpty(json))
            {
                Transactions = JsonSerializer.Deserialize<List<Transaction>>(json) ?? new List<Transaction>();
            }
            else
            {
                Transactions = new List<Transaction>();
            }
        }

        private void SaveTransactionsToSession()
        {
            var json = JsonSerializer.Serialize(Transactions);
            HttpContext.Session.SetString("Transactions", json);
        }

        private void RecalculateSums()
        {
            TotalIncome = Transactions.Where(t => t.Amount > 0).Sum(t => t.Amount);
            TotalExpenses = Transactions.Where(t => t.Amount < 0).Sum(t => t.Amount);

            CategorySums = Enum.GetValues(typeof(Category))
                .Cast<Category>()
                .ToDictionary(cat => cat, cat => Transactions
                    .Where(t => t.Category == cat)
                    .Sum(t => t.Amount));
        }
    }
}





















