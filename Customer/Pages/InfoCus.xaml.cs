﻿using CarSalesSystem.Admin.Pages;
using CarSalesSystem.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace CarSalesSystem.Customer.Pages
{
    /// <summary>
    /// Interaction logic for Info.xaml
    /// </summary>
    public partial class InfoCus : Page
    {

        private static readonly Regex _regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text
        CUSTOMER cus;
        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
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
        public InfoCus(CUSTOMER _cus)
        {
            InitializeComponent();
            cus = _cus;
            txbFullName.Text = cus.CUS_NAME;
            txbAddress.Text = cus.CUS_ADDRESS;
            txbPhoneNumber.Text = cus.PHONE;
            genderBox.Text = cus.GENDER;
            dtpDateOfBirth.SelectedDate = cus.CUS_DATE_OF_BIRTH;
            decimal totaSpent;
            if (DataProvider.Ins.DB.ORDERBILLs.Where(x => x.CUSTOMER_ID == cus.CUS_ID).Sum(x => x.TOTAL_PRICE) == null)
            {
                totaSpent = 0;
            }
            else
            {
                totaSpent = (decimal)DataProvider.Ins.DB.ORDERBILLs.Where(x => x.CUSTOMER_ID == cus.CUS_ID).Sum(x => x.TOTAL_PRICE);
            }
            if (totaSpent == 0)
            {
                lbTotalSpent.Content = "0";
            }
            else
            {
                lbTotalSpent.Content = String.Format("{0:0,0}", totaSpent);
            }
            lbTotalSpent.Content = String.Format("{0:0,0}", totaSpent);
            lbRankName.Content = RenameRankType(cus);
            lbDiscount.Content = cus.RANK_MONEY.DISCOUNT;
            oldPassBox.Content = cus.ACCOUNT.PASS;
        }

        private string RenameRankType(CUSTOMER _cus)
        {
            switch(_cus.RANK_ID)
            {
                case "R01":
                    return "Silver";
                case "R02":
                    return "Gold";
                case "R03":
                    return "Diamond";
                default:
                    return "No rank";
            }   

        }
        private void informationButton_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection connection = new SqlConnection("Data Source=DESKTOP-8RKPG08\\SQLEXPRESS;Initial Catalog=CARSALESSYSTEM;Integrated Security=True");
            string query = "update CUSTOMER set CUS_NAME=@CUS_NAME, CUS_DATE_OF_BIRTH=@CUS_DATE_OF_BIRTH, GENDER=@GENDER, PHONE=@PHONE, CUS_ADDRESS=@CUS_ADDRESS where CUS_ID=@CUS_ID";
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@CUS_NAME", txbFullName.Text);
                    command.Parameters.AddWithValue("@CUS_DATE_OF_BIRTH", DateTime.Parse(dtpDateOfBirth.Text));
                    command.Parameters.AddWithValue("@GENDER", genderBox.Text);
                    command.Parameters.AddWithValue("@PHONE", txbPhoneNumber.Text);
                    command.Parameters.AddWithValue("@CUS_ADDRESS", txbAddress.Text);
                    command.Parameters.AddWithValue("@CUS_ID", cus.CUS_ID);
                    command.ExecuteNonQuery();
                    notifier.ShowSuccess("Successfully Updated Information");
                }
                catch (Exception ex)
                {
                    notifier.ShowError(ex.Message);
                }
                connection.Close();

        }

        private void passwordButton_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection connection = new SqlConnection("Data Source=DESKTOP-8RKPG08\\SQLEXPRESS;Initial Catalog=CARSALESSYSTEM;Integrated Security=True");
            string query = "update ACCOUNT set PASS=@PASS where USERNAME=@USERNAME";
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@PASS", newPassBox.Content);
                command.Parameters.AddWithValue("@USERNAME", cus.ACCOUNT);
                command.ExecuteNonQuery();
                notifier.ShowSuccess("Successfully Updated New Password");
            }
            catch (Exception ex)
            {
                notifier.ShowError(ex.Message);
            }
            connection.Close();
        }

        private void phoneTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }
    }
}
