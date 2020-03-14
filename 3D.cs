using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace BrownianMotion {
    public partial class MainWindow : Window {

        private void ParametersToTransform() {
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

        private Point3D Transform2D(Point3D point) {
            double x = cosT * point.X + sinT * point.Z;
            double y = sinTsinP * point.X - cosP * point.Y - cosTsinP * point.Z;
            double z = cosTcosP * point.Z - sinTcosP * point.X - sinP * point.Y;

            
            
            return new Point3D(x, y, z);
        }
        private void DrawPoint(Point3D point) {
            //ParametersToTransform();
            //canvas.Children.Clear();
           
            Point3D p = new Point3D();
            p.X = point.X;
            p.Y = point.Y;
            p.Z = point.Z;

            Console.WriteLine();
            if (point.X > ((double)a - 20 * zoom) || point.X < ((double)(-a) + 20 * zoom)|| point.Y > ((double)a - 20 * zoom) || point.Y < ((double)(-a) + 20 * zoom) || point.Z > ((double)a -20 * zoom)|| point.Z <((double)(-a) + 20 * zoom)) {
                particle.p.X = tmpX;
                particle.p.Y = tmpY;
                particle.p.Z = tmpZ;
                p.X = tmpX;
                p.Y = tmpY;
                p.Z = tmpZ;
            }
            
            Point3D transformedPoint = Transform2D(point);

            polygonPoints3D.Add(transformedPoint);

            transformedPoint.X *= zoom * canvas.ActualHeight / (transformedPoint.Z + 2 * canvas.ActualHeight);
            transformedPoint.Y *= zoom * canvas.ActualHeight / (transformedPoint.Z + 2 * canvas.ActualHeight);

            polygonPoints.Add(new Point(transformedPoint.X + offX, transformedPoint.Y + offY));
            
            

            Console.WriteLine("count polygonPoints3D: " + polygonPoints3D.Count);
            Console.WriteLine("count polygonPoints: " + polygonPoints.Count);

            Ellipse ellipse = new Ellipse {
                Width = 20 * zoom,
                Height = 20 * zoom,
                Stroke = Brushes.Aqua,
                StrokeThickness = 2,
                //Fill = Brushes.Black
            };

            Canvas.SetLeft(ellipse, (transformedPoint.X + offX -10));
            Canvas.SetTop(ellipse, (transformedPoint.Y + offY -10));
            canvas.Children.Add(ellipse);

            
            
        }

        private void TransformPolygon(Point3DCollection polygon3D, PointCollection polygon2D) {
            // change the elemnets in polygonsPoints3D to zoom
            //polygonPoints.Clear();
            for (int i = 0; i < polygon3D.Count; i++) {
                Console.WriteLine("polygon before" + "(" + polygon3D[i].X + "," + polygon3D[i].Y + "," + polygon3D[i].Z + ")");
                Point3D new_point = Transform2D(polygon3D[i]);
                polygon3D[i] = new_point;
                Console.WriteLine("polygon after" + "(" + polygon3D[i].X + "," + polygon3D[i].Y + "," + polygon3D[i].Z + ")");
                new_point.X *= zoom * canvas.ActualHeight / (new_point.Z + 2 * canvas.ActualHeight);
                new_point.Y *= zoom * canvas.ActualHeight / (new_point.Z + 2 * canvas.ActualHeight);
                polygonPoints[i] = new Point(new_point.X + offX, new_point.Y + offY);
            }
            
           
           /* for (int i = 0; i < polygonPoints3D.Count; i++) {
                Point3D tmp = Transform2D(polygonPoints3D[i]);
                polygonPoints3D[i] = tmp;
                polygonPoints[i] = new Point(polygonPoints3D[i].X + offX, polygonPoints3D[i].Y + offY);
            }*/
        }
        private void DrawCube(int a, double zoom) {
            ParametersToTransform();
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
                Point3D p = Transform2D(point);
                p.X *= zoom * canvas.ActualHeight / (p.Z + 2 * canvas.ActualHeight);
                p.Y *= zoom * canvas.ActualHeight / (p.Z + 2 * canvas.ActualHeight);
                edges2D[i] = new Point(p.X + offX, p.Y + offY);
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
            
            DrawPoint(particle.p);
            //TransformPolygon(polygonPoints3D, polygonPoints);
            CreateAPolyline();


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


        private void CreateAPolyline() {
            SolidColorBrush brush = new SolidColorBrush();
            brush.Color = Colors.Blue;
            
            Polyline yellowPolyline = new Polyline();
            yellowPolyline.Stroke = brush;
            yellowPolyline.StrokeThickness = 1;
            yellowPolyline.Points = polygonPoints;
            
            canvas.Children.Add(yellowPolyline);
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
