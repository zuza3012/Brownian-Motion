using System;
using System.Windows;

namespace BrownianMotion {
    class Particle {
        public int d;
        public Point p;
        int u1, u2;
        Random rand = new Random();
        public Particle() {
            p.X = 0;
            p.Y = 0;
            d = 0;
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
