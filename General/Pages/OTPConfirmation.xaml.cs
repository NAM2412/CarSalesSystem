﻿using System;
using System.Collections.Generic;
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
using System.Windows.Threading;

namespace CarSalesSystem.General
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class OTPConfirmation : Page
    {
        private int time = 180;
        private DispatcherTimer Timer;
        
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
    }
}
