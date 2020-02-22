using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
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

        Particle particle;
        bool stop;
        int steps;
        private BackgroundWorker drawWorker = null;
        List<Point> pixelToDraw = new List<Point>();
        public MainWindow() {
            InitializeComponent(); 
        }

        protected override void OnClosed(EventArgs e) {
            base.OnClosed(e);
            Application.Current.Shutdown();
        }

        

        private void SaveGraph_Click(object sender, RoutedEventArgs e) {

        }

        private void Start_Click(object sender, RoutedEventArgs e) {
            stop = false;
            canvas.Children.Clear();
            particle = new Particle();
            particle.d = int.Parse(stepTb.Text);
            steps = Convert.ToInt32(slider.Value);
            Console.WriteLine("steps: " + steps);
              
            if (null == drawWorker) {
                drawWorker = new BackgroundWorker();
                drawWorker.DoWork += new DoWorkEventHandler(drawWorker_DoWork);
                drawWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(drawWorker_RunWorkerCompleted);
                drawWorker.ProgressChanged += new ProgressChangedEventHandler(drawWorker_ProgressChanged);
                drawWorker.WorkerReportsProgress = true;
                drawWorker.WorkerSupportsCancellation = true;
            }
            if (!drawWorker.IsBusy) {
                drawWorker.RunWorkerAsync();
            }
            

            btnStart.IsEnabled = false;
            btnStop.IsEnabled = true;

        }

        private void Stop_Click(object sender, RoutedEventArgs e) {
            drawWorker.CancelAsync();

            btnStart.IsEnabled = true;
            btnStop.IsEnabled = false;
            

        }


    }
}
