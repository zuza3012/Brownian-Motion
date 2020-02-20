using System;
using System.Collections.Generic;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace BrownianMotion {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        
        double T, eta, r0;
        private const double kB = 1.38E-23;
        int d;
        Particle particle, room;

        public MainWindow() {
            InitializeComponent();

            CreateObject();
        }

        protected override void OnClosed(EventArgs e) {
            base.OnClosed(e);
            Application.Current.Shutdown();
        }

        
        private void SaveGraph_Click(object sender, RoutedEventArgs e) {

        }

        private void Start_Click(object sender, RoutedEventArgs e) {
            r0 = double.Parse(radiusTb.Text) * System.Math.Pow(10, -6);
            d = int.Parse(dTb.Text);

            if (water0Rbtn.IsChecked == true) {
                T = 273;
                eta = 1.79E-03;
                label.Content = "Water 0°C";
            } else if (water100Rbtn.IsChecked == true) {
                T = 373;
                eta = 0.28E-03;
                label.Content = "Water 100°C";
            } else if (water25Rbtn.IsChecked == true) {
                T = 298;
                eta = 0.89E-03;
                label.Content = "Water 25°C";
            } else if (glicRbtn.IsChecked == true) {
                T = 298;
                eta = 934E-03;
                label.Content = "Gliceryn 25°C";
            } else if (ethanolRbtn.IsChecked == true) {
                T = 298;
                eta = 1.07E-03;
                label.Content = "Ethanol 25°C";
            } else if (PhenolRbtn.IsChecked == true) {
                T = 318;
                eta = 4.036E-06;
                label.Content = "Phenol 45°C";
            } else if (HydrodenRbtn.IsChecked == true) {
                T = 273;
                eta = 8.35E-06;
                label.Content = "Hydrogen 0°C";
            } else {
                //default is Air
                T = 273;
                eta = 17.08E-06;
                label.Content = "Air 0°C";
            }

            water0Rbtn.IsEnabled = false;
            water25Rbtn.IsEnabled = false;
            water100Rbtn.IsEnabled = false;
            glicRbtn.IsEnabled = false;
            ethanolRbtn.IsEnabled = false;
            PhenolRbtn.IsEnabled = false;
            AirRbtn.IsEnabled = false;
            HydrodenRbtn.IsEnabled = false;
            radiusTb.IsEnabled = false;
            angleTb.IsEnabled = false;

            /* particle = new Particle();
             particle.D = kB * T / (3.14 * 6 * eta * r0);
             particle.d = d;*/

        }

        private void Stop_Click(object sender, RoutedEventArgs e) {
            water0Rbtn.IsEnabled = true;
            water25Rbtn.IsEnabled = true;
            water100Rbtn.IsEnabled = true;
            glicRbtn.IsEnabled = true;
            ethanolRbtn.IsEnabled = true;
            PhenolRbtn.IsEnabled = true;
            AirRbtn.IsEnabled = true;
            HydrodenRbtn.IsEnabled = true;
            radiusTb.IsEnabled = true;
            angleTb.IsEnabled = true;

        }


    }
}
