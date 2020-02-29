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
using Point = System.Windows.Point;

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
        private List<Point> pixelOnCanvas = new List<Point>();
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
        particle = new Particle(canvas.ActualWidth, canvas.ActualHeight);


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


    }

    private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
        steps = Convert.ToInt32(slider.Value) * 5;

    }
}
}
