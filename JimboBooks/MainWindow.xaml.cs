using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using JimboBooks.Views;
using JimboBooks.Controllers;
using JimboBooks.Models;

namespace JimboBooks
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Sidebar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = (ListViewItem)((ListView)sender).SelectedItem;
            switch (selectedItem.Content.ToString())
            {
                case "Expenses":
                    MainContent.Content = new ExpensesOverviewWindow();
                    break;
                case "Invoices":
                    MainContent.Content = new InvoicesOverviewWindow();
                    break;
                // Add cases for additional views
            }
        }
    }
}