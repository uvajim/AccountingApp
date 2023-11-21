using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using JimboBooks.Views;
using JimboBooks.Controllers;
using JimboBooks.Models;

namespace JimboBooks.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ExpensesOverviewWindow : UserControl
    {
        private ExpenseDatabaseController db;
        private decimal totalExpenses; // Changed from int to decimal

        public ExpensesOverviewWindow()
        {
            InitializeComponent(); // Always call this first
            db = new ExpenseDatabaseController(Constants.DatabaseFilename);
            Loaded += MainWindow_Loaded; // Subscribe to the Loaded event
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ExpensesListView.ItemsSource = db.GetAllExpenses();
            totalExpenses = await db.GetTotalAmountAsync(); // Asynchronously get the total amount
            // Update your UI with totalExpenses here if needed
            TotalExpensesTextBlock.Text = $"Total: {totalExpenses:C}";
        }
        
        private void ExpensesListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Assuming YourDataType is the class name of the bound items in the ListView
            var item = ((FrameworkElement)e.OriginalSource).DataContext as Expense;
            if (item != null)
            {
                
                var popup = new ExpenseCard(item, db, () => RefreshExpensesList());
    
                // If the data context is set for the MainWindow and should be used for the popup as well
                
                popup.ShowDialog(); 
            }
        }
        private async void RefreshTotalExpenses()
        {
            totalExpenses = await db.GetTotalAmountAsync();
            TotalExpensesTextBlock.Text = $"Total: {totalExpenses:C}";
        }

        private void AddExpensePage_Closed(object sender, EventArgs e)
        {
            // Refresh the list of expenses and the total expenses amount after the AddExpense window is closed
            RefreshExpensesList();
            RefreshTotalExpenses();
        }

        private void RefreshExpensesList()
        {
            ExpensesListView.ItemsSource = db.GetAllExpenses(); // Fetch the updated list
            RefreshTotalExpenses(); // Refresh the total expenses
        }

        private void deleteEntryOnClick(object sender, RoutedEventArgs e)
        {
            var item = ((FrameworkElement)e.OriginalSource).DataContext as Expense;
            if (item != null)
            {
                
                db.deleteExpense(item.Id);
            }
            RefreshExpensesList();
        }


        private void PlusButton_Click(object sender, RoutedEventArgs e)
        {
            // Code to execute when the plus button is clicked, such as opening a new window
            AddExpensePage addExpenseWindow = new AddExpensePage(() => RefreshExpensesList());

            // Subscribe to the Closed event of the addExpenseWindow
            addExpenseWindow.Closed += AddExpensePage_Closed;

            // Show the window as a modal dialog box.
            // This will block input to other windows until this one is closed.
            addExpenseWindow.ShowDialog();
        }

    }
}
