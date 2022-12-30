using CarSalesSystem.General.Windows;
using ControlzEx.Standard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CarSalesSystem.General
{
    /// <summary>
    /// Interaction logic for WpfMessageBox.xaml
    /// </summary>
    public partial class WpfMessageBox : Window
    {
        public WpfMessageBox()
        {
            InitializeComponent();

        }
        static WpfMessageBox _messageBox;
        protected MessageBoxResult _result = MessageBoxResult.No;
        private bool OTPvalidattion = false;
        void CloseWindow(Type type)
        {
            var window = App.Current.Windows.OfType<Window>().FirstOrDefault(w => w.GetType() == type);
            if (window != null)
                window.Close();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender == btnYes)
                _result = MessageBoxResult.Yes;
            else if (sender == btnNo)
                _result = MessageBoxResult.No;
            OTPConfirmation confirmation = new OTPConfirmation();
            if(_result == MessageBoxResult.Yes)
            {
                confirmation.Show();
                CloseWindow(typeof(SignUp));
                
                
            }
            CloseWindow(typeof(WpfMessageBox));

        }
        private void SigningUpNewAccount()
        {
            if(OTPvalidattion)
            {

            }
        }

    }
}
