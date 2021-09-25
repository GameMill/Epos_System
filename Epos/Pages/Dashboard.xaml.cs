using Epos.Models;
using Epos.Pages.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Epos.Pages
{
    public class Sale
    {
        public int InvoiceNumber { get; set; } = -1;
        public ObservableCollection<Models.ProductForCart> Cart { get; set; } = new ObservableCollection<Models.ProductForCart>();
        public bool HasCheckoutComplate { get; set; } = false;
        public DateTime DateSold { get; set; }
        public Orders Save()
        {
            Orders Data = null;
            var data = new Models.Orders()
            {

                Cart = new List<Models.ProductForCart>(Cart),
                HasCheckoutComplate = true,
                LastUpdated = DateTime.Now,
                Order = DateTime.Now,
            };
            if (InvoiceNumber != -1)
            {
                data.Id = InvoiceNumber;
                Data = DBContext.CoreDBContext.Instance.Orders.Update(data).Entity;

            }
            else
            {
                Data = DBContext.CoreDBContext.Instance.Orders.Add(data).Entity;
            }
            DBContext.CoreDBContext.Instance.SaveChanges();
            return Data;
        }
        public static Sale Load(int InvoiceNumber) 
        {
            var data2 = DBContext.CoreDBContext.Instance.ProductForCart.ToArray();

            var data = DBContext.CoreDBContext.Instance.Orders.ToArray();
            return DBContext.CoreDBContext.Instance.Orders.Where(a => a.Id == InvoiceNumber).FirstOrDefault()?.toSale();
        }
    }

    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Page
    {
        public Sale CurrentSale = null;

        public Dashboard()
        {

            InitializeComponent();
            
            UpdateCurrentSale(new Sale());
        }

        private void Cart_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            double totalPrice = 0;
            foreach (var item in CurrentSale.Cart)
            {
                totalPrice += item.TotalPrice;
            }
            TotalPrice.Text = "£" + totalPrice;
        }

        private void Orders_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Rapid_Invertory_Edit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MixMatch_Click(object sender, RoutedEventArgs e)
        {

        }

        private void WarrantyEditor_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BackToDashboard_Click(object sender, RoutedEventArgs e)
        {

        }

        private void About_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Discount_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Pay_Click(object sender, RoutedEventArgs e)
        {
            var Order = CurrentSale.Save();
            if(MessageBox.Show("Do you want to print a receipt?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                (new Models.ReceiptPrinter.Sale(Order)).Print();
            }
            UpdateCurrentSale(new Sale());
        }

        public void UpdateCurrentSale(Sale NewCurrentSale)
        {
            if (CurrentSale != null)
            {
                foreach (var item in CurrentSale.Cart)
                {
                    item.PropertyChanged -= Product_PropertyChanged;
                }
                CurrentSale.Cart.CollectionChanged -= Cart_CollectionChanged;
            }
            CurrentSale = NewCurrentSale;

            foreach (var item in CurrentSale.Cart)
            {
                item.PropertyChanged += Product_PropertyChanged;
            }
            DataViewer.ItemsSource = CurrentSale.Cart;
            CurrentSale.Cart.CollectionChanged += Cart_CollectionChanged;
            Cart_CollectionChanged(null,null);
            if (CurrentSale.InvoiceNumber != -1)
            {
                PayButton.Visibility = Visibility.Collapsed;
                PayButton.IsEnabled = false;

                SaleDate.Text = CurrentSale.DateSold.ToString();
                SaleDate.Visibility = Visibility.Visible;

                CurrentInvoice.Text = "Invoice: " + CurrentSale.InvoiceNumber;
                CurrentInvoice.Visibility = Visibility.Visible;
            }
            else
            {
                PayButton.Visibility = Visibility.Visible;

                PayButton.IsEnabled = true;
                CurrentInvoice.Text = "";
                CurrentInvoice.Visibility = Visibility.Collapsed;

                SaleDate.Text = "";
                SaleDate.Visibility = Visibility.Collapsed;
            }
        }

        private void Button_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void EmptyCart_Click(object sender, RoutedEventArgs e)
        {
            UpdateCurrentSale(new Sale());
        }

        private void lvInvertory_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void OpenInventory_Click(object sender, RoutedEventArgs e)
        {
            Inventory Window = new Inventory(this);
            Window.Show();
        }

        private void AddNewItem_KeyDown(object sender, KeyEventArgs e)
        {
            
            if(e.Key == Key.F1)
            {
                if (int.TryParse(((TextBox)sender).Text, out int index))
                {
                    var sale = Sale.Load(index);
                    if(sale != null)
                    {
                        UpdateCurrentSale(sale);
                    }
                    else
                    {
                        MessageBox.Show("Invoice Numbers was not valid.");
                    }
                    
                }
                else
                {
                    MessageBox.Show("Invoice Numbers Are only Numbers.");
                }
            }
            if (e.Key == Key.Enter)
            {
                var Product = new Models.ProductForCart() { ProductName = ((TextBox)sender).Text, Price = 15.00, Id = 0, SnNeed = true, QTY = 1 };
                CurrentSale.Cart.Add(Product);
                Product.PropertyChanged += Product_PropertyChanged;
                ((TextBox)sender).Text = "";
            }
        }

        private void Product_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Cart_CollectionChanged(null, null);
        }

        private void Reprint_Click(object sender, RoutedEventArgs e)
        {
            var Order = DBContext.CoreDBContext.Instance.Orders.Where(a => a.Id == CurrentSale.InvoiceNumber).FirstOrDefault();
            if (Order != null)
            {
                (new Models.ReceiptPrinter.Sale(Order)).Print();
            }
            EmptyCart_Click(null, null);
        }
    }
}
