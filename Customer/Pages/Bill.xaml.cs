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
    /// Interaction logic for Bill.xaml
    /// </summary>
    public partial class Bill : Page
    {
        CUSTOMER cus;
        public Bill(CUSTOMER _cus)
        {
            InitializeComponent();
            cus= _cus;
            datagridOrderBill.ItemsSource = DataProvider.Ins.DB.ORDERBILLs.Where(x=> x.CUSTOMER_ID == cus.CUS_ID).ToList();
        }
    }
}
