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
            /*
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
                notifier.ShowSuccess("Added new employee successfully");
                
            }
            catch (Exception exception)
            {
                //noti
                notifier.ShowError(exception.Message);
                return;
            }
            connection.Close();
            */
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["CarSalesSystem.Properties.Settings.CARSALESSYSTEMConnectionString"].ConnectionString;
            try
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"INSERT INTO EMPLOYEE (EMP_ID, EMP_ACCOUNT, GENDER, EMP_TYPE, EMP_ADDRESS, EMP_DATE_OF_BIRTH, DATE_OF_WORK, PHONE) 
                        VALUES(@EMP_ID, @EMP_ACCOUNT, @GENDER, @EMP_TYPE, @EMP_ADDRESS, @EMP_DATE_OF_BIRTH, @DATE_OF_WORK, @PHONE)";
                    command.Parameters.AddWithValue("@EMP_ID",AccountInfo.IdAccount);
                    command.Parameters.AddWithValue("@EMP_ACCOUNT", AccountInfo.Username);
                    command.Parameters.AddWithValue("@GENDER", genderBox.Text);
                    command.Parameters.AddWithValue("@EMP_TYPE", positionBox.Text);
                    command.Parameters.AddWithValue("@EMP_ADDRESS", addressBox.Text);
                    command.Parameters.AddWithValue("@EMP_DATE_OF_BIRTH", dobBox.SelectedDate.Value.Date.ToShortDateString());
                    command.Parameters.AddWithValue("@DATE_OF_WORK", dowBox.SelectedDate.Value.Date.ToShortDateString());
                    command.Parameters.AddWithValue(" @PHONE", phoneBox.Text);
                    command.ExecuteNonQuery();
                    notifier.ShowSuccess("Added new employee successfully");

                }
            }
            catch (Exception exception)
            {
                //noti
                notifier.ShowError(exception.Message);
                return;
            }
            finally
            {
                connection.Close();
            }

        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
