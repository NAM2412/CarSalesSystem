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

namespace CarSalesSystem.General
{
    /// <summary>
    /// Interaction logic for SignIn.xaml
    /// </summary>
    public partial class SignIn : Window
    {
        private bool usernameValid = false;
        private bool passwordValid = false;
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
            if (usernameTextBox.Text.Equals("Username"))
                usernameTextBox.Text = "";
            if (usernameTextBox.BorderBrush == Brushes.Red)
                usernameTextBox.BorderBrush = Brushes.White;
        }

        private void usernameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (usernameTextBox.Text.Equals(""))
                usernameTextBox.Text = "Username";
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
            /*
            SqlConnection connection = new SqlConnection("Data Source=localHost;Initial Catalog=Data;");
            connection.Open();
            SqlCommand cmd = new SqlCommand("Select * from ACCOUNT where USERNAME='" + usernameTextBox.Text + "'  and PASSWORD='" + passwordTextBox.Password+ "'", connection);
            cmd.CommandType = CommandType.Text;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = cmd;
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet);
            if (dataSet.Tables[0].Rows.Count > 0)
            {
                customerWindow.Show();
                this.Close();
            }
            connection.Close();
            */
        }
    }
}
