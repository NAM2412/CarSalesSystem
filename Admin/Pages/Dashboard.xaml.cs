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

namespace CarSalesSystem.Admin.Pages
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Page
    {
        PRODUCT product;
        public Dashboard()
        {
            InitializeComponent();
            
            cardBugatti.SaleNumber = NumberCarSale("P001").ToString();
            cardBugatti.Profit = String.Format("{0:0,0}", Calculateprofit("P001"));

            cardMaserati.SaleNumber = NumberCarSale("P002").ToString();
            cardMaserati.Profit = String.Format("{0:0,0}", Calculateprofit("P002"));

            cardToyota.SaleNumber = NumberCarSale("P003").ToString();
            cardToyota.Profit = String.Format("{0:0,0}", Calculateprofit("P003"));

            cardTesla.SaleNumber = NumberCarSale("P004").ToString();
            cardTesla.Profit = String.Format("{0:0,0}", Calculateprofit("P004"));

            cardLexus.SaleNumber = NumberCarSale("P006").ToString();
            cardLexus.Profit = String.Format("{0:0,0}", Calculateprofit("P006"));

        }
        private int NumberCarSale(string carID)
        {
            return DataProvider.Ins.DB.ORDERBILLs.Where(x => x.PRO_ID == carID && x.OB_DATEB <= DateTime.Now).Count();
        }
        private decimal Calculateprofit(string carID)
        {
            product = DataProvider.Ins.DB.PRODUCTs.Where(x => x.PRO_ID.Equals(carID)).FirstOrDefault();
            decimal revenue = product.PRICE*NumberCarSale(product.PRO_ID);
            return revenue + (revenue*30)/100;
        }

    }
}
