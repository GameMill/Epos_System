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
using System.Text.RegularExpressions;

namespace EPOS.Pages
{
    /// <summary>
    /// Interaction logic for CreateAccount.xaml
    /// </summary>
    public partial class CreateAccount : Page
    {
        public CreateAccount()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Redirect<Pages.ConpanySelection>();
        }

        public void ReturnTO(Models.UserAccount Model)
        {
            CompanyName.Text = Model.CompanyName;
            AddressLine1.Text = Model.AddressLine1;
            AddressLine2.Text = Model.AddressLine2;
            City.Text = Model.City;
            State.Text = Model.State;
            Postcode.Text = Model.Postcode;
            Vat.Text = Model.Vat;
            Phone.Text = Model.Phone;
            Fax.Text = Model.Fax;
            Email.Text = Model.Email;
            Website.Text = Model.Website;

            UserName.Text = Model.Username;
            Password.Password = Model.Password;

            Invoice.Text = Model.Invoice.ToString();
            Order.Text = Model.Order.ToString();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            bool Cancel = false;
            if(CompanyName.Text == "")
            {
                CompanyName.Background = Brushes.LightCyan;
                Cancel = true;
            }

            if (AddressLine1.Text == "")
            {
                AddressLine1.Background = Brushes.LightCyan;
                Cancel = true;
            }
            if (City.Text == "")
            {
                City.Background = Brushes.LightCyan;
                Cancel = true;
            }

            if (Cancel)
                return;
            var Model = new Models.UserAccount();
            Model.CompanyName = CompanyName.Text;
            Model.AddressLine1 = AddressLine1.Text;
            Model.AddressLine2 = AddressLine2.Text;
            Model.City = City.Text;
            Model.State = State.Text;
            Model.Postcode = Postcode.Text;
            Model.Vat = Vat.Text;
            Model.Phone = Phone.Text;
            Model.Fax = Fax.Text;
            Model.Email = Email.Text;
            Model.Website = Website.Text;

            Model.Username = UserName.Text;
            Model.Password = Password.Password;

            Model.Invoice = int.Parse(Invoice.Text);
            Model.Order = int.Parse(Order.Text);
            MainWindow.Redirect<Pages.CreateDatabase>("CreateNewDatabase", new object[1] { Model });

        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
