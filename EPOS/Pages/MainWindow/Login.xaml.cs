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

namespace EPOS.Pages
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        public Login()
        {
            InitializeComponent();
            MainWindow.SetTitle("Login");
        }
        Page From;
        LoginComplate Complate;
        public void Setup(Page From, LoginComplate Complate, string SelectedConpany=null)
        {
            this.From = From;
            this.Complate = Complate;
            if (SelectedConpany != null && !TillManager.Instance.RequiresLogin(SelectedConpany))
            {
                TillManager.Instance.CurrentUser = TillManager.Instance.GetUser(SelectedConpany);
                Complate();
                //MainWindow.Redirect(ToIfComplate);
            }
        }

        public delegate void LoginComplate();


        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(From);
        }

        public void ChangePage(Page NewPage)
        {
            MainWindow.Redirect(NewPage);

            From = null;
            Complate = null;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var username = Username.Text;
            var password = Password.Password;

            if (username == "" || password == "")
            { return; }
            MainWindow.Print("Username: " + username);
            MainWindow.Print("Password: " + password);

            Complate();
        }
    }
}
