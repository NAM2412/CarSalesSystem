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

namespace CarSalesSystem.Customer.Windows
{
    /// <summary>
    /// Interaction logic for MaintenanceBill.xaml
    /// </summary>
    public partial class MaintenanceBill : Window
    {
        public MaintenanceBill()
        {
            InitializeComponent();
        }


        private void btnCloseDetailWD_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
    }
}
