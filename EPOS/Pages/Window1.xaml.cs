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
using System.Data.Entity;
using System.Collections.ObjectModel;
using System.Data.Entity;

namespace EPOS.Pages
{
    /* <DataGrid.Columns>
                <DataGridTextColumn Header="ID" Binding="{Binding ID, Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}" />
                <DataGridTextColumn Header="Name" Binding="{Binding Name, Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}" />
                <DataGridTextColumn Header="Price" Binding="{Binding Price, Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}" />
       </DataGrid.Columns>*/
    public partial class Window1 : Page
    {

        public Window1()
        {
            InitializeComponent();
            //TillManager.Instance.FakeLogin();
            var DB = new DB.DBContext();
            DB.Products.Load();
            Status.Text = "Status: Done";

        }
    }
}
