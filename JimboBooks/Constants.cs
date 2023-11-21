namespace JimboBooks
{
    public static class Constants
    {
        // Database related constants
        public const string DatabaseFilename = "expenses.db";
        
        // Connection string for SQLite database
        // For simplicity we're assuming the database file is located in the same directory as the executable
        public static readonly string DbConnectionString = $"Data Source={DatabaseFilename};Cache=Shared";

        // Other constants for the application
        public const decimal TaxRate = 0.08m;
        // Add other constants as needed
    }
}