using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Position;
using ToastNotifications.Messages;
using CarSalesSystem.Viewmodel;
using System.Configuration;
using System.Web.UI.WebControls;
using CarSalesSystem.Model;

namespace CarSalesSystem
{
    /// <summary>
    /// Interaction logic for Addemp.xaml
    /// </summary>
    public partial class Addemp : Window
    {
        public Addemp()
        {
            InitializeComponent();
        }
 

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
