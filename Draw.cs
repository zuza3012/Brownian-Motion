﻿using System.ComponentModel;
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
            Thread.Sleep(10);
            double height = canvas.ActualHeight;
            double width = canvas.ActualWidth;

            pixelToDraw.Clear();

            if (!drawWorker.CancellationPending) {

                for (int i = 0; i < steps ; i++) {

                    if (!pixelOnCanvas.Contains(particle.p)) {
                        pixelToDraw.Add(particle.p);
                        pixelOnCanvas.Add(particle.p);
                    }
                    particle.MoveParticle(width, height);
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

            Thread.Sleep(10);
            if (stop) {
                drawWorker.CancelAsync();
                return;
            }
            drawWorker.RunWorkerAsync(); 

        }


    }
}
