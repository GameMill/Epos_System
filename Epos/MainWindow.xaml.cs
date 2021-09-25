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

namespace EPOS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static Page CurrentPage = null;
        private static Page LastPage = null;
        public static Type MainPageAfterLogin = typeof(Pages.Invoice);

        public MainWindow()
        {
            InitializeComponent();
            Redirect<Pages.ConpanySelection>();
            
            //TillManager.Instance.FakeLogin();
            //Redirect<Pages.Invoice>();
            // TillManager.Instance.FakeLogin();
            //Redirect<Pages.Window1>();
            SizeChanged += MainWindow_SizeChanged;
            

        }
        
        /*private void SetupBarcodeScanner()
        {
            var hook = new KeyboardHook();
            var availableScanners = KeyboardHook.GetKeyboardDevices();
            // find out which scanner to use 
            hook.SetDeviceFilter(availableScanners.First());
            hook.KeyPressed += OnBarcodeKey;
            hook.AddHook(this);
        }
        public void OnBarcodeKey(object sender, KeyPressedEventArgs e)
        {
            MessageBox.Show("received " + e.Text);
        }*/

        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            contentControl.Width = e.NewSize.Width;
            contentControl.Height = e.NewSize.Height;
        }
        
        
        

        public static void Redirect(Page TO,string Action="", object[] Arg = null,bool Direct = true)
        {
            if (CurrentEndPage != TO && Direct == false)
                return;
            LastPage = CurrentPage;
            CurrentPage = TO;
            ((MainWindow)Application.Current.MainWindow).contentControl.Content = CurrentPage.Content;

            if (Action == "")
                return;

            Type PageType = TO.GetType();

            var theMethod = PageType.GetMethod(Action);
            theMethod.Invoke(CurrentPage, Arg);
        }

        public static void Print(string Message)
        {
            System.Diagnostics.Debug.Print(Message);
        }
        public static Page CurrentEndPage;
        public static void Redirect<T>(string Action="",object[] Arg=null ) where T: Page
        {
            ((MainWindow)Application.Current.MainWindow).DataContext = null;
            CurrentEndPage = (T)Activator.CreateInstance(typeof(T));
            Redirect(CurrentEndPage, Action, Arg,false);
        }

        public static void Redirect(Type Page,string Action = "", object[] Arg = null)
        {
            ((MainWindow)Application.Current.MainWindow).DataContext = null;
            CurrentEndPage = (Page)Activator.CreateInstance(Page);
            Redirect(CurrentEndPage, Action, Arg,false);
        }

        public static void ReturnToLastPage()
        {
            CurrentPage = LastPage;
            LastPage = null;
            Application.Current.MainWindow.Content = LastPage.Content;
        }

        public static void SetTitle(string Name)
        {
            Application.Current.MainWindow.Title = "Epos - " + Name;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //SetupBarcodeScanner();

        }
    }
}
