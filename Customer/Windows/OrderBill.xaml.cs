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
using System.Windows.Shapes;

namespace CarSalesSystem.Customer.Windows
{
    /// <summary>
    /// Interaction logic for OrderBill.xaml
    /// </summary>
    public partial class OrderBill : Window
    {
        CUSTOMER customer;
        PRODUCT product;
        
        public OrderBill()
        {
            InitializeComponent();
        }
        public OrderBill(PRODUCT _product, CUSTOMER _customer)
        {
            InitializeComponent();
            customer = _customer;
            product = _product;
            txtTenSP.Text = _product.PRO_NAME;
            txtTenKH.Text = _customer.CUS_NAME;
            txtNgayDatHang.SelectedDate = DateTime.Now;
        }
    }
}
