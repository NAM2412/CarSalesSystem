using CarSalesSystem.Admin.Windows;
using System;
using System.Collections.Generic;
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

namespace CarSalesSystem.Admin.Pages
{
    /// <summary>
    /// Interaction logic for Info.xaml
    /// </summary>
    public partial class Info : Page
    {
        public Info()
        {
            InitializeComponent();
        }
        private void informationButton_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection connection =
            new SqlConnection("Data Source=MSI\\SQLEXPRESS;Initial Catalog=CARSALESSYSTEM;Integrated Security=True"))
            {
                connection.Open();
                using (SqlCommand cmd =
                    new SqlCommand("UPDATE EMPLOYEE SET EMP_NAME='" + nameTextBox.Text +
                    "', GENDER='" + genderBox.Text +
                    "', EMP_ADDRESS='"+ addressTextBox.Text +
                    "', EMP_DATE_OF_BIRTH='" + birthdayTextBox.DisplayDate +
                    "', PHONE='" + phoneTextBox.Text +

                    " WHERE EMP_ID=@ID", connection))
                {
                    /*
                    cmd.Parameters.AddWithValue("EMP_NAME", user.UserId);
                    cmd.Parameters.AddWithValue("@firstname", user.FirstName);
                    cmd.Parameters.AddWithValue("@lastname", user.LastName);
                    
                    //add whatever parameters you required to update here
                    int rows = cmd.ExecuteNonQuery();
                    connection.Close();
                    */
                }
            }
        }


        private void passwordButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void rankUpdateButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateRankMoney updateRankMoney = new UpdateRankMoney();
            updateRankMoney.Show();
        }
    }
}
