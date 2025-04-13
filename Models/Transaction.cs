using System;

namespace MyFirstWebApp.Models
{
    public class Transaction
    {
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public Category Category { get; set; }
        public DateTime Date { get; set; }
    }
}




