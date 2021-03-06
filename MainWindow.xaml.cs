﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows;
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
        bool captured = false, radio2 = false, radio3 = false;
        int a = 200;
        private BackgroundWorker drawWorker = null;
        double mx, my, azimuth = 0, elevation = 0, offX = 0, offY = 0, theta = 0, phi = 0, zoom = 1, tmpX = 0, tmpY = 0, tmpZ = 0;
        private List<Point3D> edges3D;
        PointCollection polygonPoints;
        Point3DCollection polygonPoints3D;
        private Point[] edges2D;
        float cosT = 0, sinT = 0, cosP = 0, sinP = 0, cosTcosP = 0, sinTsinP = 0, sinTcosP = 0, cosTsinP = 0;
        WriteableBitmap writeableBmp;
        HashSet<Point3D> hashset_points;

        public MainWindow() {
            InitializeComponent();
        }

        protected override void OnClosed(EventArgs e) {
            base.OnClosed(e);
            Application.Current.Shutdown();
        }

        private void SaveGraph_Click(object sender, RoutedEventArgs e) {
            Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.FileName = "Document"; 
            dialog.DefaultExt = ".png";
            dialog.Filter = "Image files (*.png) | *.png"; 

            // Show save file dialog box
            Nullable<bool> result = dialog.ShowDialog();

            // Process save file dialog box results
            if (result == true) {
                string filename = dialog.FileName;
                ControlToBmp(canvas, 96, 96).Save(dialog.FileName, System.Drawing.Imaging.ImageFormat.Png);
            } else {
                MessageBox.Show("File Save Error.");
            }
        }

        private void Canvas_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e) {

            if (r2.IsChecked == true)
                return;

            if (e.Delta > 0 && zoom < 1.6) {
                zoom += 0.05;
            } else if (e.Delta < 0 && Math.Abs(zoom) > 0 && zoom > 0.5) {
                zoom -= 0.05;

            }

            DrawCube(a, zoom, false);
        }

        private void Canvas_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            if (r2.IsChecked == true)
                return;
            mx = e.GetPosition(canvas).X;
            my = e.GetPosition(canvas).Y;
            captured = true;
        }


        private void Canvas_PreviewMouseMove(object sender, System.Windows.Input.MouseEventArgs e) {
            if (r2.IsChecked == true)
                return;

            if (captured) {
                double new_mx = e.GetPosition(canvas).X;
                double new_my = e.GetPosition(canvas).Y;

                azimuth -= new_mx - mx;
                elevation += new_my - my;

                mx = new_mx;
                my = new_my;

                DrawCube(a, zoom, false);
            }
        }

        private void Canvas_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            if (r2.IsChecked == true)
                return;
            captured = false;
        }



        private void Start_Click(object sender, RoutedEventArgs e) {
            btnStart.IsEnabled = false;
            btnStop.IsEnabled = true;

            canvas.Children.Clear();
            pgBar.Value = 0;

            if (r2.IsChecked == true) {
                writeableBmp = BitmapFactory.New(1010, 600);
               // writeableBmp.Clear(Colors.White);

                radio3 = false;
                radio2 = true;
                particle = new Particle(canvas.ActualWidth, canvas.ActualHeight, false);
            } else {
                edges3D = new List<Point3D>();
                radio2 = false;
                radio3 = true;
                polygonPoints = new PointCollection();
                polygonPoints3D = new Point3DCollection();
              
                // polygonPoints3D.Clear();
                //polygonPoints.Clear();

                particle = new Particle(canvas.ActualWidth, canvas.ActualHeight, true);

                DrawCube(a, zoom, true);
            }

            if (null == drawWorker) {
                drawWorker = new BackgroundWorker();
                drawWorker.DoWork += new DoWorkEventHandler(drawWorker_DoWork);
                drawWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(drawWorker_RunWorkerCompleted);
                drawWorker.ProgressChanged += new ProgressChangedEventHandler(drawWorker_ProgressChanged);
                drawWorker.WorkerReportsProgress = true;
                drawWorker.WorkerSupportsCancellation = true;
            }

            drawWorker.RunWorkerAsync();
        }

        private void Stop_Click(object sender, RoutedEventArgs e) {
            saveItem.IsEnabled = true;
            btnStart.IsEnabled = true;
            btnStop.IsEnabled = false;
            if ((null != drawWorker) && drawWorker.IsBusy) {
                drawWorker.CancelAsync();
            }           
        }   

        public static Bitmap ControlToBmp(Visual target, double dpiX, double dpiY) {
            if (target == null) {
                return null;
            }
            // render control content
            Rect bounds = VisualTreeHelper.GetDescendantBounds(target);
            RenderTargetBitmap rtb = new RenderTargetBitmap((int)(bounds.Width * dpiX / 96.0),
                                                            (int)(bounds.Height * dpiY / 96.0),
                                                            dpiX,
                                                            dpiY,
                                                            PixelFormats.Pbgra32);
            DrawingVisual dv = new DrawingVisual();
            using (DrawingContext ctx = dv.RenderOpen()) {
                VisualBrush vb = new VisualBrush(target);
                ctx.DrawRectangle(vb, null, new Rect(new System.Windows.Point(), bounds.Size));
            }
            rtb.Render(dv);

            MemoryStream stream = new MemoryStream();
            BitmapEncoder encoder = new BmpBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(rtb));
            encoder.Save(stream);
            return new Bitmap(stream);
        }

    }
}
