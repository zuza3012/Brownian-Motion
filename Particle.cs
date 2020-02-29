using System;
using System.Windows;

namespace BrownianMotion {
    class Particle {
        public int d;
        public Point p;
        int u1, u2;
        Random rand = new Random();
        public Particle(double width, double height) {
            p.X = width / 2;
            p.Y = height / 2;
            d = 1;
        }
        public void MoveParticle(double width, double height) {
            u1 = rand.Next(0, 100);

            u2 = rand.Next(0, 100);

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


    }
}
