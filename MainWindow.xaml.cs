using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Point = System.Windows.Point;

namespace BrownianMotion {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        Particle particle;
        bool stop = true, captured = false, radio2 = false, radio3 = false;
        int steps, a = 200;
        private BackgroundWorker drawWorker = null;
        List<Point3D> pixelToDraw = new List<Point3D>();
        private List<Point3D> pixelOnCanvas = new List<Point3D>();
        double mx, my, azimuth = 0, elevation = 0, offX = 0, offY = 0, theta = 0, phi = 0, tmpX = 0, tmpY = 0, tmpZ = 0, zoom = 1;
        private List<Point3D> edges3D = new List<Point3D>();
        private List<Point> pointsOnLine = new List<Point>();
        private PointCollection polygonPoints = new PointCollection();
        private Point3DCollection polygonPoints3D = new Point3DCollection();
        private Point[] edges2D;
        //double zoom -> w mouseWheel czy w czymś zmieniaj to np od 0.5 do 2 -> przemnożyć przez to x i y
        float cosT = 0, sinT = 0, cosP = 0, sinP = 0, cosTcosP = 0, sinTsinP = 0, sinTcosP = 0, cosTsinP = 0;

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

            if (e.Delta > 0) {
                zoom += 0.05;
            } else if (e.Delta < 0 && Math.Abs(zoom) > 0) {
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
            Console.WriteLine("down!");
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
            stop = false;
            canvas.Children.Clear();
            polygonPoints3D.Clear();
            polygonPoints.Clear();

            stop = false;

            if (r2.IsChecked == true) {
                radio3 = false;
                radio2 = true;
                particle = new Particle(canvas.ActualWidth, canvas.ActualHeight, false);
            } else {
                radio2 = false;
                radio3 = true;

                particle = new Particle(canvas.ActualWidth, canvas.ActualHeight, true);
                DrawCube(a, zoom);
            }

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
            stop = true;
            btnStart.IsEnabled = true;
            btnStop.IsEnabled = false;

            stop = true;
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
            steps = Convert.ToInt32(slider.Value) * 5;

        }


    }
}
