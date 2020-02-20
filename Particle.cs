using System;
using System.Collections.Generic;

namespace BrownianMotion {
    class Particle {
        public int x, y, d;
        public double D;
        // public double angle { get; set; }
        public static double angle = 0.2;
        public double angle2 { get; set; }
        //public double angle = 0.30;
        public Particle() {
            x = 0;
            y = 0;
            D = 0;
            d = 0;
        }

        
        //List<double[,]> listOfPoints = new List<double[,]>();
        //public List<double[,]> projected2dPoints = new List<double[,]>();
        public List<double[,]> MakeARoom(List<double[,]> pointsList) {

            double[,] projection = { {1,0,0},
                              {0,1,0}
        };


            double[,] rotationZ = {{ System.Math.Cos(angle), -System.Math.Sin(angle) ,0},
                              { System.Math.Sin(angle), System.Math.Cos(angle), 0},
                              {0, 0, 1}
        };

            double[,] rotationX = { {1,0,0},
                              {0, System.Math.Cos(angle), -System.Math.Sin(angle)},
                              {0, System.Math.Sin(angle), System.Math.Cos(angle)}
        };


            double[,] rotationY = { { System.Math.Cos(angle2), 0, -System.Math.Sin(angle2)},
                              {0,1,0},
                              { System.Math.Sin(angle2), 0, System.Math.Cos(angle2)}
        };
/*
            double[,] p01 = { {0},
                         {220},
                         {0}
        };

            double[,] p02 = { {0},
                         {-220},
                         {0}
        };


            double[,] point1 = { {220},
                         {-220},
                         {-220}
        };
            double[,] point2 = { {220},
                         {220},
                         {-220}
        };
            double[,] point3 = { {-220},
                         {220},
                         {-220}
        };
            double[,] point4 = { {-220},
                         {-220},
                         {-220}
        };
            double[,] point5 = { {220},
                         {-220},
                         {220}
        };
            double[,] point6 = { {220},
                         {220},
                         {220}

        };
            double[,] point7 = { {-220},
                         {220},
                         {220}

        };
            double[,] point8 = { {-220},
                         {-220},
                         {220}

        };


            listOfPoints.Add(point1);
            listOfPoints.Add(point2);
            listOfPoints.Add(point3);
            listOfPoints.Add(point4);
            listOfPoints.Add(point5);
            listOfPoints.Add(point6);
            listOfPoints.Add(point7);
            listOfPoints.Add(point8);
            listOfPoints.Add(p01);
            listOfPoints.Add(p02);*/

            List<double[,]> projected2dPoints = new List<double[,]>();
            foreach (double [,] item in pointsList) {
                double[,] rotated = MatrixMul(rotationY, item);
                rotated = MatrixMul(rotationX, rotated);
                double[,] projected2d = MatrixMul(projection, rotated);
                projected2dPoints.Add(projected2d);
            }
            return projected2dPoints;
        }

        public double[,] MatrixMul(double[,] a, double[,] b) {
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

        public void PrintMatrix(double[,] m) {
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
