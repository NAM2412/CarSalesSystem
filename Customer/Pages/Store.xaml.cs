using CarSalesSystem.Customer.Windows;
using CarSalesSystem.Model;
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

namespace CarSalesSystem.Customer.Pages
{
    /// <summary>
    /// Interaction logic for Store.xaml
    /// </summary>
    public partial class Store : Page
    {
        CUSTOMER customer;
    
        public Store()
        {
            InitializeComponent();
            ListProduct.ItemsSource = DataProvider.Ins.DB.PRODUCTs.ToList();
            customer = DataProvider.Ins.DB.CUSTOMERs.FirstOrDefault();
        }

        private void ListProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PRODUCT prd = ListProduct.SelectedItem as PRODUCT;
            DetailCar detailCar = new DetailCar(prd, customer);
            detailCar.Show();
        }
    }
}
