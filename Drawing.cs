using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace BrownianMotion {
    public partial class MainWindow : Window {

        private void DrawPoint(double[,] point) { 

            Ellipse ellipse = new Ellipse {
                Width = 10,
                Height = 10,
                Stroke = Brushes.Black,
                StrokeThickness = 2,
                Fill = Brushes.Black
            };

            Canvas.SetLeft(ellipse, (point[0, 0] - 5));
            Canvas.SetTop(ellipse, (point[1, 0] - 5));
            canvas.Children.Add(ellipse);
        }

        private void DrawLine(double[,] point1, double[,] point2, bool dash) {

            if (dash) {
                Line line = new Line {
                    Stroke = Brushes.Black,
                    StrokeDashArray = new DoubleCollection() { 2 },
                    X1 = point1[0, 0],
                    Y1 = point1[1, 0],

                    X2 = point2[0, 0],
                    Y2 = point2[1, 0]
                };
                canvas.Children.Add(line);
            } else {
                Line line = new Line {
                    Stroke = Brushes.Black,
                    X1 = point1[0, 0],
                    Y1 = point1[1, 0],

                    X2 = point2[0, 0],
                    Y2 = point2[1, 0]
                };
                canvas.Children.Add(line);
                
            }
             
              
        }


        private void Draw() {       // rysuje szescian
            List<double[,]> listOfCorners = room.projected2dPoints;
            foreach (double[,] item in listOfCorners) {
                DrawPoint(item);
            }

            // zrobic ifa so linii
            DrawLine(listOfCorners[0], listOfCorners[1], false);
            DrawLine(listOfCorners[1], listOfCorners[2], false);
            DrawLine(listOfCorners[2], listOfCorners[3], true);
            DrawLine(listOfCorners[3], listOfCorners[0], true);

            DrawLine(listOfCorners[4], listOfCorners[5], false);
            DrawLine(listOfCorners[5], listOfCorners[6], false);
            DrawLine(listOfCorners[6], listOfCorners[7], false);
            DrawLine(listOfCorners[7], listOfCorners[4], false);

            DrawLine(listOfCorners[0], listOfCorners[4], false);
            DrawLine(listOfCorners[1], listOfCorners[5], false);
            DrawLine(listOfCorners[2], listOfCorners[6], false);
            DrawLine(listOfCorners[3], listOfCorners[7], true);

          
        }
    }
}
