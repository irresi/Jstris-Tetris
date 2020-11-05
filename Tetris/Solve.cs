using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using static Tetris.GeneticAlgorithm;

namespace Tetris
{
    class Solve
    {

        public static Random r = new Random();
        public static void start() //jstris에서 플레이
        {
            Console.Clear();

            Console.WriteLine("______________________________");
            Console.WriteLine("  1. 20Line 플레이");
            Console.WriteLine("  2. 1 vs 少 플레이");
            Console.WriteLine("  3. 1 vs 多 플레이");
            Console.WriteLine("______________________________");
            int k = 0;
            while (true)
            {
                if (Console.ReadKey(true).Key == ConsoleKey.D1)
                {
                    k = 1;
                    break;
                }
                if (Console.ReadKey(true).Key == ConsoleKey.D2)
                {
                    k = 2;
                    break;
                }
                if (Console.ReadKey(true).Key == ConsoleKey.D3)
                {
                    k = 3;
                    break;
                }
            }
            Console.WriteLine(k + " selected");
            Color c = new Color();
            while (!(c.R == 203 && c.G == 214 && c.B == 0))
            {
                c = Getpixel.GetColorAt(new Point(704, 473));
                Thread.Sleep(100);
            }

            Console.WriteLine("시작\n");
            if (k == 1) P1();
            if (k == 2) P2();
            if (k == 3) P3();

            return;
        }

        private static void P3()
        {
            throw new NotImplementedException();
        }

        private static void P1()
        {
            double TempWeight;
            //5.0 0.5 0.2 -8.0 -0.5 -0.5 default
            //
            GetWeights.Add(Weight.ClearLine);
            GetWeights.Add(Weight.AdjacentWall);
            GetWeights.Add(Weight.Adjacent);
            GetWeights.Add(Weight.BlockEmpty);
            GetWeights.Add(Weight.MaxHeight);
            GetWeights.Add(Weight.fourway);
            GetWeights.Add(Weight.threeway);


            int i, j, k, ii, jj, kk, tc, t, top = 0, flag = 0, godown, sel, si, sj, tom = 0;
            List<Point> ls = new List<Point>();
            List<data> Rank = new List<data>();
            StreamReader sr = new StreamReader(@"1PBEST.txt");
            l[0] = new List<double>();
            foreach (string s in sr.ReadLine().Split())
            {
                try
                {
                    // Console.WriteLine((double)Convert.ToDouble(s));
                    l[top].Add(Convert.ToDouble(s));
                }
                catch (Exception e)
                {
                    //Console.WriteLine(e);
                }
            }
            sr.Close();
            Block.Read();
            Thread.Sleep(1700);
            while (!Map.IsDied())
            {
                PlayOneBlock(0);
                Map.cll();
                //Map.Print();
                Thread.Sleep(30+r.Next(1,5));

            }
        }

        private static void P2()
        {
            double TempWeight;
            //5.0 0.5 0.2 -8.0 -0.5 -0.5 default
            //


            GetWeights.Add(Weight.ClearLine);
            GetWeights.Add(Weight.AdjacentWall);
            GetWeights.Add(Weight.Adjacent);
            GetWeights.Add(Weight.BlockEmpty);
            GetWeights.Add(Weight.MaxHeight);
            GetWeights.Add(Weight.fourway);
            GetWeights.Add(Weight.threeway);
            GetWeights.Add(Weight.AttackLine);


            //GetWeights.Add(TWeight.AttackLine);
            //GetWeights.Add(TWeight.AdjacentWall);
            //GetWeights.Add(TWeight.Adjacent);
            //GetWeights.Add(TWeight.BlockEmpty);
            //GetWeights.Add(TWeight.EmptyBlock);
            //GetWeights.Add(TWeight.MaxHeight);
            //GetWeights.Add(TWeight.fourway);
            //GetWeights.Add(TWeight.threeway);

            int i, j, k, ii, jj, kk, tc, t, top = 0, flag = 0, godown, sel, si, sj, tom = 0;
            List<Point> ls = new List<Point>();
            List<data> Rank = new List<data>();
            StreamReader sr = new StreamReader(@"2PBEST.txt");
            l[0] = new List<double>();
            foreach (string s in sr.ReadLine().Split())
            {
                try
                {
                    // Console.WriteLine((double)Convert.ToDouble(s));
                    l[top].Add(Convert.ToDouble(s));
                }
                catch (Exception e)
                {
                    //Console.WriteLine(e);
                }
            }
            sr.Close();
            Thread.Sleep(1700);
            while (!Map.IsDied())
            {
                Map.Read();
                Block.TRead();
                PlayOneBlock2(0);
                //Map.Print();
                //Map.cll();
                Thread.Sleep(28);
            }
        }


        //GeneticAlgorithm.cs에서 복사해서 고쳐야 됨.
        private static void PlayOneBlock(int tc)
        //HoldTBlock을 사용하는 경우와 수 깊이 추가해야 됨
        {

            double TempWeight = -1e9;
            //Block.Read();
            //Map.Read();
            BestWeight = -1e9;

            int i, j, k, ii, jj, kk, top = 0, flag = 0, godown;
            List<Point> ls = new List<Point>(), bs = new List<Point>();


            //CurBlock을 사용하는 경우
            for (i = -1; i <= 9; i++)
            {
                for (j = 0; j < 4; j++)
                {
                    if (Block.shape[Block.CurBlockNum, j, 0, 0] == -1) continue;
                    flag = 1;
                    for (k = 0; k < 4; k++)
                    {
                        for (ii = 0; ii < 4; ii++)
                        {
                            if (Block.shape[Block.CurBlockNum, j, k, ii] == 1)
                            {
                                if (ii + i < 1 || ii + i > 10) flag = -1;
                            }
                        }
                    }
                    if (flag == -1) continue;
                    for (godown = 24; godown >= 1; godown--)
                    {
                        for (k = 0; k < 4; k++)
                        {
                            for (ii = 0; ii < 4; ii++)
                            {
                                if (Block.shape[Block.CurBlockNum, j, k, ii] == 1)
                                {
                                    //godown - k - 1<0은 테스트때는 추가 안 했었던 것 같음
                                    if (godown < 1 || godown - k - 1 < 0 || Map.map[godown - k - 1, i + ii] == 1) flag = -1;
                                }
                                if (flag == -1) break;
                            }
                            if (flag == -1) break;
                        }
                        if (flag == -1) break;
                    }
                    for (k = 0; k < 4; k++)
                    {
                        for (ii = 0; ii < 4; ii++)
                        {
                            if (Block.shape[Block.CurBlockNum, j, k, ii] == 1)
                            {
                                Map.map[godown - k, i + ii] = 2; //임시로 테트리스 놓기
                                ls.Add(new Point(godown - k, i + ii));
                            }
                        }
                    }
                    //TMap.Print();
                    TempWeight = GetW(tc);
                    //if (TMap.isDied()) TempWeight -= 1000000;
                    if (BestWeight < TempWeight)
                    {
                        BestWeight = TempWeight;
                        BestMove = i;
                        BestRotate = j;
                        bs.Clear();
                        foreach (Point p in ls)
                        {
                            Map.map[p.X, p.Y] = 1;
                            bs.Add(p); //복사를 어떻게 하는 지 까먹어서
                        }
                        //TMap.Print();
                    }
                    //가중치 얻고 가장 좋은 것 저장하기
                    foreach (Point p in ls)
                    {
                        Map.map[p.X, p.Y] = 0;
                    }
                    ls.Clear();
                }
            }
            foreach (Point p in bs)
            {
                Map.map[p.X, p.Y] = 1;
            }

            //가장 좋은 블럭을 Tmap에 실제로 넣기
            // Winform을 공부하고 난 다음에 실제로 움직이게 하기
            //move blocks(rotate and down)
            Block.Read();
            //Thread.Sleep(100);
            string s = "";
            int wt = 5;
            if (BestRotate == 1)
            {
                //s+="{UP}";
                SendKeys.SendWait("{UP}");
                SendKeys.Flush();
                Thread.Sleep(wt + r.Next(0, 2));
            }
            if (BestRotate == 2)
            {
                //s+="a";
                SendKeys.SendWait("a");
                SendKeys.Flush();
                Thread.Sleep(wt + r.Next(0, 2));
            }
            if (BestRotate == 3)
            {
                //s+="z";
                SendKeys.SendWait("z");
                SendKeys.Flush();
                Thread.Sleep(wt + r.Next(0, 2));
            }
            if (BestMove < 4)
            {
                while (BestMove < 4)
                {
                    //s += "{LEFT}";
                    BestMove++;
                    SendKeys.SendWait("{LEFT}");
                    Thread.Sleep(wt + r.Next(0, 2));
                    SendKeys.Flush();
                }
            }
            else if (BestMove > 4)
            {
                while (BestMove > 4)
                {
                    //s += "{RIGHT}";
                    SendKeys.SendWait("{RIGHT}");
                    Thread.Sleep(wt + r.Next(0, 2));
                    SendKeys.Flush();
                    BestMove--;
                }
            }
            s += " ";
            SendKeys.SendWait(s);
            SendKeys.Flush();
            Thread.Sleep(wt + r.Next(0, 2));
            //Console.WriteLine(s);
            //LeftArrow
            //SendKeys.Send(s);*/
        }

        //public static List<Intdel> GetWeights = new List<Intdel> { }; //가중치 함수를 저장하는 델리게이트 리스트 !
        //public static List<double>[] l = new List<double>[33];
        private static double GetW(int num)
        {
            int i, j;
            double ret = 0;
            //TMap.RawPrint();
            for (i = 0; i < GetWeights.Count; i++)
            {
                ret += l[num][i] * GetWeights[i]();
            }
            return ret;
        }





        private static void PlayOneBlock2(int tc)
        //HoldTBlock을 사용하는 경우와 수 깊이 추가해야 됨
        {

            double TempWeight = -1e9;
            //Block.Read();
            //Map.Read();
            BestWeight = -1e9;

            int i, j, k, ii, jj, kk, top = 0, flag = 0, godown;
            List<Point> ls = new List<Point>(), bs = new List<Point>();


            //CurBlock을 사용하는 경우
            for (i = -1; i <= 9; i++)
            {
                for (j = 0; j < 4; j++)
                {
                    if (Block.shape[Block.CurBlockNum, j, 0, 0] == -1) continue;
                    flag = 1;
                    for (k = 0; k < 4; k++)
                    {
                        for (ii = 0; ii < 4; ii++)
                        {
                            if (Block.shape[Block.CurBlockNum, j, k, ii] == 1)
                            {
                                if (ii + i < 1 || ii + i > 10) flag = -1;
                            }
                        }
                    }
                    if (flag == -1) continue;
                    for (godown = 24; godown >= 1; godown--)
                    {
                        for (k = 0; k < 4; k++)
                        {
                            for (ii = 0; ii < 4; ii++)
                            {
                                if (Block.shape[Block.CurBlockNum, j, k, ii] == 1)
                                {
                                    //godown - k - 1<0은 테스트때는 추가 안 했었던 것 같음
                                    if (godown < 1 || godown - k - 1 < 0 || Map.map[godown - k - 1, i + ii] == 1) flag = -1;
                                }
                                if (flag == -1) break;
                            }
                            if (flag == -1) break;
                        }
                        if (flag == -1) break;
                    }
                    for (k = 0; k < 4; k++)
                    {
                        for (ii = 0; ii < 4; ii++)
                        {
                            if (Block.shape[Block.CurBlockNum, j, k, ii] == 1)
                            {
                                Map.map[godown - k, i + ii] = 2; //임시로 테트리스 놓기
                                ls.Add(new Point(godown - k, i + ii));
                            }
                        }
                    }
                    //TMap.Print();
                    TempWeight = GetW(tc);
                    //if (TMap.isDied()) TempWeight -= 1000000;
                    if (BestWeight < TempWeight)
                    {
                        BestWeight = TempWeight;
                        BestMove = i;
                        BestRotate = j;
                        bs.Clear();
                        foreach (Point p in ls)
                        {
                            Map.map[p.X, p.Y] = 1;
                            bs.Add(p); //복사를 어떻게 하는 지 까먹어서
                        }
                        //TMap.Print();
                    }
                    //가중치 얻고 가장 좋은 것 저장하기
                    foreach (Point p in ls)
                    {
                        Map.map[p.X, p.Y] = 0;
                    }
                    ls.Clear();
                }
            }
            foreach (Point p in bs)
            {
                Map.map[p.X, p.Y] = 1;
            }

            //가장 좋은 블럭을 Tmap에 실제로 넣기
            // Winform을 공부하고 난 다음에 실제로 움직이게 하기
            //move blocks(rotate and down)
            //Thread.Sleep(100);
            string s = "";
            int wt = 4;
            if (BestRotate == 1)
            {
                //s+="{UP}";
                SendKeys.SendWait("{UP}");
                SendKeys.Flush();
                Thread.Sleep(wt);
            }
            if (BestRotate == 2)
            {
                //s+="a";
                SendKeys.SendWait("a");
                SendKeys.Flush();
                Thread.Sleep(wt);
            }
            if (BestRotate == 3)
            {
                //s+="z";
                SendKeys.SendWait("z");
                SendKeys.Flush();
                Thread.Sleep(wt);
            }
            if (BestMove < 4)
            {
                while (BestMove < 4)
                {
                    //s += "{LEFT}";
                    BestMove++;
                    SendKeys.SendWait("{LEFT}");
                    Thread.Sleep(wt);
                    SendKeys.Flush();
                }
            }
            else if (BestMove > 4)
            {
                while (BestMove > 4)
                {
                    //s += "{RIGHT}";
                    SendKeys.SendWait("{RIGHT}");
                    Thread.Sleep(wt);
                    SendKeys.Flush();
                    BestMove--;
                }
            }
            s += " ";
            SendKeys.SendWait(s);
            SendKeys.Flush();
            Thread.Sleep(wt);
            //Console.WriteLine(s);
            //LeftArrow
            //SendKeys.Send(s);*/
        }
    }

}
