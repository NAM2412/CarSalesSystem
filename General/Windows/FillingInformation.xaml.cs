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

        private void usernameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (usernameTextBox.Text.Equals(" Enter username"))
                usernameTextBox.Text = "";
        }

        private void usernameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (usernameTextBox.Text.Equals(""))
                usernameTextBox.Text = " Enter username";
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
            CustomMessageBox customMessageBox = new CustomMessageBox();
            if (usernameTextBox.Text.Equals(" Enter username"))
            {
                customMessageBox.Show("Warning","Please enter a username!", CustomMessageBox.MessageBoxType.Warning);
                usernameTextBox.Focus();
                return;
            }
            if (passwordTextBox.Password.Length == 0)
            {
                customMessageBox.Show("Warning", "Please enter your password!", CustomMessageBox.MessageBoxType.Warning);
                passwordTextBox.Focus();
                return;
            }
            if (confirmTextBox.Password.Length == 0)
            {
                customMessageBox.Show("Warning", "Please re-enter your password!", CustomMessageBox.MessageBoxType.Warning);
                passwordTextBox.Focus();
                return;
            }
            if (passwordTextBox.Password != confirmTextBox.Password)
            {
                customMessageBox.Show("Warning", "Your confirmed password does not match", CustomMessageBox.MessageBoxType.Warning);
                passwordTextBox.Focus();
                return;
            }
            
            
            SqlConnection connection = new SqlConnection("Data Source=MSI\\SQLEXPRESS;Initial Catalog=CARSALESSYSTEM;Integrated Security=True;");
            connection.Open();
            SqlCommand cmd = new SqlCommand("Insert into ACCOUNT (USERNAME,PASS,TYPE_USER) values('" + usernameTextBox.Text + "','" + passwordTextBox.Password+ "','" + "1')", connection);
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            connection.Close();
            return;
            
        }
    }
}
