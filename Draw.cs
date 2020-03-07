using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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
            Thread.Sleep(10);
            double height = canvas.ActualHeight;
            double width = canvas.ActualWidth;

            pixelToDraw.Clear();

            if (!drawWorker.CancellationPending) {

                if (radio2 == true) {
                    for (int i = 0; i < steps; i++) {

                        if (!pixelOnCanvas.Contains(particle.p)) {
                            pixelToDraw.Add(particle.p);
                            pixelOnCanvas.Add(particle.p);
                        }
                        particle.MoveParticle(width, height, false, a);

                    }
                } else if (radio3 == true) {
                    tmpX = particle.p.X;
                    tmpY = particle.p.Y;
                    tmpZ = particle.p.Z;

                    System.Console.WriteLine("poprezdni: " + "(" + tmpX + "," + tmpY + "," + tmpZ + ")");
                    particle.MoveParticle(width, height, true, a);
                    System.Console.WriteLine("obecny: " + "(" + particle.p.X + "," + particle.p.Y + "," + particle.p.Z + ")");

                }



            } else {
                e.Cancel = true;
                return;
            }


            drawWorker.ReportProgress(100);
        }

        void drawWorker_ProgressChanged(object sender, ProgressChangedEventArgs e) {
        }

        void drawWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {

            if (radio2 == true) {
                foreach (Point3D p in pixelToDraw) {
                    AddPixel(p.X, p.Y);
                }
                pixelToDraw.Clear();
            } else {

                DrawCube(a, zoom);

            }



            Thread.Sleep(10);
            if (stop) {
                drawWorker.CancelAsync();
                return;
            }
            drawWorker.RunWorkerAsync();

        }


    }
}
