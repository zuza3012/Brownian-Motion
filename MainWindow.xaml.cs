using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
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

namespace BrownianMotion {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }

        //private bool start = false, stop = false;
        double T, eta, r0;
        private const double kB = 1.38E-23;
        int steps, d;

        public SeriesCollection seriesCol { get; set; }
        List<int> xValues, yValues;
        ChartValues<ObservablePoint> ListOfPoints = new ChartValues<ObservablePoint>();

        protected override void OnClosed(EventArgs e) {
            base.OnClosed(e);
            Application.Current.Shutdown();
            Console.WriteLine("Application has been closed");
        }

        private void SaveGraph_Click(object sender, RoutedEventArgs e) {
            
        }

        private void Start_Click(object sender, RoutedEventArgs e) {
            stop = false;
            start = true;
            r0 = double.Parse(radiusTb.Text) * Math.Pow(10, -6);
            steps = int.Parse(stepsTb.Text);
            d = int.Parse(dTb.Text);
            Console.WriteLine("steps: " + steps);

            if (water0Rbtn.IsChecked == true) {
                T = 273;
                eta = 1.79E-03;
                label.Content = "Water 0°C";
            } else if(water100Rbtn.IsChecked == true) {
                T = 373;
                eta = 0.28E-03;
                label.Content = "Water 100°C";
            } else if(water25Rbtn.IsChecked == true) {
                T = 298;
                eta = 0.89E-03;
                label.Content = "Water 25°C";
            } else if(glicRbtn.IsChecked == true) {
                T = 298;
                eta = 934E-03;
                label.Content = "Gliceryn 25°C";
            } else if(ethanolRbtn.IsChecked == true) {
                T = 298;
                eta = 1.07E-03;
                label.Content = "Ethanol 25°C";
            } else if(PhenolRbtn.IsChecked == true) {
                T = 318;
                eta = 4.036E-06;
                label.Content = "Phenol 45°C";
            } else if(HydrodenRbtn.IsChecked == true) {
                T = 273;
                eta = 8.35E-06;
                label.Content = "Hydrogen 0°C";
            } else{ 
                //default is Air
                T = 273;
                eta = 17.08E-06;
                label.Content = "Air 0°C";
            }

           // Console.WriteLine("r0: " + r0);
          //  Console.WriteLine("eta: " + eta);
            //Console.WriteLine("T: " + T);

            water0Rbtn.IsEnabled = false;
            water25Rbtn.IsEnabled = false;
            water100Rbtn.IsEnabled = false;
            glicRbtn.IsEnabled = false;
            ethanolRbtn.IsEnabled = false;
            PhenolRbtn.IsEnabled = false;
            AirRbtn.IsEnabled = false;
            HydrodenRbtn.IsEnabled = false;
            radiusTb.IsEnabled = false;
            stepsTb.IsEnabled = false;

            Particle particle = new Particle();
            particle.D = kB * T / (3.14 * 6 * eta * r0);
            particle.d = d;
            // Console.WriteLine("particle.D: " + particle.D);

            particle.MoveParticle(steps);

            foreach(double item in particle.xLocations) {
                Console.WriteLine(item);
            }

            MakeGraph(particle);
            //Console.WriteLine("ilosc serii: " + chart.Series.Count()); 
        }

        private void Stop_Click(object sender, RoutedEventArgs e) {
            start = false;
            stop = true;

            // do czyszczenia wykresu - NA RAZIE DZIALA RAZ
            bool collIsBusy = false;
            try {
                if (seriesCol != null && seriesCol.Count > 0 && !collIsBusy) {
                    collIsBusy = true; //flag to prevent throttling multiple executions
                    seriesCol.Clear();
                    collIsBusy = false;
                }
            } catch (Exception ex) {
                Console.WriteLine("Error in clearing SectionsCollection.\n" + ex);     
            }
            seriesCol = new SeriesCollection();


            water0Rbtn.IsEnabled = true;
            water25Rbtn.IsEnabled = true;
            water100Rbtn.IsEnabled = true;
            glicRbtn.IsEnabled = true;
            ethanolRbtn.IsEnabled = true;
            PhenolRbtn.IsEnabled = true;
            AirRbtn.IsEnabled = true;
            HydrodenRbtn.IsEnabled = true;
            radiusTb.IsEnabled = true;
            stepsTb.IsEnabled = true;

        }

        private void MakeGraph(Particle p) {

            xValues = new List<int>(p.xLocations);
            yValues = new List<int>(p.yLocations);

            for (int i = 0; i < p.xLocations.Count; i++) {
                ListOfPoints.Add(new ObservablePoint {
                    X = xValues[i],
                    Y = yValues[i]
                });
            }

            seriesCol = new SeriesCollection {
                  new LineSeries{
                     Values = ListOfPoints,
                     LineSmoothness = 0,
                     //PointGeometry = DefaultGeometries.Circle, // DefaultGeometries.Square tylko jak jest malo punktuf, inzcaej kupcia
                     PointGeometrySize = 0,
                     //Title = chartTitle,
                     Fill = System.Windows.Media.Brushes.White,
                   }
                  
             };

            chart.AxisX.Add(
            new Axis {
                MinValue = p.xLocations.Min(),
                MaxValue = p.xLocations.Max(),
                Title = "x",
            }) ;

            
            chart.AxisY.Add(
            new Axis {
                MinValue = p.yLocations.Min(),
                MaxValue = p.yLocations.Max(),
                Title = "y",

            });
            //chart.Background = System.Windows.Media.Brushes.White;
            //chart.AxisX[0].Separator.StrokeThickness = 0;
            //chart.AxisY[0].Separator.StrokeThickness = 0;
           // chart.AxisY[0].Separator.IsEnabled = true;

            chart.DataContext = this;
            
        }
    }
}
