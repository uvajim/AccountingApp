using System;
using System.Windows;
using System.Windows.Media.Imaging;
using JimboBooks.Controllers;
using JimboBooks.Models;
using Microsoft.Win32;

namespace JimboBooks.Views
{
    public partial class ExpenseCard : Window
    {
        public Expense Expense { get; private set; }
        private ExpenseDatabaseController db { get; set; }
        
        private Action onExpenseChanged;

        private string filePath;

        public ExpenseCard(Expense expense, ExpenseDatabaseController _db, Action onExpenseChangedCallback)
        {
            InitializeComponent();
            Expense = expense;
            db = _db;
            DisplayExpense();
            onExpenseChanged = onExpenseChangedCallback;
        }

        private void DisplayExpense()
        {
            NameTextBox.Text = Expense.Name;
            AmountTextBox.Text = Expense.Amount;
            DateTextBox.Text = Expense.Date.ToShortDateString();

            if (Expense.FilePath != null)
            {
                if (Expense.FilePath.ToLower().EndsWith(".png") || Expense.FilePath.ToLower().EndsWith(".jpg") || Expense.FilePath.ToLower().EndsWith(".jpeg"))
                {
                    // Create a new BitmapImage and set it as the source for the Image control
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(Expense.FilePath);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;  // To make sure the file is not locked
                    bitmap.EndInit();
                    ReceiptImage.Source = bitmap;
                }
            }
        }
        
        private void AddReceipt(object sender, RoutedEventArgs e)
        {
            // Create an instance of the OpenFileDialog class
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                // Set filter for file extension and default file extension
                Filter = "Receipt Files (*.pdf;*.png;*.jpg;*.jpeg)|*.pdf;*.png;*.jpg;*.jpeg",
                FilterIndex = 1,
                RestoreDirectory = true
            };
            
            if (openFileDialog.ShowDialog() == true)
            {
                filePath = openFileDialog.FileName;
                db.AddFilePath(Expense.Id, filePath);
                
                if (filePath.ToLower().EndsWith(".png") || filePath.ToLower().EndsWith(".jpg") || filePath.ToLower().EndsWith(".jpeg"))
                {
                    // Create a new BitmapImage and set it as the source for the Image control
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(filePath);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;  // To make sure the file is not locked
                    bitmap.EndInit();
                    ReceiptImage.Source = bitmap;
                }
                
                onExpenseChanged?.Invoke();
                
            }
        }

        private void SaveExpense_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve input values from the text boxes
            string expenseName = NameTextBox.Text;
            decimal amount = Convert.ToDecimal(AmountTextBox.Text); // This will be a string, you may want to convert it to a numeric type
            DateTime date = Convert.ToDateTime(DateTextBox.Text); // This will also be a string, consider parsing to DateTime if needed

            db.UpdateExpenseAmount(Expense.Id, amount);
            db.UpdateExpenseName(Expense.Id, expenseName);
            db.UpdateExpenseDate(Expense.Id, date);
            db.AddFilePath(Expense.Id, filePath);
            onExpenseChanged?.Invoke();

            MessageBox.Show($"Expense saved:\nName: {expenseName}\nAmount: {amount}\nDate: {date}", "Expense Saved", MessageBoxButton.OK, MessageBoxImage.Information);
        }
       
    }
}