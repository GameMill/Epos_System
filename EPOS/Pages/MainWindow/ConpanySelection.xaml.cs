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
    /// Interaction logic for ConpanySelection.xaml
    /// </summary>
    public partial class ConpanySelection : Page
    {
        public static string DataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\CB_Epos_Systems\\";
        public static string AccountsPath = DataPath + "Accounts\\";
        public ConpanySelection()
        {
            InitializeComponent();
            

            if(!System.IO.Directory.Exists(DataPath))
            {
                System.IO.Directory.CreateDirectory(DataPath);
            }
            if (!System.IO.Directory.Exists(AccountsPath))
            {
                System.IO.Directory.CreateDirectory(AccountsPath);
            }
            List<string> items = new List<string>();
            foreach (var item in System.IO.Directory.GetFiles(AccountsPath))
            {
                var Length = item.Length - AccountsPath.Length;
                Length -= 4;
                items.Add((item.Substring(AccountsPath.Length, Length)).Replace('_',' '));
            }

            lvUsers.ItemsSource = items;
        }

        private void SelectCompany(object sender, RoutedEventArgs e)
        {
            Pages.Login.LoginComplate Complate = () => {
                MainWindow.Redirect(MainWindow.MainPageAfterLogin);
            };

            var Conpany = lvUsers.SelectedItem as string;
            if (Conpany != null)
            {
                MainWindow.Redirect<Pages.Login>("Setup",new object[3]{this, Complate, Conpany });
            }
        }

        private void lvUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectButton.IsEnabled = true;
            RemoveButton.IsEnabled = true;
        }

        private void CreateCompany(object sender, RoutedEventArgs e)
        {
            MainWindow.Redirect<Pages.CreateAccount>();
        }

        private void RemoveCompany(object sender, RoutedEventArgs e)
        {
            var Company = lvUsers.SelectedItem as string;
            Pages.Login.LoginComplate Complate = () => {
                Models.UserAccount.Load(Company).DeleteAccount();
                MainWindow.Redirect<Pages.ConpanySelection>();
            };
            

            MainWindow.Redirect<Login>("Setup", new object[3] { this, Complate, Company});
        }
    }
}
