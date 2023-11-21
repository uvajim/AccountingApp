using System;
using System.Windows;
using System.Windows.Media.Imaging;
using JimboBooks.Controllers;
using JimboBooks.Models;
using Microsoft.Win32;


namespace JimboBooks.Views
{
    public partial class AddInvoicePage : Window
    {
        private InvoiceDatabaseController db;
        private Action onExpenseAdded;
        
        public AddInvoicePage()
        {
            InitializeComponent();
            db = new InvoiceDatabaseController();
            //onExpenseAdded = onExpenseAddedCallback;
        }
        
        
        private void OnAddInvoiceClicked(object sender, RoutedEventArgs e)
        {
            
            // Code to handle the add expense button click
            Invoice newInvoice = new Invoice
            {
                customerName = CustomerNameTextBox.Text,
                customerEmail = CustomerEmailTextBox.Text,
                invoiceName = InvoiceNameTextBox.Text,
                amount = Convert.ToDecimal(InvoiceAmountTextBox.Text),
            };
            
            db.finalizeInvoice(newInvoice);
            

            this.Close(); 

        }
        
        
        
        
    }
}