using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Stripe;
using Invoice = JimboBooks.Models.Invoice; // Ensure this is the correct namespace for your Invoice model

namespace JimboBooks.Controllers
{
    public class InvoiceDatabaseController
    {
        private readonly string connectionString;
        private readonly StripeController _stripeController;

        public InvoiceDatabaseController()
        {
            connectionString = new SqliteConnectionStringBuilder
            {
                DataSource = "invoice.db"
            }.ToString();
            InitializeDatabase();

            _stripeController = new StripeController();
        }

        private void InitializeDatabase()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText =
                    @"
                    CREATE TABLE IF NOT EXISTS Invoices (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        CustomerName TEXT NOT NULL,
                        CustomerEmail TEXT NOT NULL,
                        ShortDesc TEXT,
                        DueDate TEXT NOT NULL,
                        InvoiceName TEXT NOT NULL,
                        Amount REAL NOT NULL,
                        PaymentLink REAL NOT NULL      
                    );
                    ";
                command.ExecuteNonQuery();
            }
        }

        public void AddInvoice(Invoice invoice, string paymentLink)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText =
                    @"
            INSERT INTO Invoices (CustomerName, CustomerEmail, ShortDesc, DueDate, InvoiceName, Amount, PaymentLink)
            VALUES ($customerName, $customerEmail, $shortDesc, $dueDate, $invoiceName, $amount, $paymentLink);
            ";
                command.Parameters.AddWithValue("$customerName", invoice.customerName);
                command.Parameters.AddWithValue("$customerEmail", invoice.customerEmail);
                command.Parameters.AddWithValue("$shortDesc", invoice.shortDesc ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("$dueDate", invoice.dueDate.ToString("o")); // ISO 8601 format
                command.Parameters.AddWithValue("$invoiceName", invoice.invoiceName);
                command.Parameters.AddWithValue("$amount", invoice.amount);
                command.Parameters.AddWithValue("$paymentLink", paymentLink); // Ensure the parameter name matches with the SQL command

                command.ExecuteNonQuery();
            }
        }


        public void DeleteInvoice(int invoiceId)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText =
                    @"
                    DELETE FROM Invoices
                    WHERE Id = $id;
                    ";
                command.Parameters.AddWithValue("$id", invoiceId);

                command.ExecuteNonQuery();
            }
        }

        public List<Invoice> GetAllInvoices()
        {
            var invoices = new List<Invoice>();

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
            
                var command = connection.CreateCommand();
                command.CommandText = "SELECT Id, CustomerName, CustomerEmail, ShortDesc, DueDate, InvoiceName, Amount, PaymentLink FROM Invoices;";
            
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var invoice = new Invoice
                        {
                            Id = reader.GetInt32(0),
                            customerName = reader.GetString(1),
                            customerEmail = reader.GetString(2),
                            shortDesc = !reader.IsDBNull(3) ? reader.GetString(3) : null,
                            dueDate = DateTime.Parse(reader.GetString(4)),
                            invoiceName = reader.GetString(5),
                            amount = reader.GetDecimal(6),
                            paymentLink = reader.GetString(7)
                        };
                        invoices.Add(invoice);
                    }
                }
            }

            return invoices;
        }
        
        public void finalizeInvoice(Invoice currInvoice)
        {
            string paymentLink = _stripeController.finalizePaymentLink(currInvoice);
            Console.WriteLine(paymentLink);
            AddInvoice(currInvoice, paymentLink);
            
        }

        // Additional methods for updating specific fields of an invoice can be implemented here.
    }
}
