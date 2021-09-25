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
    /// Interaction logic for NewConpany.xaml
    /// </summary>
    public partial class NewConpany : Page
    {
        public NewConpany()
        {
            InitializeComponent();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            DBContext.CoreDBContext.Instance.Conpanys.Add(new Models.Conpany()
            {
                ConpanyName = ConpanyName.Text,
                PhoneNumber = PhoneNumber.Text,
                Postcode = Postcode.Text,
                EmailAddress = EmailAddress.Text,
                Country = country.Text,
                County = country.Text,
                FirstLine = FirstLine.Text,
                WebSite = WebSite.Text,
                Registed = DateTime.Now
            });
            DBContext.CoreDBContext.Instance.SaveChanges();
            MainWindow.Instance.Frame.Navigate(new ConpanySelection());
        }
    }
}
