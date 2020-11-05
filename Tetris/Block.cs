using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Threading;

namespace Tetris
{
    static class Block
    {
        static private Color FromRgb(int a, int b, int c, int d)
        {
            // Create a green color using the FromRgb static method.
            Color myRgbColor = new Color();
            myRgbColor = Color.FromArgb(a, b, c, d);
            return myRgbColor;
        }
        public static int CurBlockNum, NextBlockNum, HoldBlockNum;
        public static Color[] C = new Color[]{
            FromRgb(255,15, 155, 215),
            FromRgb(255, 227, 159, 2),
            FromRgb(255, 227, 91, 2),
            FromRgb(255, 89, 177, 1),
            FromRgb(255, 215, 15, 55),
            FromRgb(255, 175, 41, 138),
            FromRgb(255, 33, 65, 198)
        };
        

        public static int[,,,] shape =new int[,,,]{ // 테트리스 종류, 돌렸을 때,y좌표 x좌표
            { //하늘색
                    {{1, 1, 1, 1}, {0, 0, 0, 0}, {0, 0, 0, 0}, {0, 0, 0, 0}},
                    {{0, 0, 1, 0}, {0, 0, 1, 0}, {0, 0, 1, 0}, {0, 0, 1, 0}},
                    {{-1,-1,-1,-1}, {-1,-1,-1,-1}, {-1,-1,-1,-1}, {-1,-1,-1,-1}},
                    {{-1,-1,-1,-1}, {-1,-1,-1,-1}, {-1,-1,-1,-1}, {-1,-1,-1,-1}}
            },
            { //노란색
                    {{0, 1, 1, 0}, {0, 1, 1, 0},{0, 0, 0, 0}, {0, 0, 0, 0}},
                    {{-1,-1,-1,-1}, {-1,-1,-1,-1}, {-1,-1,-1,-1}, {-1,-1,-1,-1}},
                    {{-1,-1,-1,-1}, {-1,-1,-1,-1}, {-1,-1,-1,-1}, {-1,-1,-1,-1}},
                    {{-1,-1,-1,-1}, {-1,-1,-1,-1}, {-1,-1,-1,-1}, {-1,-1,-1,-1}}
            },
            { //주황색
                    {{0, 0, 1, 0}, {1, 1, 1, 0}, {0, 0, 0, 0}, {0, 0, 0, 0}},
                    {{0, 1, 0, 0}, {0, 1, 0, 0}, {0, 1, 1, 0}, {0, 0, 0, 0}},
                    {{1, 1, 1, 0}, {1, 0, 0, 0}, {0, 0, 0, 0}, {0, 0, 0, 0}},
                    {{1, 1, 0, 0}, {0, 1, 0, 0}, {0, 1, 0, 0}, {0, 0, 0, 0}},
            },
            { //초록색
                    {{0, 1, 1, 0}, {1, 1, 0, 0}, {0, 0, 0, 0}, {0, 0, 0, 0}},
                    {{0, 1, 0, 0}, {0, 1, 1, 0}, {0, 0, 1, 0}, {0, 0, 0, 0}},
                    {{-1,-1,-1,-1}, {-1,-1,-1,-1}, {-1,-1,-1,-1}, {-1,-1,-1,-1}},
                    {{-1,-1,-1,-1}, {-1,-1,-1,-1}, {-1,-1,-1,-1}, {-1,-1,-1,-1}}
            },
            { //빨간색
                    {{1, 1, 0, 0}, {0, 1, 1, 0}, {0, 0, 0, 0}, {0, 0, 0, 0}},
                    {{0, 0, 1, 0}, {0, 1, 1, 0}, {0, 1, 0, 0}, {0, 0, 0, 0}},
                    {{-1,-1,-1,-1}, {-1,-1,-1,-1}, {-1,-1,-1,-1}, {-1,-1,-1,-1}},
                    {{-1,-1,-1,-1}, {-1,-1,-1,-1}, {-1,-1,-1,-1}, {-1,-1,-1,-1}}
            },
            { //핑크색
                    {{0, 1, 0, 0}, {1, 1, 1, 0}, {0, 0, 0, 0}, {0, 0, 0, 0}},
                    {{0, 1, 0, 0}, {0, 1, 1, 0}, {0, 1, 0, 0}, {0, 0, 0, 0}},
                    {{1, 1, 1, 0}, {0, 1, 0, 0}, {0, 0, 0, 0}, {0, 0, 0, 0}},
                    {{0, 1, 0, 0}, {1, 1, 0, 0}, {0, 1, 0, 0}, {0, 0, 0, 0}},
            },
            { //파란색
                    {{1, 0, 0, 0}, {1, 1, 1, 0}, {0, 0, 0, 0}, {0, 0, 0, 0}},
                    {{0, 1, 1, 0}, {0, 1, 0, 0}, {0, 1, 0, 0}, {0, 0, 0, 0}},
                    {{1, 1, 1, 0}, {0, 0, 1, 0}, {0, 0, 0, 0}, {0, 0, 0, 0}},
                    {{0, 1, 0, 0}, {0, 1, 0, 0}, {1, 1, 0, 0}, {0, 0, 0, 0}},
            },
    };
        public static void TRead()
        {
            Color color;
            int i, j;
            color = Getpixel.GetColorAt(new Point(693, 180));
            if (color.R + color.G + color.B < 200)
            {
                color = Getpixel.GetColorAt(new Point(693, 204));
            }
            // Print the RGB information of the pixel color
            //Console.WriteLine(color.R.ToString() + " " + color.G.ToString() + " " + color.B.ToString());
            for (i = 0; i < 7; i++)
            { //Get tetris block number
                if (color == C[i])
                {
                    CurBlockNum = i;
                }
            }
        }
        public static void Read()
        {
            //Thread.Sleep(500); //temporary
            Color color;
            int i, j;
            color=Getpixel.GetColorAt(new Point(895, 206));
                if (color.R + color.G + color.B < 200)
                {
                    color = Getpixel.GetColorAt(new Point(897, 230));
                }
            // Print the RGB information of the pixel color
            //Console.WriteLine(color.R.ToString() + " " + color.G.ToString() + " " + color.B.ToString());
            for (i = 0; i < 7; i++)
            { //Get tetris block number
               if (color==C[i])
                {
                    CurBlockNum = i;
                }
            }
            
            color = Getpixel.GetColorAt(new Point(517, 206));
            if (color.R + color.G + color.B < 200)
            {
                color = Getpixel.GetColorAt(new Point(517, 230));
            }
            // Print the RGB information of the pixel color
            //Console.WriteLine(color.R.ToString() + " " + color.G.ToString() + " " + color.B.ToString());
            for (i = 0; i < 7; i++)
            { //Get tetris block number
                if (color == C[i])
                {
                    HoldBlockNum = i;
                }
            }

            color = Getpixel.GetColorAt(new Point(897, 276));
            if (color.R + color.G + color.B < 200)
            {
                color = Getpixel.GetColorAt(new Point(897, 305));
            }
            // Print the RGB information of the pixel color
            //Console.WriteLine(color.R.ToString() + " " + color.G.ToString() + " " + color.B.ToString());
            for (i = 0; i < 7; i++)
            { //Get tetris block number
                if (color == C[i])
                {
                    NextBlockNum = i;
                }
            }
        }
    }
}













