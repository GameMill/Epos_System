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
using System.Data.Entity;

namespace EPOS.Pages.Invertory
{
    /// <summary>
    /// Interaction logic for Orders.xaml
    /// </summary>
    public partial class Orders : Page
    {
        public Orders()
        {
            InitializeComponent();
            DB.DBContext.Instance.Orders.Load();
            Order = DB.DBContext.Instance.Orders.Local.ToList();
            gridEmployees.ItemsSource = Order;
        }
        List<DB.Models.Order> Order;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).Tag != null)
            {
                int ID = (int)((Button)sender).Tag;
                Windows.Invertory.Redirect<DetailOrder>("Setup", new object[2] { ID, this });
            }
        }
        public void RefreshGrid(bool RefreshRowEdit = false)
        {
            //if (RefreshRowEdit)
            //{
            //    gridEmployees.RowEditEnding -= DataGrid_RowEditEnding;
            //    gridEmployees.CommitEdit();
            //}

            gridEmployees.Items.Refresh();

            //if (RefreshRowEdit)
            //    gridEmployees.RowEditEnding += DataGrid_RowEditEnding;
        }

        private void Filter_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (gridEmployees == null)
                return;
            if (FilterText.Text == "")
            {
                DB.DBContext.Instance.Orders.Load();
                gridEmployees.Items.Refresh();
                return;
            }

            string Type = ((ListBoxItem)FilterSelection.SelectedItem).Content.ToString();
            if (Type == "LessThan")
            {
                // ((System.Data.DataView)gridEmployees.DataContext).RowFilter = "Price < " + FilterText.Text + "";
                float Num;
                if (float.TryParse(FilterText.Text, out Num))
                    gridEmployees.ItemsSource = DB.DBContext.Instance.Orders.Where(a => a.TotalPrice < Num).ToList();
            }
            else if (Type == "MoreThan")
            {
                float Num;
                if(float.TryParse(FilterText.Text,out Num))
                    gridEmployees.ItemsSource = DB.DBContext.Instance.Orders.Where(a => a.TotalPrice > Num).ToList();

            }
            else
            {
                gridEmployees.ItemsSource = DB.DBContext.Instance.Orders.Where(a => a.InvoiceNumber.ToString().Contains(FilterText.Text)).ToList();
            }

        }
        private void Filter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterText.Text = "";
            Filter_TextChanged(null, null);

        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
