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
        bool stop = true, captured = false;
        int steps, a = 200;
        private BackgroundWorker drawWorker = null;
        List<Point3D> pixelToDraw = new List<Point3D>();
        private List<Point3D> pixelOnCanvas = new List<Point3D>();
        double mx, my, azimuth = 0, elevation = 0, zoom = 1;
        private List<Point3D> edges3D = new List<Point3D>();
        private Point[] edges2D;
        //double zoom -> w mouseWheel czy w czymś zmieniaj to np od 0.5 do 2 -> przemnożyć przez to x i y
         
        public MainWindow() {
            InitializeComponent();
            
        }

        protected override void OnClosed(EventArgs e) {
            base.OnClosed(e);
            Application.Current.Shutdown();
        }

        private void SaveGraph_Click(object sender, RoutedEventArgs e) {
        }

        private void Canvas_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e) {
            
            if (r2.IsChecked == true || stop == true)
                return;

            if(e.Delta > 0) {
                zoom += 0.05;
            }else if(e.Delta < 0 && Math.Abs(zoom) > 0) {
                zoom -= 0.05;
                
            } else {
                zoom = 1;
            }
            Console.WriteLine("Delta: " + e.Delta);
            Console.WriteLine("zoom: " + zoom);

            DrawCube(a, zoom);
        }

        private void Canvas_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            if (r2.IsChecked == true || stop == true)
                return;
            mx = e.GetPosition(canvas).X;
            my = e.GetPosition(canvas).Y;
            captured = true;
        }


        private void Canvas_PreviewMouseMove(object sender, System.Windows.Input.MouseEventArgs e) {
            if (r2.IsChecked == true || stop == true)
                return;

            if (captured) {
                double new_mx = e.GetPosition(canvas).X;
                double new_my = e.GetPosition(canvas).Y;

                azimuth -= new_mx - mx;
                elevation += new_my - my;

                mx = new_mx;
                my = new_my;

                DrawCube(a, zoom);
            }
        }

        private void Canvas_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            if (r2.IsChecked == true || stop == true)
                return;
            captured = false;
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
                stop = false;
               

                DrawCube(a, zoom);
                

            }
    }

    private void Stop_Click(object sender, RoutedEventArgs e) {
            if (r2.IsChecked == true) {
                drawWorker.CancelAsync();
                stop = true;
                btnStart.IsEnabled = true;
                btnStop.IsEnabled = false;
            } else {
                stop = true;
            }


    }

    private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
        steps = Convert.ToInt32(slider.Value) * 5;

    }


}
}
