using System;
using System.Windows;
using System.Windows.Media.Media3D;

namespace BrownianMotion {
    class Particle {
        public int d;
        public Point3D p;
        int u1, u2, u3;
        Random rand = new Random();
        public Particle(double width, double height, bool want3D) {
            if (!want3D) {
                p.X = width / 2;
                p.Y = height / 2;
                p.Z = 0;
                d = 1;
            } else {
                p.X = 0;
                p.Y = 0;
                p.Z = 0;
                d = 15;
            }
        }
        public void MoveParticle(double width, double height, bool want3D, int a, int radius) {
            u1 = rand.Next(0, 100);
            u2 = rand.Next(0, 100);
            double tmpX = 0, tmpY = 0, tmpZ = 0;

            tmpX = p.X;
            tmpY = p.Y;
            if (want3D) {
                tmpZ = p.Z;
            }

            if (u1 >= 50) {
                double tmp;
                tmp = p.X + d;
                p.X = tmp;  

            } else {
                double tmp;
                tmp = p.X - d;
                p.X = tmp;
            }

            if (u2 >= 50) {
                double tmp;
                tmp = p.Y + d;
                p.Y = tmp;
            } else {
                double tmp;
                tmp = p.Y - d;
                p.Y = tmp;
            }

            if (want3D) {
                u3 = rand.Next(0, 100);
                if (u3 >= 50) {
                    double tmp;
                    tmp = p.Z + d;
                    p.Z = tmp;

                } else {
                    double tmp;
                    tmp = p.Z - d;
                    p.Z = tmp;
                }

            }

            if (!want3D) {
                if (p.X >= width) {
                    p.X = p.X - width;
                } else if (p.X < 0) {
                    p.X = p.X + width;
                } else {
                }

                if (p.Y >= height) {
                    p.Y = p.Y - height;
                } else if (p.Y < 0) {
                    p.Y = p.Y + height;
                } else {
                }
            }

            if (want3D && (Math.Abs(p.X) > (double)(a - radius) || Math.Abs(p.Y) > (double)(a - radius) || Math.Abs(p.Z) > (double)(a - radius))) {
                p.X = tmpX;
                p.Y = tmpY;
                p.Z = tmpZ;
                return;
            }

        }

    }
}
