using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace BrownianMotion {
    public partial class MainWindow : Window {
        
        private GeometryModel3D mGeometry;
        private bool mouseDown;
        private Point mouseLastPos;
        private void CreateObject() {
            MeshGeometry3D mesh = new MeshGeometry3D(); // siatka trojkatna - mesh

            // dodajemy punkty "pomieszczenia"

            mesh.Positions.Add(new Point3D(-1, -1, 1));
            mesh.Positions.Add(new Point3D(1, -1, 1));
            mesh.Positions.Add(new Point3D(1, 1, 1));
            mesh.Positions.Add(new Point3D(-1, 1, 1));

            mesh.Positions.Add(new Point3D(-1, -1, -1));
            mesh.Positions.Add(new Point3D(1, -1, -1));
            mesh.Positions.Add(new Point3D(1, 1, -1));
            mesh.Positions.Add(new Point3D(-1, 1, -1));


            // tworzymy sobie trojkaciki i je laczymy ze soba w jedan calosc

            //gora
            mesh.TriangleIndices.Add(0);
            mesh.TriangleIndices.Add(1);
            mesh.TriangleIndices.Add(2);
            mesh.TriangleIndices.Add(2);
            mesh.TriangleIndices.Add(3);
            mesh.TriangleIndices.Add(0);

            //dol 
            mesh.TriangleIndices.Add(6);
            mesh.TriangleIndices.Add(5);
            mesh.TriangleIndices.Add(4);
            mesh.TriangleIndices.Add(4);
            mesh.TriangleIndices.Add(7);
            mesh.TriangleIndices.Add(6);

            //tylna // dziwnie dzialaja te trojkaty, magia
            mesh.TriangleIndices.Add(1);
            mesh.TriangleIndices.Add(5);
            mesh.TriangleIndices.Add(2);
            mesh.TriangleIndices.Add(5);
            mesh.TriangleIndices.Add(6);
            mesh.TriangleIndices.Add(2);

            //prawa
            mesh.TriangleIndices.Add(2);
            mesh.TriangleIndices.Add(6);
            mesh.TriangleIndices.Add(3);
            mesh.TriangleIndices.Add(3);
            mesh.TriangleIndices.Add(6);
            mesh.TriangleIndices.Add(7);

            //lewa
            mesh.TriangleIndices.Add(5);
            mesh.TriangleIndices.Add(1);
            mesh.TriangleIndices.Add(0);
            mesh.TriangleIndices.Add(0);
            mesh.TriangleIndices.Add(4);
            mesh.TriangleIndices.Add(5);

            //przod
            mesh.TriangleIndices.Add(4);
            mesh.TriangleIndices.Add(0);
            mesh.TriangleIndices.Add(3);
            mesh.TriangleIndices.Add(3);
            mesh.TriangleIndices.Add(7);
            mesh.TriangleIndices.Add(4);

            //stwarzamy Geometry

            /*DiffuseMaterial mat = (DiffuseMaterial)mGeometry.Material;

            SolidColorBrush br = (SolidColorBrush)mat.Brush;

            br.Opacity = 0.3;

            group.Children.Remove(mGeometry);

            group.Children.Add(mGeometry);*/
            
            mGeometry = new GeometryModel3D(mesh, new DiffuseMaterial(new SolidColorBrush(Color.FromRgb(252, 255, 252))));
           mGeometry.Transform = new Transform3DGroup();
           group.Children.Add(mGeometry); // gropup z xaml'a
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e) {
            camera.Position = new Point3D(camera.Position.X, camera.Position.Y, 5);     // resetujemy kamere i kladziemyna z=5 
                                                                                        //bo tyle bylo pocztkowo w xamlu
            mGeometry.Transform = new Transform3DGroup();
        }


        private void Grid_MouseWheel(object sender, MouseWheelEventArgs e) { // kamera jedzie tylko po Zcie!
            camera.Position = new Point3D(camera.Position.X, camera.Position.Y, camera.Position.Z - e.Delta / 250D);
        }


        private void Grid_MouseDown(object sender, MouseButtonEventArgs e) {
            if (e.LeftButton != MouseButtonState.Pressed) {
                return;
            }
            mouseDown = true;
            Point pos = Mouse.GetPosition(viewPort);
            mouseLastPos = new Point(pos.X - viewPort.ActualWidth / 2, viewPort.ActualHeight / 2 - pos.Y);
        }

        private void Grid_MouseUp(object sender, MouseButtonEventArgs e) {
            mouseDown = false;
        }
    

        private void Grid_MouseMove(object sender, MouseEventArgs e) {
            if (mouseDown) {
                Point pos = Mouse.GetPosition(viewPort);
                Point actualPos = new Point(pos.X - viewPort.ActualWidth / 2, viewPort.ActualHeight / 2 - pos.Y); // bo 3d i takie kupy
                double dx = actualPos.X - mouseLastPos.X, dy = actualPos.Y - mouseLastPos.Y; // przesuniecie myszki

                double mouseAngle = 0;
                if (dx != 0 && dy != 0) {
                    mouseAngle = Math.Asin(Math.Abs(dy) / Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2)));
                    if (dx < 0 && dy > 0) mouseAngle += Math.PI / 2;
                    else if (dx < 0 && dy < 0) mouseAngle += Math.PI; // obrot o 90 lub 270 stopni
                    else if (dx > 0 && dy < 0) mouseAngle += Math.PI * 1.5;
                } else if (dx == 0 && dy != 0) {
                    mouseAngle = Math.Sign(dy) > 0 ? Math.PI / 2 : Math.PI * 1.5;
                } else if (dx != 0 && dy == 0) {
                    mouseAngle = Math.Sign(dx) > 0 ? 0 : Math.PI;
                }

                double axisAngle = mouseAngle + Math.PI / 2; // kat wzgledem Xa

                Vector3D axis = new Vector3D(Math.Cos(axisAngle) * 4, Math.Sin(axisAngle) * 4, 0);

                double rotation = 0.01 * Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2));

                Transform3DGroup group = mGeometry.Transform as Transform3DGroup;
                QuaternionRotation3D r = new QuaternionRotation3D(new Quaternion(axis, rotation * 180 / Math.PI));
                group.Children.Add(new RotateTransform3D(r));

                mouseLastPos = actualPos;
            }
        }




    }
}
