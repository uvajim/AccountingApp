using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JimboBooks.Models;

namespace JimboBooks.Controllers
{
    public class ExpenseDatabaseController
    {
        private readonly string connectionString;

        public ExpenseDatabaseController(string dbPath)     
        {
            connectionString = new SqliteConnectionStringBuilder
            {
                DataSource = dbPath
            }.ToString();
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText =
                    @"
                CREATE TABLE IF NOT EXISTS Expenses (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    Amount REAL NOT NULL,
                    Date TEXT NOT NULL,
                    FilePath TEXT
                );
                ";
                command.ExecuteNonQuery();
            }
        }

        public void AddExpense(Expense expense)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText =
                    @"
                INSERT INTO Expenses (Name, Amount, Date, FilePath)
                VALUES ($name, $amount, $date, $filePath);
                ";
                command.Parameters.AddWithValue("$name", expense.Name);
                command.Parameters.AddWithValue("$amount", expense.Amount);
                command.Parameters.AddWithValue("$date", expense.Date.ToString("o")); // ISO 8601 format
                command.Parameters.AddWithValue("$filePath",
                    (object)expense.FilePath ?? DBNull.Value); // Handle null file paths

                command.ExecuteNonQuery();
            }
        }

        public void AddFilePath(int expenseId, string filePath)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText =
                    @"
            UPDATE Expenses
            SET FilePath = $filePath
            WHERE Id = $id;
            ";
        
                if (filePath == null)
                {
                    command.Parameters.AddWithValue("$filePath", DBNull.Value);
                }
                else
                {
                    command.Parameters.AddWithValue("$filePath", filePath);
                }
                command.Parameters.AddWithValue("$id", expenseId);

                command.ExecuteNonQuery();
            }
        }


        public void deleteExpense(int expenseId)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText =
                    @"
            DELETE FROM Expenses
            WHERE Id = $id;
            ";

                // Assigning value to the parameter
                command.Parameters.AddWithValue("$id", expenseId);

                command.ExecuteNonQuery();
            }
        }


        public async Task<decimal> GetTotalAmountAsync()
        {
            decimal totalAmount = 0;

            using (var connection = new SqliteConnection(connectionString))
            {
                await connection.OpenAsync();

                var command = connection.CreateCommand();
                command.CommandText = "SELECT SUM(Amount) FROM Expenses;";

                var result = await command.ExecuteScalarAsync();

                if (result != DBNull.Value)
                {
                    totalAmount = Convert.ToDecimal(result);
                }
            }

            return totalAmount;
        }

        public List<Expense> GetAllExpenses()
        {
            var expenses = new List<Expense>();

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = "SELECT Id, Name, Amount, Date, FilePath FROM Expenses;";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var expense = new Expense
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Amount = reader.GetString(2),
                            Date = DateTime.Parse(reader.GetString(3)),
                            FilePath = !reader.IsDBNull(4) ? reader.GetString(4) : null
                        };
                        expenses.Add(expense);
                    }
                }
            }

            return expenses;
        }

        public void UpdateExpenseName(int expenseId, string newName)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText =
                    @"
        UPDATE Expenses
        SET Name = $name
        WHERE Id = $id;
        ";
                command.Parameters.AddWithValue("$name", newName);
                command.Parameters.AddWithValue("$id", expenseId);

                command.ExecuteNonQuery();
            }
        }

        public void UpdateExpenseAmount(int expenseId, decimal newAmount)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText =
                    @"
        UPDATE Expenses
        SET Amount = $amount
        WHERE Id = $id;
        ";
                command.Parameters.AddWithValue("$amount", newAmount);
                command.Parameters.AddWithValue("$id", expenseId);

                command.ExecuteNonQuery();
            }
        }

        public void UpdateExpenseDate(int expenseId, DateTime newDate)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText =
                    @"
        UPDATE Expenses
        SET Date = $date
        WHERE Id = $id;
        ";
                command.Parameters.AddWithValue("$date", newDate.ToString("o")); // ISO 8601 format
                command.Parameters.AddWithValue("$id", expenseId);

                command.ExecuteNonQuery();
            }
        }
    }
}