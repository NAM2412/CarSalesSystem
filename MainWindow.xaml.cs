using System;
using System.Windows;
using System.Windows.Input;


namespace CarSalesSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool isMaximized = false;
        public MainWindow()
        {
            InitializeComponent();
            PagesNavigation.Navigate(new System.Uri("Admin/Pages/Dashboard.xaml", UriKind.RelativeOrAbsolute));
        }

        private void BorderMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void BorderMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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

        private void homeBtn_Click(object sender, RoutedEventArgs e)
        {
            PagesNavigation.Navigate(new System.Uri("Admin/Pages/Dashboard.xaml", UriKind.RelativeOrAbsolute));
           
        }

        private void employeeBtn_Click(object sender, RoutedEventArgs e)
        {
            PagesNavigation.Navigate(new System.Uri("Admin/Pages/Employee.xaml", UriKind.RelativeOrAbsolute));
   
        }

        private void customerBtn_Click(object sender, RoutedEventArgs e)
        {
            PagesNavigation.Navigate(new System.Uri("Admin/Pages/Customer.xaml", UriKind.RelativeOrAbsolute));
        }

        private void productBtn_Click(object sender, RoutedEventArgs e)
        {
            PagesNavigation.Navigate(new System.Uri("Admin/Pages/Product.xaml", UriKind.RelativeOrAbsolute));
        }

        private void warehouseBtn_Click(object sender, RoutedEventArgs e)
        {
            PagesNavigation.Navigate(new System.Uri("Admin/Pages/Warehouse.xaml", UriKind.RelativeOrAbsolute));
        }

        private void infoBtn_Click(object sender, RoutedEventArgs e)
        {
            PagesNavigation.Navigate(new System.Uri("Admin/Pages/Info.xaml", UriKind.RelativeOrAbsolute));
        }
    }
}
