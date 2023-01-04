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
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.Linq.Expressions;
using CarSalesSystem.General.Windows;
using ToastNotifications;
using ToastNotifications.Messages;
using ToastNotifications.Lifetime;
using ToastNotifications.Position;

namespace CarSalesSystem.General
{
    /// <summary>
    /// Interaction logic for SignIn.xaml
    /// </summary>
    public partial class SignIn : Window
    {
        private bool usernameValid = false;
        private bool passwordValid = false;
        private int type_user = 1;

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
        public SignIn()
        {
            InitializeComponent();
        }
        void CloseWindow(Type type)
        {
            var window = App.Current.Windows.OfType<Window>().FirstOrDefault(w => w.GetType() == type);
            if (window != null)
                window.Close();
        }
        private void SignUpBtn_Click(object sender, RoutedEventArgs e)
        {
            SignUp signUp = new SignUp();
            signUp.Show();
            CloseWindow(typeof(SignIn));

        }

        private void usernameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (usernameTextBox.Text.Equals(" Username"))
                usernameTextBox.Text = "";
            if (usernameTextBox.BorderBrush == Brushes.Red)
                usernameTextBox.BorderBrush = Brushes.White;
        }

        private void usernameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (usernameTextBox.Text.Equals(""))
                usernameTextBox.Text = " Username";
        }

        private void passwordTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            phPass.Visibility = Visibility.Hidden;
            if (passwordTextBox.BorderBrush == Brushes.Red)
                passwordTextBox.BorderBrush = Brushes.White;
        }

        private void passwordTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(passwordTextBox.Password))
                phPass.Visibility = Visibility.Visible;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow adminWindow = new MainWindow();
            CustomerWindow customerWindow = new CustomerWindow();
            if (usernameTextBox.Text.Equals("admin12312") && passwordTextBox.Password.Equals("123456"))
            {
                adminWindow.Show();
                this.Close();
                return;
            }
            if (usernameValid && passwordValid)
            {
                customerWindow.Show();
                this.Close();
                return;
            }
            if (!usernameValid)
                usernameTextBox.BorderBrush = Brushes.Red;
            if (!passwordValid)
                passwordTextBox.BorderBrush = Brushes.Red;
            //retrieve data and compare with data from database
            SqlConnection connection = new SqlConnection("Data Source=MSI\\SQLEXPRESS;Initial Catalog=CARSALESSYSTEM;Integrated Security=True;MultipleActiveResultSets=true");
            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                SqlCommand cmd = new SqlCommand("Select COUNT(1) from ACCOUNT where USERNAME='" + usernameTextBox.Text + "'  and PASS='" + passwordTextBox.Password + "'", connection);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@USERNAME", usernameTextBox.Text);
                cmd.Parameters.AddWithValue("@PASS", passwordTextBox.Password);
                int result = Convert.ToInt32(cmd.ExecuteScalar());
                 if (result == 1)
                 {
                     usernameValid = true;
                     passwordValid = true;
                     customerWindow.Show();
                     this.Close();
                 }
                 else
                 {
                    //noti
                    notifier.ShowError("Invalid username or wrong password, please check again.");

                 }
               
            }
            catch (Exception ex)
            {
                notifier.ShowError(ex.Message);
            }
            

            
            
            connection.Close();
            
        }
    }
}
