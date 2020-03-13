using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace BrownianMotion {
    public partial class MainWindow : Window {

        public void TransformTo2D() {
            offX = canvas.ActualWidth / 2;
            offY = canvas.ActualHeight / 2;
            

            theta = Math.PI * azimuth / 180.0;
            phi = Math.PI * elevation / 180.0;
            cosT = (float)Math.Cos(theta);
            sinT = (float)Math.Sin(theta);
            cosP = (float)Math.Cos(phi);
            sinP = (float)Math.Sin(phi);
            cosTcosP = cosT * cosP;
            cosTsinP = cosT * sinP;
            sinTcosP = sinT * cosP;
            sinTsinP = sinT * sinP;
        }

        private void DrawPoint(Point3D point) {
            TransformTo2D();
            //canvas.Children.Clear();
            Console.WriteLine("rysuje" + "(" + point.X + "," + point.Y + "," + point.Z + ")");
            Point3D p = new Point3D();
            p.X = point.X;
            p.Y = point.Y;
            p.Z = point.Z;

            Console.WriteLine();
            if (point.X > ((double)a - 20 * zoom) || point.X < ((double)(-a) + 20 * zoom)|| point.Y > ((double)a - 20 * zoom) || point.Y < ((double)(-a) + 20 * zoom) || point.Z > ((double)a -20 * zoom)|| point.Z <((double)(-a) + 20 * zoom)) {
                Console.WriteLine("jestem tuuu");
                particle.p.X = tmpX;
                particle.p.Y = tmpY;
                particle.p.Z = tmpZ;
                p.X = tmpX;
                p.Y = tmpY;
                p.Z = tmpZ;

                Console.WriteLine();
                Console.WriteLine("spr");
                Console.WriteLine("(" + p.X + "," + p.Y + "," + p.Z + ")");
            }
            double x = cosT * p.X + sinT * p.Z;
            double y = sinTsinP * p.X - cosP * p.Y - cosTsinP * p.Z;
            double z = cosTcosP * p.Z - sinTcosP * p.X - sinP * p.Y;
          
            x *= zoom * canvas.ActualHeight / (z + 2 * canvas.ActualHeight);
            y *= zoom * canvas.ActualHeight / (z + 2 * canvas.ActualHeight);
            
            pointsOnLine.Add(new Point(x + offX, y + offY));
         

            Ellipse ellipse = new Ellipse {
                Width = 20 * zoom,
                Height = 20 * zoom,
                Stroke = Brushes.Aqua,
                StrokeThickness = 2,
                //Fill = Brushes.Black
            };

            Canvas.SetLeft(ellipse, (x + offX -10));
            Canvas.SetTop(ellipse, (y + offY -10));
            canvas.Children.Add(ellipse);
           
           DrawPath(pointsOnLine);
            
           
        }

        private void DrawCube(int a, double zoom) {
            TransformTo2D();

            canvas.Children.Clear();

            edges2D = new Point[8];
            edges3D.Clear();

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

            int i = 0;
            foreach (Point3D point in edges3D) {
                double x = cosT * point.X + sinT * point.Z;
                double y = sinTsinP * point.X - cosP * point.Y - cosTsinP * point.Z;
                double z = cosTcosP * point.Z - sinTcosP * point.X - sinP * point.Y;

                x *= zoom * canvas.ActualHeight / (z + 2 * canvas.ActualHeight);
                y *= zoom * canvas.ActualHeight / (z + 2 * canvas.ActualHeight);

                edges2D[i] = new Point(x + offX, y + offY);
                // Console.WriteLine(x + ", " + y);
                i++;
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
            Console.WriteLine("to chce rysowac: " + "(" + particle.p.X + "," + particle.p.Y + "," + particle.p.Z + ")");
            DrawPoint(particle.p);

            

            return;
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

        private void DrawPath(List<Point> list) {
            for(int i = 1; i < list.Count; i++) {
                DrawLine(list[2*i], list[2*i+1]);
                //grrrrr
            }
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
