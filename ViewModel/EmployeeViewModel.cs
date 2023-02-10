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
using System.Globalization;
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

        private SalaryOfEmployee wdsalaryOfEmp;
        public SalaryOfEmployee wdSalaryOfEmp { get => wdsalaryOfEmp; set => wdsalaryOfEmp = value; }
        public ICommand LoadEmployeeCommand { get; set; }
        public ICommand AddEmployeeCommand { get; set; }
        public ICommand UploadImageCommand { get; set; }
        public ICommand DeleteEmployeeCommand { get; set; }
        public ICommand EditEmployeeCommand { get; set; }
        public ICommand UpdateInfoEmpCommand { get; set; }
        public ICommand LoadFilterEmployeeCommand { get; set; }
        public ICommand  clickshowPaymentCommand { get; set; }
        public ICommand PaySalaryCompleteCommand { get; set; }
        public ICommand calculatePayCommand { get; set; }
        public EmployeeViewModel()
        {
            LoadEmployeeCommand = new RelayCommand<EmployeePG>((parameter) => true, (parameter) => LoadEmployee(parameter));
            AddEmployeeCommand = new RelayCommand<Addemp>((parameter) => true, (parameter) => AddEmployee(parameter));
            UploadImageCommand = new RelayCommand<Grid>((parameter) => true, (parameter) => UploadImage(parameter));
            DeleteEmployeeCommand = new RelayCommand<EmployeeControl>((parameter) => true, (parameter) => DeleteEmployee(parameter));
            EditEmployeeCommand = new RelayCommand<EmployeeControl>((parameter) => true, (parameter) => ClickEditEmployee(parameter));
            UpdateInfoEmpCommand = new RelayCommand<Editemp>((parameter) => true, (parameter) => UpdateInfoEmp(parameter));
            LoadFilterEmployeeCommand = new RelayCommand<EmployeePG>((parameter) => true, (parameter) => LoadFilterEmployee(parameter));
            clickshowPaymentCommand = new RelayCommand<EmployeeControl>((parameter) => true, (parameter) => clickshowPayment(parameter));
            PaySalaryCompleteCommand = new RelayCommand<SalaryOfEmployee>((parameter) => true, (parameter) => PaySalaryComplete(parameter));
            calculatePayCommand = new RelayCommand<SalaryOfEmployee>((parameter) => true, (parameter) => calculatePay(parameter));
        }

        private void calculatePay(SalaryOfEmployee parameter)
        {
            var emp = DataProvider.Ins.DB.EMPLOYEEs.Find(idEmp);
            decimal sa = 0;
            if (wdsalaryOfEmp.SalaryBox.Text == "")
            {
                sa = 0;
            }
            else
             sa = Decimal.Parse(wdsalaryOfEmp.SalaryBox.Text);
            int dateofW = 0;
            if (wdsalaryOfEmp.NumofdayBox.Text == "")
            {
                 dateofW = 0;
            }
            else
            dateofW = int.Parse(wdsalaryOfEmp.NumofdayBox.Text);
            decimal total = (decimal)(sa / 26 * dateofW);
            wdsalaryOfEmp.TotalSalaryBox.Text = total.ToString("C", CultureInfo.CurrentCulture);
        }

        private void PaySalaryComplete(SalaryOfEmployee parameter)
        {
            var emp = DataProvider.Ins.DB.EMPLOYEEs.Find(idEmp);
            emp.DateOfWork = 0;
            emp.SALARY = Decimal.Parse(parameter.SalaryBox.Text);
            DataProvider.Ins.DB.SaveChanges();
            LoadEmployee(EmPG);
            parameter.Close();
            notifier.ShowInformation("Pay Salary Successfully");
            
        }

        private void clickshowPayment(EmployeeControl parameter)
        {
            this.employeeControl = parameter;
            idEmp = parameter.tbNo.Text;
            showPaymentWindow(idEmp);

        }

        private void showPaymentWindow(string idEmp)
        {
            SalaryOfEmployee salary = new SalaryOfEmployee();
            this.wdsalaryOfEmp = salary;
            var emp = DataProvider.Ins.DB.EMPLOYEEs.Find(idEmp);
            int countproductsell = DataProvider.Ins.DB.SELLBILLs.Where(x => x.EMPLOYEE_ID == idEmp).Count();
            if (emp.EMP_TYPE.ToUpper() == "SALES")
            {
                salary.typeComboBox.SelectedIndex = 0;
            }
            else
            {
                salary.typeComboBox.SelectedIndex = 1;
            }
            if (emp.SALARY != null)
            {
                salary.SalaryBox.Text = emp.SALARY.ToString();
            }
            else
            {
                salary.SalaryBox.Text = 0.ToString();
            }
            salary.NumofdayBox.Text = emp.DateOfWork.ToString();
            int dateofW = int.Parse(salary.NumofdayBox.Text);
            decimal sa = Decimal.Parse(salary.SalaryBox.Text);
            decimal total = (decimal)(sa / 26 * dateofW);
            salary.TotalSalaryBox.Text = total.ToString("C", CultureInfo.CurrentCulture);
            salary.ShowDialog();
        }

        private void LoadFilterEmployee(EmployeePG parameter)
        {
            this.empPG = parameter;
            parameter.stkEmployee.Children.Clear();
            if (parameter.tbFindEmployee.Text.Trim().Length == 0) LoadEmployee(EmPG);
            var listemp = DataProvider.Ins.DB.EMPLOYEEs.Where(x => x.EMP_NAME.ToUpper().StartsWith( parameter.tbFindEmployee.Text.ToUpper())).ToList();
            foreach (var item in listemp)
            {
                EmployeeControl empcontrol = new EmployeeControl();
                empcontrol.tbNo.Text = item.EMP_ID;
                empcontrol.tbName.Text = item.EMP_NAME;
                empcontrol.tbSex.Text = item.GENDER.ToString();
                empcontrol.tbPosition.Text = item.EMP_TYPE;
                if (AccountInfo.Type_User == -1)
                {
                    empcontrol.btnpayment.Visibility = Visibility.Visible;
                }
                else empcontrol.btnpayment.Visibility = Visibility.Collapsed;
                if (item.DateOfWork == 26)
                {
                    empcontrol.iconbtnpayment.Foreground = Brushes.Green;
                }
                parameter.stkEmployee.Children.Add(empcontrol);
            }
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
            empinfo.SALARY = Decimal.Parse(parameter.tbSalary.Text);
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
            if (item.SALARY != null)
            {
                editemp.tbSalary.Text = item.SALARY.ToString();
            }
            else
                editemp.tbSalary.Text = "0";
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
            if(parameter.addressBox.Text.Length == 0)
            {
                parameter.addressBox.Focus();
            }
            else
            if (parameter.dobBox.Text.Length == 0)
            {
                parameter.dobBox.Focus();
            }
            else
            if (parameter.phoneBox.Text.Length == 0)
            {
                parameter.phoneBox.Focus();
            }
            else
            if(parameter.FullName.Text.Length == 0)
            {
                parameter.FullName.Focus();
            }
            else
            if (parameter.positionBox.SelectedIndex == -1)
            {
                parameter.positionBox.Focus();
            }
            else
            if (parameter.genderBox.SelectedIndex == -1)
            {
                parameter.genderBox.Focus();
            }
            else
            if (parameter.tbSalary.Text.Length ==0)
            {
                parameter.tbSalary.Focus();
            }
            else
            {
                var emp = new ACCOUNT();
                emp.PASS = "123";
                emp.USERNAME = parameter.idBox.Text;
                if (parameter.positionBox.SelectedIndex == 0)
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
                connection.ConnectionString = ConfigurationManager.ConnectionStrings["NamConnection"].ConnectionString;
                try
                {
                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = @"INSERT INTO EMPLOYEE (EMP_ID, EMP_ACCOUNT,EMP_NAME, GENDER, EMP_TYPE, EMP_ADDRESS, EMP_DATE_OF_BIRTH, DATE_OF_WORK,PHONE,IMG,DateOfWork,SALARY) 
                        VALUES(@EMP_ID, @EMP_ACCOUNT,@EMP_NAME, @GENDER, @EMP_TYPE, @EMP_ADDRESS, @EMP_DATE_OF_BIRTH, @DATE_OF_WORK, @PHONE,@IMG,@DateOfWork,@SALARY)";
                        command.Parameters.AddWithValue("@EMP_ID", parameter.idBox.Text);
                        command.Parameters.AddWithValue("@EMP_NAME", parameter.FullName.Text);
                        command.Parameters.AddWithValue("@EMP_ACCOUNT", parameter.idBox.Text);
                        command.Parameters.AddWithValue("@GENDER", parameter.genderBox.Text);
                        command.Parameters.AddWithValue("@EMP_TYPE", parameter.positionBox.Text);
                        command.Parameters.AddWithValue("@EMP_ADDRESS", parameter.addressBox.Text);
                        command.Parameters.AddWithValue("@EMP_DATE_OF_BIRTH", parameter.dobBox.SelectedDate.Value.Date.ToShortDateString());
                        command.Parameters.AddWithValue("@DATE_OF_WORK", parameter.dowBox.SelectedDate.Value.Date.ToShortDateString());
                        command.Parameters.AddWithValue("@PHONE", parameter.phoneBox.Text);
                        command.Parameters.AddWithValue("@IMG", Converter.Instance.StreamFile(imagefilename));
                        command.Parameters.AddWithValue("@DateOfWork", 0);
                        command.Parameters.AddWithValue("@SALARY",Decimal.Parse(parameter.tbSalary.Text));
                        command.ExecuteNonQuery();
                        LoadEmployee(EmPG);
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
                    parameter.Close();
                    connection.Close();
                }
            }
         
        }

        private void LoadEmployee(EmployeePG parameter)
        {
            this.empPG = parameter;
            parameter.stkEmployee.Children.Clear();
            var listemp = DataProvider.Ins.DB.EMPLOYEEs.Where(x => x.IsDeleted != 1 && x.ACCOUNT.TYPE_USER!= -1).ToList();
            foreach(var item in listemp)
            {
                EmployeeControl  empcontrol = new EmployeeControl();
                empcontrol.tbNo.Text = item.EMP_ID;
                empcontrol.tbName.Text = item.EMP_NAME;
                empcontrol.tbSex.Text = item.GENDER.ToString();
                empcontrol.tbPosition.Text = item.EMP_TYPE;
                if (item.DateOfWork == 26)
                {
                    empcontrol.iconbtnpayment.Foreground = Brushes.Green;
                }
                if (AccountInfo.Type_User == -1)
                {
                    empcontrol.btnpayment.Visibility = Visibility.Visible;
                }
                else empcontrol.btnpayment.Visibility = Visibility.Collapsed;
                parameter.stkEmployee.Children.Add(empcontrol);
            }
        }
    }
}
