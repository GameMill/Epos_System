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
using System.Windows.Shapes;

namespace EPOS.Windows
{
    /// <summary>
    /// Interaction logic for Attribute.xaml
    /// </summary>
    public partial class Attribute : Window
    {
        public static Attribute Instace;
        public int CurrentAttribute = 0;
        public List<string> Attributes;
        public CartInvertory Item;
        public Dictionary<string, string> Att = new Dictionary<string, string>();
        public Attribute()
        {
            InitializeComponent();
        }
        public static void Open(CartInvertory Item,List<string> Attributes)
        {
            if(Instace != null)
            {
                Instace.Close();
                Instace = null;
            }
            Instace = new Attribute();
            Instace.Attributes = Attributes;
            Instace.Item = Item;
            Instace.test.Text = Attributes[Instace.CurrentAttribute];
            Instace.Show();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Instace = null;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Att.Add(Attributes[CurrentAttribute], Box.Text);
            if (CurrentAttribute == 0)
                Item.Name += "\r\nItem "+Item.QTY.ToString();
            Item.Name += "\r\n" + Attributes[CurrentAttribute]+": "+ Box.Text;

            CurrentAttribute++;
            if(CurrentAttribute == Attributes.Count)
            {
                CurrentAttribute = 0;
                Attributes = null;
                Item.Attributes.Add(Att);
                Att = null;
                Item = null;
                Close();
            }
        }
    }
    public class Math : MathConverter { }
}
