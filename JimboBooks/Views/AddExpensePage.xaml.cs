using System;
using System.Windows;
using System.Windows.Media.Imaging;
using JimboBooks.Controllers;
using JimboBooks.Models;
using Microsoft.Win32;


namespace JimboBooks.Views
{
    public partial class AddExpensePage : Window
    {
        private ExpenseDatabaseController db;
        private Action onExpenseAdded;
        
        public AddExpensePage(Action onExpenseAddedCallback)
        {
            InitializeComponent();
            db = new ExpenseDatabaseController(Constants.DatabaseFilename);
            onExpenseAdded = onExpenseAddedCallback;
        }

        private void OnUploadReceiptClicked(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "PDF files (*.pdf)|*.pdf|Image files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg",
                FilterIndex = 2,
                RestoreDirectory = true
            };

            if (openFileDialog.ShowDialog() == true)
            {
                // Get the selected file path
                string filePath = openFileDialog.FileName;
        
                // Update the TextBlock with the file path
                FilePathTextBlock.Text = filePath;

                // Check if the file is an image before trying to display it
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
                // Here you can handle displaying the PDF by other means if necessary
                // For example, you might display a message saying that PDF preview is not available
            }
            else
            {
                // Update the TextBlock to indicate no file was selected
                FilePathTextBlock.Text = "No file selected";
                ReceiptImage.Source = null;  // Clear the current image
            }
        }
        
        private void OnAddExpenseClicked(object sender, RoutedEventArgs e)
        {
            
            // Code to handle the add expense button click
            Expense newExpense = new Expense
            {
                Name = NameTextBox.Text,
                Date = DateDatePicker.SelectedDate.GetValueOrDefault(DateTime.Now),
                Amount = PriceTextBox.Text
            };
            
            db.AddExpense(newExpense);
            onExpenseAdded?.Invoke(); // Call the callback after adding the expense
            this.Close(); 

        }
        
        
        
        
    }
}