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

namespace CarSalesSystem
{
    /// <summary>
    /// Interaction logic for CustomerWindow.xaml
    /// </summary>
    public partial class CustomerWindow : Window
    {
        bool isMaximized = false;
        public CustomerWindow()
        {
            InitializeComponent();
        }

        private void StoreBtn_Click(object sender, RoutedEventArgs e)
        {
            PagesNavigation.Navigate(new System.Uri("Customer/Pages/Store.xaml", UriKind.RelativeOrAbsolute));
        }

        private void billBtn_Click(object sender, RoutedEventArgs e)
        {
            PagesNavigation.Navigate(new System.Uri("Customer/Pages/Bill.xaml", UriKind.RelativeOrAbsolute));
        }

        private void InfoBtn_Click(object sender, RoutedEventArgs e)
        {
            PagesNavigation.Navigate(new System.Uri("Customer/Pages/Info.xaml", UriKind.RelativeOrAbsolute));
        }

        private void PagesNavigation_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (isMaximized)
                {
                    this.WindowState = WindowState.Normal;
                    this.Width = 1280;
                    this.Height = 780;

                    isMaximized = false;

                }
                else
                {
                    this.WindowState = WindowState.Maximized;
                    isMaximized = true;
                }
            }
        }
    }
}
