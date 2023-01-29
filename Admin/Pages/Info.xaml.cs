using CarSalesSystem.Admin.Windows;
using CarSalesSystem.Model;
using CarSalesSystem.Viewmodel;
using ControlzEx.Standard;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
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
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace CarSalesSystem.Admin.Pages
{
    /// <summary>
    /// Interaction logic for Info.xaml
    /// </summary>
    public partial class Info : Page
    {
        EMPLOYEE employee;
        public Info(EMPLOYEE _employee)
        {
            InitializeComponent();
            
            employee = _employee;
            nameTextBox.Text = employee.EMP_NAME;
            addressTextBox.Text = employee.EMP_ADDRESS;
            phoneTextBox.Text = employee.PHONE;
            genderBox.Text = employee.GENDER;
            birthdayTextBox.SelectedDate = employee.EMP_DATE_OF_BIRTH;
            oldPassBox.Password = employee.ACCOUNT.PASS;
            
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
                maximumNotificationCount: MaximumNotificationCount.FromCount(3));

            cfg.Dispatcher = Application.Current.Dispatcher;
        });

        private void informationButton_Click(object sender, RoutedEventArgs e)
        {
            
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["CarSalesSystem.Properties.Settings.CARSALESSYSTEMConnectionString"].ConnectionString;
            try
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"UPDATE EMPLOYEE
                        SET EMP_NAME=@EMP_NAME, GENDER=@GENDER, EMP_ADDRESS=@EMP_ADDRESS, EMP_DATE_OF_BIRTH=@EMP_DATE_OF_BIRTH,
                            PHONE=@PHONE
                        WHERE EMP_ID=@EMP_ID";
                    command.Parameters.AddWithValue("@EMP_ID", employee.EMP_ID);
                    command.Parameters.AddWithValue("@EMP_NAME", nameTextBox.Text);
                    command.Parameters.AddWithValue("@GENDER", genderBox.Text);
                    command.Parameters.AddWithValue("@EMP_ADDRESS", addressTextBox.Text);
                    command.Parameters.AddWithValue("@EMP_DATE_OF_BIRTH", birthdayTextBox.SelectedDate.Value.ToString("MM-dd-yyyy"));
                    command.Parameters.AddWithValue("@PHONE", phoneTextBox.Text);
                    command.ExecuteNonQuery();
                }
                notifier.ShowSuccess("Successfully Updated Information");
            }
            catch (Exception exception)
            {
                notifier.ShowError(exception.Message);
            }
            finally
            {
                connection.Close();
            }
            
        }


        private void passwordButton_Click(object sender, RoutedEventArgs e)
        {
            
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["CarSalesSystem.Properties.Settings.CARSALESSYSTEMConnectionString"].ConnectionString;
            try
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"UPDATE ACCOUNT
                        SET PASS=@PASS
                        WHERE USERNAME=@USERNAME";
                    
                    command.Parameters.AddWithValue("@PASS", newPassBox.Password);
                    command.Parameters.AddWithValue("@USERNAME", employee.ACCOUNT);
                    command.ExecuteNonQuery();
                }
                notifier.ShowSuccess("Successfully Updated New Password");
            }
            catch (Exception exception)
            {
                notifier.ShowError(exception.Message);
            }
            finally
            {
                connection.Close();
            }
            
        }

        private void rankUpdateButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateRankMoney updateRankMoney = new UpdateRankMoney();
            updateRankMoney.Show();
        }
    }
}
