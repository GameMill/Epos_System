using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EPOS.Pages.Invertory
{
    /// <summary>
    /// Interaction logic for DetailOrder.xaml
    /// </summary>
    public partial class DetailOrder : Page
    {
        public DB.Models.Order order;
        public List<DB.Models.OrderedProduct> Products;
        public List<DB.Models.OrderedAttribute> Attributes;
        public List<ProductOrderdViewModel> ProductOrderdViewModel = new List<Invertory.ProductOrderdViewModel>();
        public DetailOrder()
        {
            InitializeComponent();
        }

        public void Setup(int ID, Page OrdersPage)
        {
            order = DB.DBContext.Instance.Orders.Where(a => a.ID == ID).FirstOrDefault();
            if(order == null)
            {
                Windows.Invertory.Redirect(OrdersPage);
            }
            else
            {
                InvoiceNumber.Text = order.InvoiceNumber.ToString();
                Status.Text = (order.TotalPrice == 0 && order.TotalCost == 0)?"Void":"Active";
                TotalPaid.Text = "£" + order.TotalPrice;
                Products = DB.DBContext.Instance.OrderedProducts.Where(a => a.OrderID == ID).ToList();
                Attributes = DB.DBContext.Instance.OrderedAttributes.Where(a => a.OrderID == ID).ToList();

                foreach (var item in Products)
                {
                    var View = new ProductOrderdViewModel()
                    {
                        QTY = item.QTY,
                        Price = item.Price,
                        Cost = item.Cost,
                        Name = item.Name,
                    };
                    if (item.WarrantyLenght != 0)
                    {
                        View.Name += Environment.NewLine + item.WarrantyName;
                    }
                    // Add Warranty
                    if (item.HasAttributes)
                    {
                        foreach (var attr in Attributes)
                        {
                            if (attr.ProductID == item.ID)
                                View.Name += Environment.NewLine + attr.Name + ": " + attr.Value;
                        }
                    }
                    
                    ProductOrderdViewModel.Add(View);
                }
                ProductList.ItemsSource = ProductOrderdViewModel;
            }
        }

        private void ProductList_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "Cost" && !ShowCost)
            {
                e.Cancel = true;
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ShowCost = true;
            ProductList.IsReadOnly = false;
            ProductList.ItemsSource = null;
            ProductList.ItemsSource = ProductOrderdViewModel;

        }
        bool ShowCost = false;
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            ShowCost = false;
            ProductList.IsReadOnly = true;
            ProductList.ItemsSource = null;
            ProductList.ItemsSource = ProductOrderdViewModel;

        }
    }
    public class ProductOrderdViewModel
    {
        public string Name { get; set; }
        public float Price { get; set; }
        public float Cost { get; set; }
        public int QTY { get; set; }
    }

}
