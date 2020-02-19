using System;
using System.Collections.Generic;

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

        int[,] projection = { {1,0,0},
                              {0,1,0}
        };
        int[,] point = { {10},
                         {20},
                         {30}
        };
        public void MoveParticle(int steps) {
            xLocations = new List<int>();
            yLocations = new List<int>();

            Random rand = new Random();
            for (int i = 0; i < steps; i++) {

                int u1 = rand.Next(0, 100);
                int u2 = rand.Next(0, 100);
               

                xLocations.Add(x);
                yLocations.Add(y);

                if (u1 >= 50) {
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
            }

            
            PrintMatrix(projection);
            PrintMatrix(point);
            PrintMatrix(MatrixMul(projection, point));
        }

        public int[,] MatrixMul(int[,] a, int[,] b) {
            int colsA = a.GetLength(1);
            int rowsA = a.GetLength(0);
            int colsB = b.GetLength(1);
            int rowsB = b.GetLength(0);

            if (colsA != rowsB) {
                Console.WriteLine("Cannot multyply!");
                return null;
            }

            int[,] result = new int[rowsA, colsB];

            for (int i = 0; i < rowsA; i++) {
                for (int j = 0; j < colsB; j++) {
                    int sum = 0;
                    for (int k = 0; k < colsA; k++) {
                        sum += a[i, k] * b[k, j];
                    }
                    result[i, j] = sum;
                }
            }

            return result;
        }

        public void PrintMatrix(int[,] m) {
            int rows = m.GetLength(0);
            int cols = m.GetLength(1);

            Console.WriteLine();
            Console.WriteLine(rows + "x" + cols);

            for(int i = 0; i < rows; i++) {
                for(int j = 0; j < cols; j++) {
                    Console.Write(m[i,j] + " ");
                }
                Console.WriteLine();
            }
            
        }
    }
}
