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
    public partial class InvoicesOverviewWindow : UserControl
    {
        private InvoiceDatabaseController db;
        private StripeController _stripeController;
        private decimal totalInvoices; // Changed from int to decimal

        public InvoicesOverviewWindow()
        {
            InitializeComponent(); // Always call this first
            db = new InvoiceDatabaseController();
            _stripeController = new StripeController();
            Loaded += MainWindow_Loaded; // Subscribe to the Loaded event
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //InvoicesListView.ItemsSource = db.GetAllInvoices();
            //totalInvoices = await db.GetTotalAmountAsync(); // Asynchronously get the total amount
            // Update your UI with totalInvoices here if needed
        }
        
        private void InvoicesListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Assuming YourDataType is the class name of the bound items in the ListView

        }

        private void AddInvoicePage_Closed(object sender, EventArgs e)
        {
            // Refresh the list of Invoices and the total Invoices amount after the AddExpense window is closed
            RefreshInvoicesList();
        }

        private void RefreshInvoicesList()
        {
            InvoicesListView.ItemsSource = db.GetAllInvoices(); 
        }

        private void deleteEntryOnClick(object sender, RoutedEventArgs e)
        {
            var item = ((FrameworkElement)e.OriginalSource).DataContext as Invoice;
            if (item != null)
            {
                
                db.DeleteInvoice(item.Id);
            }
            RefreshInvoicesList();
        }

        
        private void PlusButton_Click(object sender, RoutedEventArgs e)
        {
            // Code to execute when the plus button is clicked, such as opening a new window
            AddInvoicePage addInvoiceWindow = new AddInvoicePage();

            // Subscribe to the Closed event of the addExpenseWindow
            addInvoiceWindow.Closed += AddInvoicePage_Closed;

            // Show the window as a modal dialog box.
            // This will block input to other windows until this one is closed.
            addInvoiceWindow.ShowDialog();
        }

    }
}
