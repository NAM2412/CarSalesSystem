using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace CarSalesSystem.General.Windows
{
    /// <summary>
    /// Interaction logic for OTPConfirmation.xaml
    /// </summary>
    public partial class OTPConfirmation : Window
    {
        private static readonly Regex _regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text
        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }
        private bool OTPSuccess = false;
        private int time = 180;
        private DispatcherTimer Timer;
        private int retryTimes = 2;

        public OTPConfirmation()
        {
            InitializeComponent();
            Timer = new DispatcherTimer();
            Timer.Interval = new TimeSpan(0, 0, 1);
            Timer.Tick += Timer_Tick;
            Timer.Start();
        }
        void Timer_Tick(object sender, EventArgs e)
        {
            if (time > 0)
            {
                if (time <= 10)
                {
                    TBCountDown.Foreground = Brushes.Red;
                }
                time--;
                TBCountDown.Text = string.Format("{0}:{1}", time / 60, time % 60);

            }
            else
                Timer.Stop();
        }

        private void PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            retryTimes--;
            if(retryTimes == 0)
            {
                MessageBox.Show("You have requested OTP code too many times!", "Warning", MessageBoxButton.OK,MessageBoxImage.Warning);
                Timer.Stop();
                this.Close();
                return;
            }
            time = 180;
        }
        private void OpenAddInformationWindow()
        {
            FillingInformation fillingInformationWindow = new FillingInformation();
            if (OTPSuccess)
            {
                fillingInformationWindow.Show();
                this.Close();
                return;
            }
        }
    }
}
