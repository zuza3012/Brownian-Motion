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

        /* private void Draw() {

             int counter = 0;
             for (int i = 0; i < 14; i++) {
                 System.Console.WriteLine("w forze");
                 System.Console.WriteLine("(" + particle.p.X + "," + particle.p.Y + ")");
                 System.Console.WriteLine(counter);
                 particle.MoveParticle();
                 counter++;
             }

         }*/

        void drawWorker_DoWork(object sender, DoWorkEventArgs e) {
            Thread.Sleep(1000);
            int counter = 0;
            pixelToDraw.Clear();
            System.Console.WriteLine("ilosc elemnetuf: " + pixelToDraw.Count);

            for (int i = 0; i < 1000; i++) {
                pixelToDraw.Add(particle.p);
                System.Console.WriteLine("(" + particle.p.X + "," + particle.p.Y + ")");
                System.Console.WriteLine(counter);
                particle.MoveParticle();
                counter++;

            }
            if (drawWorker.CancellationPending) {
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

            drawWorker.RunWorkerAsync(); // This will make the BgWorker run again, and never runs before it is completed.

        }


    }
}
