using System;
using System.Collections.Generic;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace BrownianMotion {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            
            foreach (Shape control in canvas.Children)
            {
                control.PreviewMouseLeftButtonDown += this.MouseLeftButtonDown;
                control.PreviewMouseLeftButtonUp += this.PreviewMouseLeftButtonUp;
                control.Cursor = Cursors.Hand;
            }

            // Setting the MouseMove event for our parent control(In this case it is canvas).
            canvas.PreviewMouseMove += this.MouseMove;
            canvas.MouseLeftButtonUp += this.MouseLeftButtonUp;

        }

        double T, eta, r0;
        private const double kB = 1.38E-23;
        int d;
        Particle particle, room;
        double FirstXPos, FirstYPos, endXPos, endYPos, azimuth = 0, elevation = 0;
       
        bool captured = false;
        double x_shape, x_canvas, y_shape, y_canvas;
        UIElement source = null;


        void PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
            //tego nie musi byc chyba
            Console.WriteLine("Preview");
        }

        private void MouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
            Mouse.Capture(null);
            captured = false;
        }

        private void MouseMove(object sender, MouseEventArgs e) {

            if (captured) {
                double x = e.GetPosition(canvas).X;
                double y = e.GetPosition(canvas).Y;
                x_shape += x - x_canvas;
                Canvas.SetLeft(source, x_shape);
                x_canvas = x;
                y_shape += y - y_canvas;
                Canvas.SetTop(source, y_shape);
                y_canvas = y;
            }

            /*if (e.LeftButton == MouseButtonState.Pressed) {
                canvas.Children.Clear();

                // We start to moving objects with setting the lines positions.
                endXPos = e.GetPosition(sender as Control).X;
                endYPos = e.GetPosition(sender as Control).Y;

                Console.WriteLine("Moved");
                Console.WriteLine("end: " + "(" + endXPos + ", " + endYPos + ")");

                room = new Particle();


               
                azimuth -= endXPos - FirstXPos;
                elevation += endYPos - FirstYPos;

                room.angle2 = azimuth * 2 * 3.14 / 360;
                Console.WriteLine("angle2: " + room.angle2);

               
                
                room.MakeARoom();
                Draw();

                FirstYPos = endXPos;
                FirstYPos = endYPos;
            }*/
        }
       
        private void MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            /* //In this event, we get current mouse position on the control to use it in the MouseMove event.
             FirstXPos = e.GetPosition(sender as Control).X;
             FirstYPos = e.GetPosition(sender as Control).Y;

             Console.WriteLine("first: " + "(" + FirstXPos + ", " + FirstYPos + ")");
             Console.WriteLine("Left");*/

            source = (UIElement)sender;
            Mouse.Capture(source);
            captured = true;
            x_shape = Canvas.GetLeft(source);
            x_canvas = e.GetPosition(canvas).X;
            y_shape = Canvas.GetTop(source);
            y_canvas = e.GetPosition(canvas).Y;


        }


        protected override void OnClosed(EventArgs e) {
            base.OnClosed(e);
            Application.Current.Shutdown();
        }

        private void SetAngle_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
            canvas.Children.Clear();
            var slider = sender as Slider;
            room = new Particle();
            room.angle2 = slider.Value * 2 * 3.14 / 360;

            room.MakeARoom();
            Draw();
        }

        

        /*private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            pLeftButtonDown = e.GetPosition(canvas);
            Console.WriteLine("(x,y) LeftcosTam: " + "(" + pLeftButtonDown.X + ", " + pLeftButtonDown.Y + ")");
        }

        private void Border_Drop(object sender, DragEventArgs e) {
            pUp = e.GetPosition(canvas);
            Console.WriteLine("(x,y) Up: " + "(" + pUp.X + ", " + pUp.Y + ")");
        }*/

        private void SaveGraph_Click(object sender, RoutedEventArgs e) {

        }

        private void Start_Click(object sender, RoutedEventArgs e) {
            r0 = double.Parse(radiusTb.Text) * Math.Pow(10, -6);
            d = int.Parse(dTb.Text);


            if (water0Rbtn.IsChecked == true) {
                T = 273;
                eta = 1.79E-03;
                label.Content = "Water 0°C";
            } else if (water100Rbtn.IsChecked == true) {
                T = 373;
                eta = 0.28E-03;
                label.Content = "Water 100°C";
            } else if (water25Rbtn.IsChecked == true) {
                T = 298;
                eta = 0.89E-03;
                label.Content = "Water 25°C";
            } else if (glicRbtn.IsChecked == true) {
                T = 298;
                eta = 934E-03;
                label.Content = "Gliceryn 25°C";
            } else if (ethanolRbtn.IsChecked == true) {
                T = 298;
                eta = 1.07E-03;
                label.Content = "Ethanol 25°C";
            } else if (PhenolRbtn.IsChecked == true) {
                T = 318;
                eta = 4.036E-06;
                label.Content = "Phenol 45°C";
            } else if (HydrodenRbtn.IsChecked == true) {
                T = 273;
                eta = 8.35E-06;
                label.Content = "Hydrogen 0°C";
            } else {
                //default is Air
                T = 273;
                eta = 17.08E-06;
                label.Content = "Air 0°C";
            }

            water0Rbtn.IsEnabled = false;
            water25Rbtn.IsEnabled = false;
            water100Rbtn.IsEnabled = false;
            glicRbtn.IsEnabled = false;
            ethanolRbtn.IsEnabled = false;
            PhenolRbtn.IsEnabled = false;
            AirRbtn.IsEnabled = false;
            HydrodenRbtn.IsEnabled = false;
            radiusTb.IsEnabled = false;
            angleTb.IsEnabled = false;




            /* particle = new Particle();
             particle.D = kB * T / (3.14 * 6 * eta * r0);
             particle.d = d;*/




        }

        private void Stop_Click(object sender, RoutedEventArgs e) {
            water0Rbtn.IsEnabled = true;
            water25Rbtn.IsEnabled = true;
            water100Rbtn.IsEnabled = true;
            glicRbtn.IsEnabled = true;
            ethanolRbtn.IsEnabled = true;
            PhenolRbtn.IsEnabled = true;
            AirRbtn.IsEnabled = true;
            HydrodenRbtn.IsEnabled = true;
            radiusTb.IsEnabled = true;
            angleTb.IsEnabled = true;

        }


    }
}
