using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.ModelBinding;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.Threading;
using ToastNotifications;
using ToastNotifications.Position;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;

namespace CarSalesSystem.General.Windows
{
    /// <summary>
    /// Interaction logic for FillingInformation.xaml
    /// </summary>
    public partial class FillingInformation : Window
    {
        public FillingInformation()
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

        private void usernameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (usernameTextBox.Text.Equals("Enter username"))
                usernameTextBox.Text = "";
        }

        private void usernameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (usernameTextBox.Text.Equals(""))
                usernameTextBox.Text = "Enter username";
        }
        private void passwordTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            phPass.Visibility = Visibility.Hidden;
        }

        private void passwordTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(passwordTextBox.Password))
                phPass.Visibility = Visibility.Visible;
        }

        private void confirmTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            confirmPhPass.Visibility = Visibility.Hidden;
        }

        private void confirmTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(confirmTextBox.Password))
                confirmPhPass.Visibility = Visibility.Visible;
        }
        private void SignUpBtn_Click(object sender, RoutedEventArgs e)
        {
            if (usernameTextBox.Text.Equals(" Enter username"))
            {
                //noti
                notifier.ShowWarning("Please enter your username!");
                usernameTextBox.Focus();
                return;
            }
            if (passwordTextBox.Password.Length == 0)
            {
                //noti
                notifier.ShowWarning("Please type in your password!"); ;
                passwordTextBox.Focus();
                return;
            }
            if (confirmTextBox.Password.Length == 0)
            {
                //noti
                notifier.ShowWarning("Please confirm your password!"); ;
                confirmTextBox.Focus();
                return;
            }
            if (passwordTextBox.Password != confirmTextBox.Password)
            {
                //noti
                notifier.ShowWarning("Your password does not match!"); ;
                passwordTextBox.Focus();
                return;
            }
            
            
            SqlConnection connection = new SqlConnection("Data Source=MSI\\SQLEXPRESS;Initial Catalog=CARSALESSYSTEM;Integrated Security=True;");
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("Insert into ACCOUNT(USERNAME,PASS,TYPE_USER) values('" + usernameTextBox.Text + "','" + passwordTextBox.Password + "','" + "1')", connection);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();

                //create new customer following new account registration
                cmd = new SqlCommand("INSERT INTO CUSTOMER (CUS_NAME, CUS_ACCOUNT, PHONE, CUS_ADDRESS, REGIST_DATE, IMG, RANK_ID) VALUES('" + nameBox.Text +
                                    "','" + usernameTextBox.Text +
                                    "','" + phoneBox.Text +
                                    "','" + addressBox.Text +
                                    "','" + DateTime.Now.ToShortDateString() +
                                    "', NULL,'R00')", connection) ;
                cmd.ExecuteNonQuery();
            }
            catch(Exception exception)
            {
                //noti
                notifier.ShowError(exception.Message);
                return;
            }
            connection.Close();
            SuccessfulMessage message= new SuccessfulMessage();
            message.Show();
            this.Close();
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void addressBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (addressBox.Text.Equals("Address"))
                addressBox.Text = "";
        }

        private void addressBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (addressBox.Text.Equals(""))
                addressBox.Text = "Address";
        }

        private void nameBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (nameBox.Text.Equals("Full name"))
                nameBox.Text = "";
        }

        private void nameBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (nameBox.Text.Equals(""))
                nameBox.Text = "Full name";

        }

        private void phoneBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (phoneBox.Text.Equals("Phone number"))
                phoneBox.Text = "";

        }

        private void phoneBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (phoneBox.Text.Equals(""))
                phoneBox.Text = "Phone number";

        }
    }
}
