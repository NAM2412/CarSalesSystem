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
        Notifier notifier = new Notifier(cfg =>
        {
            cfg.PositionProvider = new WindowPositionProvider(
                parentWindow: Application.Current.MainWindow,
                corner: Corner.TopRight,
                offsetX: 10,
                offsetY: 10);

            cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                notificationLifetime: TimeSpan.FromSeconds(3),
                maximumNotificationCount: MaximumNotificationCount.FromCount(5));

            cfg.Dispatcher = Application.Current.Dispatcher;
        });


        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection connection = new SqlConnection("Data Source=MSI\\SQLEXPRESS;Initial Catalog=CARSALESSYSTEM;Integrated Security=True;");
            try
            {
                connection.Open();
                //create new customer following new account registration
                SqlCommand cmd = new SqlCommand("INSERT INTO EMPLOYEE (EMP_ID, EMP_ACCOUNT, GENDER, EMP_TYPE, EMP_ADDRESS, EMP_DATE_OF_BIRTH, DATE_OF_WORK, PHONE) VALUES('" + idBox.Text +
                                    "','" + AccountInfo.Username + 
                                    "','" + genderBox.Text +
                                    "','" + positionBox.Text +
                                    "','" + addressBox.Text +
                                    "','" + dobBox.SelectedDate.Value.Date.ToShortDateString() +
                                    "','" + dowBox.SelectedDate.Value.Date.ToShortDateString() + 
                                    "','" + phoneBox.Text +
                                    "')", connection);
                cmd.ExecuteNonQuery();
            }
            catch (Exception exception)
            {
                //noti
                notifier.ShowError(exception.Message);
                return;
            }
            connection.Close();

        }
    }
}
