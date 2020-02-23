using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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

            int counter = 0;
            pixelToDraw.Clear();

            if (!drawWorker.CancellationPending) {

                for (int i = 0; i < steps ; i++) {

                    if (!pixelOnCanvas.Contains(particle.p)) {
                        pixelToDraw.Add(particle.p);
                        pixelOnCanvas.Add(particle.p);
                    }

                    System.Console.WriteLine("(" + particle.p.X + "," + particle.p.Y + ")");
                    System.Console.WriteLine(counter);
                    particle.MoveParticle();

                    if (particle.p.X >= width) {
                        particle.p.X = particle.p.X - width;
                    } else {
                        particle.p.X = particle.p.X + width;
                    }

                    if (particle.p.Y >= height) {
                        particle.p.Y = particle.p.Y - height;
                    } else {
                        particle.p.Y = particle.p.Y + height;
                    }
                    counter++;
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
            foreach (Point p in pixelToDraw) {
                AddPixel(p.X, p.Y);
            }
            pixelToDraw.Clear();
            //System.Console.WriteLine("sleep: "+ time);
            Thread.Sleep(10);
            if (stop) {
                drawWorker.CancelAsync();
                return;
            }
            drawWorker.RunWorkerAsync(); // This will make the BgWorker run again, and never runs before it is completed.

        }


    }
}
