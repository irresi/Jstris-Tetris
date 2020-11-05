using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Runtime.InteropServices;

namespace Tetris
{
    static class TMap
    {
        public static int[,] map = new int[30, 15];
        public static int[,] bestmap = new int[30, 15];
        public static int[,] col = new int[30,15];
        public static int maxheight;
        static TMap()
        {
            int i;
            for (i = 0; i <= 20; i++) map[i, 0] = map[i, 11] = 1;
            for (i = 0; i <= 10; i++) map[0, i] = 1;
        }
        public static void ColorSet(int num)
        {
            switch (num)
                    {
                        case 0:
                            Console.ForegroundColor = ConsoleColor.White;
                            break;
                        case 1: 
                            Console.ForegroundColor = ConsoleColor.Blue;
                            break;
                        case 2:
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            break;
                        case 3:
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            break;
                        case 4:
                            Console.ForegroundColor = ConsoleColor.Green;
                            break;
                        case 5:
                            Console.ForegroundColor = ConsoleColor.Red;
                            break;
                        case 6:
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            break;
                        case 7:
                            Console.ForegroundColor = ConsoleColor.Blue;
                            break;
                    }
        }
        public static void Initialize()
        {
            int i,j;
            for (i = 0; i < 30; i++)
            {
                for(j=0;j<15;j++) col[i,j]=map[i,j]=0;
            }
            for (i = 0; i <= 20; i++) map[i, 0] = map[i, 11] = 1;
            for (i = 0; i <= 10; i++) map[0, i] = 1;
        }
        public static bool isDied()
        {
            int i;
            for (i = 1; i <= 10; i++)
            {
                if(map[21,i]!=0) return true;
            }
            return false;
        }
        public static void Read()
        {
            return;
        }
        public static void Print()
        { //문제 : 떨어지는 테트리스를 없애야 함. 맨 위에 5칸 정도 읽지 말기?
            int i, j;
            //Console.Clear();
            Console.SetCursorPosition(0, 20 - maxheight+1);
            Console.ForegroundColor = ConsoleColor.Gray;
            for (i = maxheight; i >= 1; i--)
            {
                Console.ForegroundColor=ConsoleColor.Gray;
                Console.Write("■");
                for (j = 1; j <= 10; j++)
                {
                    ColorSet(col[i,j]);
                    if (map[i, j] > 0) Console.Write("■");
                    else Console.Write("□");
                }
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("■"); 
                Console.WriteLine();
            }
            return;
        }
        public static void RawPrint()
        { //문제 : 떨어지는 테트리스를 없애야 함. 맨 위에 5칸 정도 읽지 말기?
            int i, j;
            for (i = 21; i >= 0; i--)
            {
                for (j = 0; j <= 11; j++)
                {
                    Console.Write(map[i,j]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            return;
        }
        public static int GetWeight()
        { //Return nextmap weight
            int i, j;
            return 0;
        }
        public static int cll() //실제로 맵을 클리어 (오프라인 학습 위해)
        {
            int i, j,top=1,ret=0;
            //RawPrint();
            for (i = 1; i <=20; i++)
            {
                for (j = 1; j <= 10; j++) if (map[i, j] == 0) break;
                if (j > 10)
                {
                    ret++;
                    continue; //clear
                }
                for(j=1;j<=10;j++) {
                    map[top,j]=map[i,j];
                    col[top,j]=col[i,j];
                }
                top++;
                //RawPrint(); //debug
            }
            for (; top <=20; top++)
            {
                for(j=1;j<=10;j++) map[top,j]=col[top,j]=0;
            }
            return ret;
        }
    }
}
