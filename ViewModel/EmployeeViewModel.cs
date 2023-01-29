using CarSalesSystem.Admin.Pages;
using CarSalesSystem.Admin.User_Controls;
using CarSalesSystem.Admin.Windows;
using CarSalesSystem.Model;
using CarSalesSystem.Viewmodel;
using CarSalesSystem.ViewModel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace CarSalesSystem.ViewModel
{
    
    public class EmployeeViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private string imagefilename;
        private string idEmp;
        private EmployeeControl employeeControl;
        private EmployeePG empPG;
        public EmployeePG EmPG { get => empPG; set => empPG = value; }
        public ICommand LoadEmployeeCommand { get; set; }
        public ICommand AddEmployeeCommand { get; set; }
        public ICommand UploadImageCommand { get; set; }
        public ICommand DeleteEmployeeCommand { get; set; }
        public ICommand EditEmployeeCommand { get; set; }
        public ICommand UpdateInfoEmpCommand { get; set; }
        public EmployeeViewModel()
        {
            LoadEmployeeCommand = new RelayCommand<EmployeePG>((parameter) => true, (parameter) => LoadEmployee(parameter));
            AddEmployeeCommand = new RelayCommand<Addemp>((parameter) => true, (parameter) => AddEmployee(parameter));
            UploadImageCommand = new RelayCommand<Grid>((parameter) => true, (parameter) => UploadImage(parameter));
            DeleteEmployeeCommand = new RelayCommand<EmployeeControl>((parameter) => true, (parameter) => DeleteEmployee(parameter));
            EditEmployeeCommand = new RelayCommand<EmployeeControl>((parameter) => true, (parameter) => ClickEditEmployee(parameter));
            UpdateInfoEmpCommand = new RelayCommand<Editemp>((parameter) => true, (parameter) => UpdateInfoEmp(parameter));
        }

        private void UpdateInfoEmp(Editemp parameter)
        {
            var empinfo = DataProvider.Ins.DB.EMPLOYEEs.Find(parameter.idBox.Text);

            if (parameter.positionBox.SelectedIndex == 0)
            {
                empinfo.ACCOUNT.TYPE_USER = 1;
            }
            else if (parameter.positionBox.SelectedIndex == 1)
            {
                empinfo.ACCOUNT.TYPE_USER = 0;
            }
            empinfo.GENDER = parameter.genderBox.Text;
            empinfo.EMP_NAME = parameter.FullName.Text;
            empinfo.EMP_ADDRESS = parameter.addressBox.Text;
            empinfo.EMP_DATE_OF_BIRTH = DateTime.Parse(parameter.dobBox.Text);
            empinfo.PHONE = parameter.phoneBox.Text;
            if(imagefilename != null)
            {
                empinfo.IMG = Converter.Instance.StreamFile(imagefilename);
            }
            DataProvider.Ins.DB.SaveChanges();
            LoadEmployee(EmPG);
            notifier.ShowSuccess("Edit Successfully");
            parameter.Close();
           
        }

        private void ClickEditEmployee(EmployeeControl parameter)
        {
            this.employeeControl = parameter;
            idEmp = parameter.tbNo.Text;
            showEditEmployee(idEmp);

        }

        private void showEditEmployee(string idEmp)
        {
            Editemp editemp = new Editemp();
            var item = DataProvider.Ins.DB.EMPLOYEEs.Find(idEmp);
            editemp.idBox.Text = item.EMP_ID;
            editemp.FullName.Text = item.EMP_NAME;
            if(item.GENDER.ToUpper()== "FEMALE")
            {
                editemp.genderBox.SelectedIndex = 0;
            }
            else editemp.genderBox.SelectedIndex=1;
            editemp.addressBox.Text = item.EMP_ADDRESS;
            editemp.dobBox.Text = item.EMP_DATE_OF_BIRTH.ToString();
            
            editemp.dowBox.Text = item.DATE_OF_WORK.ToString();
            editemp.dowBox.IsEnabled = false;
            editemp.phoneBox.Text = item.PHONE;
            if(item.EMP_TYPE.ToUpper() =="SALES")
            {
                editemp.positionBox.SelectedIndex = 0;
            }
            else
            {
                editemp.positionBox.SelectedIndex = 1;
            }
            if (item.IMG != null)
            {
                ImageBrush imageBrush = new ImageBrush();
                imageBrush.ImageSource = Converter.Instance.ToImage(item.IMG);
                editemp.grdSelectImage.Background = imageBrush;
            }
            editemp.ShowDialog();
        }

        private void DeleteEmployee(EmployeeControl parameter)
        {
            var item = DataProvider.Ins.DB.EMPLOYEEs.Find(parameter.tbNo.Text);
            item.IsDeleted = 1;
            DataProvider.Ins.DB.SaveChanges();
            LoadEmployee(empPG);
            notifier.ShowSuccess("Delete Successfully");
        }

        private void UploadImage(Grid parameter)
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

        private void AddEmployee(Addemp parameter)
        {
            if(parameter.addressBox.Text == null)
            {
                parameter.addressBox.Focus();
            }
            if (parameter.dobBox.Text == null)
            {
                parameter.dobBox.Focus();
            }
            if (parameter.phoneBox.Text == null)
            {
                parameter.phoneBox.Focus();
            }
            var emp = new ACCOUNT();
            emp.PASS = "123";
            emp.USERNAME = parameter.idBox.Text;
            if(parameter.positionBox.SelectedIndex == 0)
            {
                emp.TYPE_USER = 1;
            }
            else
            {
                emp.TYPE_USER = 2;
            }
            DataProvider.Ins.DB.ACCOUNTs.Add(emp);
            DataProvider.Ins.DB.SaveChanges();
            
           SqlConnection connection = new SqlConnection();
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["CarSalesSystem.Properties.Settings.CARSALESSYSTEMConnectionString"].ConnectionString;
            try
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"INSERT INTO EMPLOYEE (EMP_ID, EMP_ACCOUNT,EMP_NAME, GENDER, EMP_TYPE, EMP_ADDRESS, EMP_DATE_OF_BIRTH, DATE_OF_WORK,PHONE,IMG) 
                        VALUES(@EMP_ID, @EMP_ACCOUNT,@EMP_NAME, @GENDER, @EMP_TYPE, @EMP_ADDRESS, @EMP_DATE_OF_BIRTH, @DATE_OF_WORK, @PHONE,@IMG)";
                    command.Parameters.AddWithValue("@EMP_ID",parameter.idBox.Text);
                    command.Parameters.AddWithValue("@EMP_NAME", parameter.FullName.Text);
                    command.Parameters.AddWithValue("@EMP_ACCOUNT", parameter.idBox.Text);
                    command.Parameters.AddWithValue("@GENDER",parameter. genderBox.Text);
                    command.Parameters.AddWithValue("@EMP_TYPE",parameter. positionBox.Text);
                    command.Parameters.AddWithValue("@EMP_ADDRESS",parameter. addressBox.Text);
                    command.Parameters.AddWithValue("@EMP_DATE_OF_BIRTH",parameter. dobBox.SelectedDate.Value.Date.ToShortDateString());
                    command.Parameters.AddWithValue("@DATE_OF_WORK",parameter.dowBox.SelectedDate.Value.Date.ToShortDateString());
                    command.Parameters.AddWithValue("@PHONE",parameter. phoneBox.Text);
                    command.Parameters.AddWithValue("@IMG", Converter.Instance.StreamFile(imagefilename));
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

        private void LoadEmployee(EmployeePG parameter)
        {
            this.empPG = parameter;
            parameter.stkEmployee.Children.Clear();
            var listemp = DataProvider.Ins.DB.EMPLOYEEs.Where(x => x.IsDeleted != 1).ToList();
            foreach(var item in listemp)
            {
                EmployeeControl  empcontrol = new EmployeeControl();
                empcontrol.tbNo.Text = item.EMP_ID;
                empcontrol.tbName.Text = item.EMP_NAME;
                empcontrol.tbSex.Text = item.GENDER.ToString();
                empcontrol.tbPosition.Text = item.EMP_TYPE;
                parameter.stkEmployee.Children.Add(empcontrol);
            }
        }
    }
}
