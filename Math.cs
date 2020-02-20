using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BrownianMotion {
    public partial class MainWindow : Window {
        private List<double[,]> listOfPoints;
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
        private void TransformPoints() {

            listOfCorners = room.MakeARoom(listOfPoints);

        }



    }
}
