using System;
using System.Diagnostics;
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

            x *= zoom * canvas.ActualHeight / (z + 2 * canvas.ActualHeight);
            y *= zoom * canvas.ActualHeight / (z + 2 * canvas.ActualHeight);

            return new Point3D(x, y, z);
        }
        private void DrawPoint(Point3D point, bool worker) {
            Point3D p = new Point3D();
            p.X = point.X;
            p.Y = point.Y;
            p.Z = point.Z;
         
            // transformowane polozenie kulki 
            Point3D transformedPoint = Transform2D(p);

            if (worker) {
                polygonPoints3D.Add(transformedPoint);
                polygonPoints.Add(new Point(transformedPoint.X + offX, transformedPoint.Y + offY));
            }
           
            // to jakos trzeba usprawnic
            for (int j = 0; j < polygonPoints3D.Count; j++) {
                Point3D tmp = Transform2D(polygonPoints3D[j]);
                //Debug.WriteLine("linia 3d: " + (tmp.X + offX) + ", " + (tmp.Y + offY));
                polygonPoints[j] = new Point(tmp.X + offX, tmp.Y + offY);             
                //Debug.WriteLine("linia 2d: " + polygonPoints[j].X + ", " + polygonPoints[j].Y);
            }

            Ellipse ellipse = new Ellipse {
                Width = 20 * zoom,
                Height = 20 * zoom,
                Stroke = Brushes.Aqua,
                StrokeThickness = 2,
                //Fill = Brushes.Black
            };

            int number = polygonPoints.Count - 1;
            Canvas.SetLeft(ellipse, (polygonPoints[number].X - 10));
            Canvas.SetTop(ellipse, (polygonPoints[number].Y - 10));

            canvas.Children.Add(ellipse);
  
        }

        private void DrawCube(int a, double zoom, bool worker) {
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

            DrawPoint(particle.p, worker);

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

            Polyline polyline = new Polyline();
            polyline.Stroke = brush;
            polyline.StrokeThickness = 1;
            polyline.Points = polygonPoints;

            canvas.Children.Add(polyline);
        }
       
    }
}
