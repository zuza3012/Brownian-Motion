using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace BrownianMotion {
    public partial class MainWindow : Window {

        private void AddPixel(double x, double y) {
            Rectangle rec = new Rectangle();
            Canvas.SetTop(rec, y);
            Canvas.SetLeft(rec, x);
            rec.Width = 1;
            rec.Height = 1;
            rec.Fill = new SolidColorBrush(Colors.Black);
            canvas.Children.Add(rec);
        }

        void drawWorker_DoWork(object sender, DoWorkEventArgs e) {
            double height = canvas.ActualHeight;
            double width = canvas.ActualWidth;
            int max = 500;

            hashset_points = new HashSet<Point3D>();
            if (radio2 == true) {
                max = 90000;
            }
            //var s2 = Stopwatch.StartNew();
            for (int nCounter = 1; nCounter <= max; ++nCounter) {
                if (drawWorker.CancellationPending) {
                    e.Cancel = true;
                    break;
                }
                wait_less(0.000033);

                if (radio3 == true) {
                    tmpX = particle.p.X;
                    tmpY = particle.p.Y;
                    tmpZ = particle.p.Z;
                    particle.MoveParticle(width, height, true, a, 20);
                }


                if (radio2 == true) {
                    particle.MoveParticle(width, height, false, a, 20);
                    if (!hashset_points.Contains(particle.p)) {
                        hashset_points.Add(particle.p);
                    }

                }
                if (radio3 == true) {
                    Thread.Sleep(10);
                }

                drawWorker.ReportProgress((int)System.Math.Floor(nCounter * 100.0 / (double)max));
            }
            //s2.Stop();
            //System.Console.WriteLine(s2.Elapsed.TotalMilliseconds.ToString("0.000 ms"));

        }

        void drawWorker_ProgressChanged(object sender, ProgressChangedEventArgs e) {

            if (radio3 == true) {
                DrawCube(a, zoom, true);
            }
            pgBar.Value = e.ProgressPercentage;
        }

        void drawWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {

            if (radio2 == true) {
                using (writeableBmp.GetBitmapContext()) {
                    //var s3 = Stopwatch.StartNew();
                    foreach (Point3D point in hashset_points) {
                        writeableBmp.SetPixel((int)point.X, (int)point.Y, Colors.Black);
                    }
                    hashset_points.Clear();
                    //s3.Stop();
                    //System.Console.WriteLine(s3.Elapsed.TotalMilliseconds.ToString("0.000 ms"));
                }

                Image image = new Image();
                image.Source = writeableBmp;
                canvas.Children.Add(image);
            }
            if (e.Cancelled) {
                MessageBox.Show("Process has been stopped.");
            } else {
                MessageBox.Show("Done :)");
            }
            btnStart.IsEnabled = true;
            btnStop.IsEnabled = false;

        }

        private static void wait_less(double durationSeconds) {
            var durationTicks = System.Math.Round(durationSeconds * Stopwatch.Frequency);
            var sw = Stopwatch.StartNew();

            while (sw.ElapsedTicks < durationTicks) {
            }
        }

    }
}
