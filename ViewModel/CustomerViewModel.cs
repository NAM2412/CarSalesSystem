using CarSalesSystem.Admin.Pages;
using CarSalesSystem.Admin.User_Controls;
using CarSalesSystem.Admin.Windows;
using CarSalesSystem.Model;
using CarSalesSystem.Viewmodel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CarSalesSystem.ViewModel
{
    public class CustomerViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private CustomerPG customer;
        private String idCustom;
        private String imagefilename;
        private CustomerControl customerControl;
        public CustomerPG customerPG { get => customer; set => customer = value; }
        public ICommand loadCustomerCommand { get; set; }
        public ICommand EditCusomterCommand { get; set; }
        public ICommand UploadPictureCommand { get; set; }

        public ICommand UpdateInforCommand { get; set; }
        public ICommand BackCommand { get; set; }
        public CustomerViewModel()
        {
            loadCustomerCommand = new RelayCommand<CustomerPG>((parameter) => true, (parameter) => LoadCustomer(parameter));
            EditCusomterCommand = new RelayCommand<CustomerControl>((parameter) => true, (parameter) => ClickEditCusomter(parameter));
            UploadPictureCommand = new RelayCommand<Grid>((parameter) => true, (parameter) => UploadPicture(parameter));
            UpdateInforCommand = new RelayCommand<EditCustomer>((parameter) => true, (parameter) => UpdateInfor(parameter));
            BackCommand = new RelayCommand<EditCustomer>((parameter) => true, (parameter) => parameter.Close());
        }

        private void UpdateInfor(EditCustomer parameter)
        {
            var customerInfo = DataProvider.Ins.DB.CUSTOMERs.Find(idCustom);
            customerInfo.CUS_NAME = parameter.tbFullName.Text;
            customerInfo.CUS_ADDRESS = parameter.tbAddress.Text;
            customerInfo.PHONE = parameter.tbTelephone.Text;
            customerInfo.CUS_ADDRESS = parameter.tbAddress.Text;
            customerInfo.GENDER = parameter.cbGender.Text;
            customerInfo.CUS_DATE_OF_BIRTH = DateTime.Parse(parameter.dpBirth.Text);
            customerInfo.IMG = Converter.Instance.StreamFile(imagefilename);
            DataProvider.Ins.DB.SaveChanges();
            parameter.Close();

        }
        private void UploadPicture(Grid parameter)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                imagefilename = op.FileName;
                ImageBrush imageBrush = new ImageBrush();
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.UriSource = new Uri(imagefilename);
                bitmap.EndInit();
                imageBrush.ImageSource = bitmap;
                parameter.Background = imageBrush;
            }
        }

        private void ClickEditCusomter(CustomerControl parameter)
        {
            this.customerControl = parameter;
            idCustom = parameter.tbNo.Text;
            showEditCustomer(parameter.tbNo.Text);
        }

        public void showEditCustomer(String idCus)
        {
            EditCustomer editCustomerwindow = new EditCustomer();
            editCustomerwindow.Title = "Edit information of Customer";
            var customerInfo = DataProvider.Ins.DB.CUSTOMERs.Find(idCus);
            editCustomerwindow.tbFullName.Text = customerInfo.CUS_NAME;
            editCustomerwindow.tbAddress.Text = customerInfo.CUS_ADDRESS;
            editCustomerwindow.tbTelephone.Text = customerInfo.PHONE;
            editCustomerwindow.tbExpenditure.Text = (((int)customerInfo.REVENUE.GetValueOrDefault()) + " USD").ToString();
            editCustomerwindow.tbExpenditure.Foreground = System.Windows.Media.Brushes.Green;
            editCustomerwindow.lbRegistDate.Content = customerInfo.REGIST_DATE.GetValueOrDefault();

            if (customerInfo.PRODUCT_NUMBER == null) editCustomerwindow.tbProducNumber.Text = "0";
            else editCustomerwindow.tbProducNumber.Text = customerInfo.PRODUCT_NUMBER.ToString();

            if (customerInfo.GENDER == null) editCustomerwindow.cbGender.SelectedIndex = 1;
            else 
                editCustomerwindow.cbGender.Text = customerInfo.GENDER.ToString();

            editCustomerwindow.cbRank.Text = customerInfo.RANK_MONEY.RANK_TYPE.ToString();
            if (customerInfo.CUS_DATE_OF_BIRTH.GetValueOrDefault() != null)
                editCustomerwindow.dpBirth.Text = customerInfo.CUS_DATE_OF_BIRTH.GetValueOrDefault().ToString();
            string s = editCustomerwindow.tbExpenditure.Text;
            s = s.Remove(s.IndexOf(' '));
            if (customerInfo.IMG != null)
            {
                ImageBrush imageBrush = new ImageBrush();
                imageBrush.ImageSource = Converter.Instance.ToImage(customerInfo.IMG);
                editCustomerwindow.grdSelectImage.Background = imageBrush;
            }

            editCustomerwindow.ShowDialog();

        }

        private void LoadCustomer(CustomerPG parameter)
        {
            this.customer = parameter;
            parameter.skpCustomer.Children.Clear();

            var listCustomer = DataProvider.Ins.DB.CUSTOMERs.ToList();
            bool flat = false;
            int i = 1;
            foreach (var item in listCustomer)
            {
                CustomerControl infCustomer = new CustomerControl();
                flat = !flat;
                if (flat)
                {
                    infCustomer.grMain.Background = (System.Windows.Media.Brush)new BrushConverter().ConvertFrom("#FFFFFF");
                }
                infCustomer.tbName.Text = item.CUS_NAME;
                infCustomer.tbNo.Text = item.CUS_ID;
                infCustomer.tbSex.Text = item.GENDER;
                infCustomer.tbRank.Text = item.RANK_MONEY.RANK_TYPE.ToString();
                parameter.skpCustomer.Children.Add(infCustomer);
                i++;
            }
        }
    }
}