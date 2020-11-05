﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace Tetris
{
    static class TBlock
    {
        static private Color FromRgb(int a, int b, int c, int d)
        {
            // Create a green color using the FromRgb static method.
            Color myRgbColor = new Color();
            myRgbColor = Color.FromArgb(a, b, c, d);
            return myRgbColor;
        }
        public static int CurTBlockNum, NextTBlockNum, HoldTBlockNum;
        private static int []pocket = new int[8];
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
        private static Random R = new Random();
        public static void Initialize()
        {

            int i;
            for(i=0;i<7;i++) pocket[i]=1;
            return;
        }
        public static void Read()
        {
            int ava=0,i,t;
            for(i=0;i<7;i++) ava+=pocket[i];
            if (ava == 0)
            {
                for(i=0;i<7;i++) pocket[i]=1;
                ava=7;
            }
            t=Math.Abs(R.Next())%ava+1;
            for (i = 0; i < 7; i++)
            {
                if(pocket[i]==1) t--;
                if(t==0) break;    
            }
            pocket[i]=0;
            ava--;
            CurTBlockNum=i;
            return;
        }
    }
}
