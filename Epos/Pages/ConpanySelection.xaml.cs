using Microsoft.EntityFrameworkCore;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Epos.Pages
{
    /// <summary>
    /// Interaction logic for ConpanySelection.xaml
    /// </summary>
    public partial class ConpanySelection : Page
    {
        public ConpanySelection()
        {
            InitializeComponent();
            DBContext.CoreDBContext.Instance.Conpanys.Load();
            ListView.ItemsSource = DBContext.CoreDBContext.Instance.Conpanys.Local.ToBindingList();
        }

        private void NewConpany_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.Frame.Navigate(new NewConpany());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ListView.SelectedItem == null) return;
            MainWindow.Conpany = (Models.Conpany)ListView.SelectedItem;
            MainWindow.Instance.Frame.Navigate(new Dashboard());
        }
    }
}
