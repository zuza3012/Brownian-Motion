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
        public void MoveParticle() {
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


        }


    }
}
