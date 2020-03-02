using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;
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

        private void DrawCube(int a) {
            double theta = Math.PI * azimuth / 180.0;
            double phi = Math.PI * elevation / 180.0;
            float cosT = (float)Math.Cos(theta), sinT = (float)Math.Sin(theta), cosP = (float)Math.Cos(phi),
                    sinP = (float)Math.Sin(phi);
            float cosTcosP = cosT * cosP, cosTsinP = cosT * sinP, sinTcosP = sinT * cosP, sinTsinP = sinT * sinP;


            Point3D p1 = new Point3D(a, a, a);
            Point3D p2 = new Point3D(-a, a, a);
            Point3D p3 = new Point3D(-a, -a, a);
            Point3D p4 = new Point3D(a, -a, a);
           
            Point3D p5 = new Point3D(a, a, -a);
            Point3D p6 = new Point3D(-a, a, -a);
            Point3D p7 = new Point3D(-a, -a, -a);
            Point3D p8 = new Point3D(a, -a, -a);

            edges3D.Add(p1);
            edges3D.Add(p2);
            edges3D.Add(p3);
            edges3D.Add(p4);
            edges3D.Add(p5);
            edges3D.Add(p6);
            edges3D.Add(p7);
            edges3D.Add(p8);

            foreach(Point3D point in edges3D) {
                double x = cosT * point.X + sinT * point.Z;
                double y = sinTsinP * point.X - cosP * point.Y - cosTsinP * point.Z;
                double z = cosTcosP * point.Z - sinTcosP * point.X - sinP * point.Y;

                x *= 500 / (z + 2 * 500);
                y *= 500 / (z + 2 * 500);

                edges2D.Add(new Point(x, y));
                
            }
            
            DrawLine(edges2D[0], edges2D[1]);
            DrawLine(edges2D[1], edges2D[2]);
            DrawLine(edges2D[2], edges2D[3]);
            DrawLine(edges2D[3], edges2D[0]);

            DrawLine(edges2D[4], edges2D[5]);
            DrawLine(edges2D[5], edges2D[6]);
            DrawLine(edges2D[6], edges2D[7]);
            DrawLine(edges2D[7], edges2D[4]);

            DrawLine(edges2D[0], edges2D[4]);
            DrawLine(edges2D[1], edges2D[5]);
            DrawLine(edges2D[2], edges2D[6]);
            DrawLine(edges2D[3], edges2D[7]);


        }

        private void DrawLine(Point point1, Point point2) {
            Line line = new Line {
                Stroke = Brushes.Black,
                X1 = point1.X,
                Y1 = point1.Y,

                X2 = point2.X,
                Y2 = point2.Y
            };
            canvas.Children.Add(line);
        }


        private void DrawCircle() {
            double posX = 100, posY = 100;
            Ellipse ellipse = new Ellipse {
                Width = 10,
                Height = 10,
                Stroke = Brushes.Blue,
                StrokeThickness = 2
            };
        
            Canvas.SetLeft(ellipse, posX);
            Canvas.SetTop(ellipse, posY);
            canvas.Children.Add(ellipse);
        }
        private double[,] MatrixMul(double[,] a, double[,] b) {
            int colsA = a.GetLength(1);
            int rowsA = a.GetLength(0);
            int colsB = b.GetLength(1);
            int rowsB = b.GetLength(0);

            if (colsA != rowsB) {
                Console.WriteLine("Cannot multyply!");
                return null;
            }

            double[,] result = new double[rowsA, colsB];

            for (int i = 0; i < rowsA; i++) {
                for (int j = 0; j < colsB; j++) {
                    double sum = 0;
                    for (int k = 0; k < colsA; k++) {
                        sum += a[i, k] * b[k, j];
                    }
                    result[i, j] = sum;
                }
            }

            return result;
        }

        private void PrintMatrix(double[,] m) {
            int rows = m.GetLength(0);
            int cols = m.GetLength(1);

            Console.WriteLine();
            Console.WriteLine(rows + "x" + cols);

            for (int i = 0; i < rows; i++) {
                for (int j = 0; j < cols; j++) {
                    Console.Write(m[i, j] + " ");
                }
                Console.WriteLine();
            }


        }
    }
}
