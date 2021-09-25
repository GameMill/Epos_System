using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Epos.Pages.Windows
{
    /// <summary>
    /// Interaction logic for Inventory.xaml
    /// </summary>
    public partial class Inventory : Window
    {
        Dashboard Page;
        public Inventory(Dashboard Page)
        {
            this.Page = Page;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Page.CurrentSale.Cart.Add(new Models.ProductForCart() { ProductName = "From Inventory" });
            Close();
        }
    }
}
