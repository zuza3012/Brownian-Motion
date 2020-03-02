using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using Point = System.Windows.Point;

namespace BrownianMotion {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        Particle particle;
        bool stop = true;
        int steps, a = 300;
        private BackgroundWorker drawWorker = null;
        List<Point3D> pixelToDraw = new List<Point3D>();
        private List<Point3D> pixelOnCanvas = new List<Point3D>();
        double mx, my, azimuth = 0, elevation = 0;
        private List<Point3D> edges3D = new List<Point3D>();
        private List<Point> edges2D = new List<Point>();

         
        public MainWindow() {
            InitializeComponent();
            
        }

        protected override void OnClosed(EventArgs e) {
            base.OnClosed(e);
            Application.Current.Shutdown();
        }

        private void SaveGraph_Click(object sender, RoutedEventArgs e) {
        }

        private void Canvas_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            if (r2.IsChecked == true || stop == true)
                return;
            Console.WriteLine("down");
            mx = e.GetPosition(canvas).X;
            my = e.GetPosition(canvas).Y;

        }

        private void Canvas_PreviewMouseMove(object sender, System.Windows.Input.MouseEventArgs e) {
            if (r2.IsChecked == true || stop == true)
                return;
            Console.WriteLine("move");
            double new_mx = e.GetPosition(canvas).X;
            double new_my = e.GetPosition(canvas).Y;

            azimuth -= new_mx - mx;
            elevation += new_my - my;

            mx = new_mx;
            my = new_my;

            canvas.Children.Clear();
            DrawCube(a);
        }

        private void Canvas_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            if (r2.IsChecked == true || stop == true)
                return;
            Console.WriteLine("up");
        }

        private void Start_Click(object sender, RoutedEventArgs e) {

            if (r2.IsChecked == true ) {
                stop = false;
                canvas.Children.Clear();
                particle = new Particle(canvas.ActualWidth, canvas.ActualHeight, false);


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
            } else {
                
                TransformGroup group = new TransformGroup();
                group.Children.Add(new TranslateTransform(canvas.ActualWidth / 2, canvas.ActualHeight / 2));
                group.Children.Add(new ScaleTransform(1, 1));
                group.Children.Add(new RotateTransform(0));
                canvas.RenderTransform = group;

                DrawCube(a);
                DrawCircle();

            }
    }

    private void Stop_Click(object sender, RoutedEventArgs e) {
            if (r2.IsChecked == true) {
                drawWorker.CancelAsync();
                stop = true;
                btnStart.IsEnabled = true;
                btnStop.IsEnabled = false;
            } else {
                
            }


    }

    private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
        steps = Convert.ToInt32(slider.Value) * 5;

    }


}
}
