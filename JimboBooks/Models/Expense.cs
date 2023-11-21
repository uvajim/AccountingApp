using System;

namespace JimboBooks.Models
{
    public class Expense
    {
        public int Id { get; set; } // Assuming you have an Id property
        public string Name { get; set; }
        public string Amount { get; set; } // Changed from string to decimal for proper amount handling
        public DateTime Date { get; set; }
        public string FilePath { get; set; } // New property to store the file path of the receipt

        // Add additional properties and logic as needed for your application
    }
}