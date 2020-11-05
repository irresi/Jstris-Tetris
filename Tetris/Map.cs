using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    static class Map
    {
        public static int[,] map = new int[30, 15];
        public static int[,] nextmap = new int[30, 15];
        public static int[,] bestmap = new int[30, 15];
        public static int maxHeight=0;
        public static bool Died=false;
        static Map()
        {
            int i;
            for (i = 0; i <= 20; i++) map[i, 0] = map[i, 11] = 1;
            for (i = 0; i <= 10; i++) map[0, i] = 1;

        }
        public static bool IsDied() //나중에 구현해야 됨.
        {
            return Died;
        }
        public static int cll() //실제로 맵을 클리어 (오프라인 학습 위해)
        {
            int i, j, top = 1, ret = 0;
            //RawPrint();
            for (i = 1; i <= 20; i++)
            {
                for (j = 1; j <= 10; j++) if (map[i, j] == 0) break;
                if (j > 10)
                {
                    ret++;
                    continue; //clear
                }
                for (j = 1; j <= 10; j++)
                {
                    map[top, j] = map[i, j];
                }
                top++;
                //RawPrint(); //debug
            }
            for (; top <= 20; top++)
            {
                for (j = 1; j <= 10; j++) map[top, j] = 0;
            }
            return ret;
        }
        public static void Read()
        { //Read map from screen
          //516,228
          /*int i, j;
          Color T;
          for (i = 0; i < 20; i++)
          {
              for (j = 0; j < 10; j++)
              {
                  // The the pixel color information from next Tetris Block at Web
                  T = Getpixel.GetColorAt(new Point(602 + j * 24, 184 + i * 24));
                  if (T.R+T.G+T.B>200) map[i, j] = 1;
                  else map[i, j] = 0;
              }
          }
          return;*/
            var start_x = 590;
            var start_y = 170;
            var term = 24;

            Bitmap bitmap = new Bitmap(240, 480);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.CopyFromScreen(new Point(start_x, start_y), new Point(0, 0), new Size(240, 480));
            }

            var y = 12;
            for (var i = 1; i <= 20; i++)
            {
                var x = 12;
                if (i > 1) { 
                    for (var j = 1; j <= 10; j++)
                    {
                        Color pix = bitmap.GetPixel(x, y);

                        if (pix.R + pix.G + pix.B < 200)
                        {
                            map[21-i, j] = 0;
                        }
                        else if(pix.R==106 && pix.G==106 && pix.B == 106) {
                            map[21-i,j]=-1; //클리어로 처리하지 않는 줄
                            //Died=true;
                        }
                        else {
                            map[21-i, j] = 1;
                        }
                        // 만약 픽셀이 0,0,0 이면
                        x += term;
                    }
                }
                y += term;
                // Console.WriteLine("")
            }
            // Console.WriteLine("==========================")
            // Console.Write(result)
        }
        public static void Print()
        { //문제 : 떨어지는 테트리스를 없애야 함. 맨 위에 5칸 정도 읽지 말기?
            int i, j;
            for (i = 20; i>=1; i--)
            {
                for (j = 1; j <=10; j++)
                {
                    if (map[i, j] > 0) Console.Write("■");
                    else Console.Write("□");
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
    }
}
