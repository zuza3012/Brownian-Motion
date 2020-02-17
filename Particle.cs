using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrownianMotion {
    class Particle {
        public int x, y, d;
        public double D;
        public List<int> xLocations, yLocations;
        public Particle() {
            x = 0;
            y = 0;
            D = 0;
            d = 0;
        }
        public void MoveParticle(int steps) {
            xLocations = new List<int>();
            yLocations = new List<int>();



            //Transformacja Boxa-Mullera
            // double R2 = -2 * Math.Log(u1);
            // double theta = 2 * 3.14 * u2;

            // stwarzamy z1 i z2 niezalezne z rozkladu normlnego
            //double z1 = Math.Sqrt(R2) * Math.Cos(theta);
            //double z2 = Math.Sqrt(R2) * Math.Sin(theta);

            Random rand = new Random();
            for (int i = 0; i < steps; i++) {

               
                int u1 = rand.Next(0, 100);
                int u2 = rand.Next(0, 100);
                Console.WriteLine("u1: " + u1);
                Console.WriteLine("u2: " + u2);

                xLocations.Add(x);
                yLocations.Add(y);

                if(u1 >= 50) {
                    int tmp;
                    tmp = x + d;
                    x = tmp;
                } else {
                    int tmp;
                    tmp = x - d;
                    x = tmp;
                }

                if (u2 >= 50) {
                    int tmp;
                    tmp = y + d;
                    y = tmp;
                } else {
                    int tmp;
                    tmp = y - d;
                    y = tmp;
                }


               // x = x + Math.Sqrt(2 * D * i) * z1;
               // y = y + Math.Sqrt(2 * D * i) * z2;
                
            }

        }


    }   
}
