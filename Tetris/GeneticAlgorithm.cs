using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;

/*
테트리스 개체 30개 중 1개 선택
	테트리스 다음 블럭 선정(7종이 한 묶음으로 나옴)
	테트리스를 놓을 위치를 선정
	테트리스 놓기 
*/
/*
테트리스 봇을 따로 학습
20Line
1vs1 
2P+
넥스트와 홀드 고려
쓰레기 줄을 공격 받는 것 고려 그냥 jstris에서 pvp로 학습, 게임 시작 되는 것 어떻게 알음? 준비, 시작 할 때 픽셀 값의 색깔로 추정

가중치가 높아진다(좋다)
    
    공격할 수 있는 줄 수 
    블럭을 배치할 때 맞닿은 면의 개수
    벽과 인접한 블럭의 개수? (* 0.1정도)

가중치가 낮아진다(안 좋다)
    블럭 밑에 있는 빈 공간의 개수
    줄의 최대 높이 
    양 옆 블럭 차이^2 (*1~2정도)

점수 : 놓은 블럭 수 + 공격한 줄 수*10

           12345678910
        20  □□□□□□□□□□
        19  □□□□□□□□□□
        18  □□□□□□□□□□
        17  □□□□□□□□□□
        16  □□□□□□□□□□
        15  □□□□□□□□□□
        14  □□□□□□□□□□
        13  □□□□□□□□□□
        12  □□□□□□□□□□
        11  □□□□□□□□□□
        10  □□□□□□□□□□
        9   □□□□□□□□□□
        8   □□□□□□□□□□
        7   □□□□□□□□□□
        6   □□□□□□□□□□
        5   □□□□□□□□□□
        4   □□□□□□□□□□
        3   □□□□□□□□□□
        2   □□□□□□□□□□
        1   □□□□□□□□□□

올클리어 고려? T 스핀 고려는 나중에 다 만들고
(686,466) 에 203,214,0이 나타났다가 사라지면 0.5초 후에 시작

벽은 -1, 빈 칸은 0, 현재 놓인 테트리스는 1, 앞으로 놓을 테트리스는 2
*/
namespace Tetris
{
    delegate int Intdel();
    static class GeneticAlgorithm //유전 알고리즘을 이용해 테트리스 학습
    {

        public static List<Intdel> GetWeights = new List<Intdel> { }; //가중치 함수를 저장하는 델리게이트 리스트 !
        public static List<double>[] l = new List<double>[33];
        public static List<double>[] nl = new List<double>[33];
        public static ArrayList F = new ArrayList(); //
        public static double[] Res = new double[33];
        public static double BestWeight;
        public static int BestMove, BestRotate,show;
        public static long[] SumUp = new long[33];
        private static int MaxGene;

        static public void Start()
        {

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("______________________________");
            Console.WriteLine("  1. 20Line 학습");
            Console.WriteLine("  2. 1 vs 少 학습");
            Console.WriteLine("  3. 1 vs 多 학습");
            Console.WriteLine("______________________________");
            while (true)
            {
                if (Console.ReadKey(true).Key == ConsoleKey.D1)
                {
                again:
                    Console.Write("\n학습할 세대 수 입력 : ");
                    try
                    {
                        MaxGene = int.Parse(Console.ReadLine());
                    }
                    catch
                    {
                        goto again;
                    }
                    Console.Write("\n시각화? ");
                    try
                    {
                        show = int.Parse(Console.ReadLine());
                    }
                    catch
                    {
                        goto again;
                    }
                    /*
                     20Line
                    클리어할 수 있는 줄 수
                    블럭을 배치할 때 맞닿은 면의 개수
                    벽과 인접한 블럭의 개수 ? (*0.1정도)
                    
                    빈 공간 위에 있는 블럭의 개수
                    줄의 최대 높이 
                    양 옆 블럭 차이^3 (*0.5정도)
                    */

                    ///Get Weight에 함수 담아서 가중치 얻어야 함.
                    P1();
                    break;
                }
                else if (Console.ReadKey(true).Key == ConsoleKey.D2)
                {
                    again2:
                    Console.Write("\n학습할 세대 수 입력 : ");
                    try
                    {
                        MaxGene = int.Parse(Console.ReadLine());
                    }
                    catch
                    {
                        goto again2;
                    }
                    P2();
                    //Solve()
                    break;
                }

            }
            //Data.txt에는 가장 좋은 개체 30개의 가중치 계수 저장

        }
        public struct data
        {
            public int i;
            public long num;
            public data(int i, long num)
            {
                this.i = i;
                this.num = num;
            }
        }
        public static void P1()
        {
            double TempWeight;
            //5.0 0.5 0.2 -8.0 -0.5 -0.5 default
            GetWeights.Add(TWeight.ClearLine);
            GetWeights.Add(TWeight.AdjacentWall);
            GetWeights.Add(TWeight.Adjacent);
            GetWeights.Add(TWeight.BlockEmpty);
            GetWeights.Add(TWeight.EmptyBlock);
            GetWeights.Add(TWeight.MaxHeight);
            GetWeights.Add(TWeight.fourway);
            GetWeights.Add(TWeight.threeway);

            int i, j, k, ii, jj, kk, tc, t, top = 0, flag = 0, godown, sel, si, sj;
            List<Point> ls = new List<Point>();
            List<data> Rank = new List<data>();

            Random r = new Random();

            //가중치가 없어서 잠시 1로
            top = 30;
            int Gene = 1, cnt = 0, tom, maxcnt = 0;
            int defaultinp=0;
            double p;
            while (Gene <= MaxGene)
            { //입력받은 세대 수 보다 현재 세대가 작은 동안
                //initialize
                maxcnt = 0;
                defaultinp=0;
                Rank.Clear();
                StreamReader sr = new StreamReader(@"1P.txt");
                for (i = 0; i < 30; i++)
                {
                    SumUp[i] = 0;
                    nl[i] = new List<double>();
                    l[i] = new List<double>();
                }
                top = 0;
                while (sr.Peek() >= 0 && top < 30)
                {
                    defaultinp=1;
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
                    top++;
                } //파일 입력
                if (defaultinp==0)
                {
                    Console.Write("NO INPUT");
                    for (top = 0; top < 30; top++)
                    {
                        for (j = 0; j < 8; j++)
                        {
                            l[top].Add(r.NextDouble()*10);
                        }
                    }
                }
                sr.Close();

                for (tc = 0; tc < top; tc++) //개체 수가 최대 개체 수 보다 작으 동안
                {
                    if(show!=0) Console.Clear();
                    TMap.Initialize();

                    TMap.maxheight = 20;
                    if (show != 0) { 
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.SetCursorPosition(0, 0);
                        for (i = 1; i <= 12; i++) Console.Write("■");
                        TMap.Print(); //debug
                        Console.ForegroundColor = ConsoleColor.Gray;
                        for (i = 1; i <= 12; i++) Console.Write("■");
                        TMap.maxheight = 0;
                        Console.SetCursorPosition(2, 22);
                        Console.WriteLine(Gene + "세대, " + tc + "번째 개체 : ");
                    }
                    while (!TMap.isDied())
                    {
                        ///Thread.Sleep(50); ////set set set
                        TBlock.Read();
                        PlayOneBlock(tc);
                        cnt++;
                        tom = TMap.cll();
                        //if(tom>0) TMap.Print();
                        TMap.maxheight -= tom;
                        
                        if (show!=0)
                        {
                            TMap.ColorSet(0);
                            Console.SetCursorPosition(20, 22);
                            Console.Write(cnt);
                        }
                    }

                    //Thread.Sleep(203); //show
                    if (show!=0) { 
                        Console.SetCursorPosition(12, 23);
                        Console.Write("DIED");
                    }
                    Rank.Add(new data(tc, cnt));
                    if (tc != 0) SumUp[tc] += SumUp[tc - 1];
                    SumUp[tc] += cnt;
                    if (maxcnt < cnt) maxcnt = cnt;
                    cnt = 0;
                }
                Console.Write("\n 최고 기록 : " + maxcnt);
                Rank.Sort(delegate (data A, data B) //점수 내림차순으로 정렬
                {
                    if (A.num < B.num) return 1;
                    else if (A.num > B.num) return -1;
                    return 0;
                });
                tc = 0;
                //유전자 변이 ! 
                //1. 가장 좋은 해 
                for (i = 0; i < 5; i++)
                {
                    for (k = 0; k < l[i].Count(); k++)
                    {
                        si=Rank[i].i;
                        nl[tc].Add(l[si][k]);
                    }
                    tc++;
                }
                //2. 룰렛 휠 교차
                for (i = 0; i < 5; i++)
                {
                    for (j = 0; j < i; j++)
                    {
                        for (k = 0; k < l[i].Count(); k++)
                        {
                            si = Rank[i].i; sj = Rank[j].i;
                            p = (double)Rank[si].num / (Rank[si].num + Rank[sj].num); //랭크가 높은 것이 더 많은 영향 미침.
                            nl[tc].Add(l[si][k] * p + l[sj][k] * (1 - p));
                        }
                        tc++;
                    }
                }
                //3. 변이 
                for (i = 0; i < 15; i++)
                {
                    t = r.Next(1, (int)SumUp[top - 1]);
                    for (j = 0; j < top; j++)
                    {
                        if (t < SumUp[j]) break;
                    }
                    if (j == top) sel = 1;
                    sel = j;
                    sel = Rank[sel].i;
                    /*if (SumUp[j]-t <= 100) //변이 , 처음에는 변이할 확률이 높고 점점 변이할 확률이 낮아지는 방식
                    {*/
                    for (k = 0; k < l[sel].Count(); k++)
                    {
                        p = 0.5 + (r.NextDouble() + r.NextDouble() + r.NextDouble() + r.NextDouble()) / 4; //0.5~1.5배로 변이
                        nl[tc].Add(l[sel][k] * p);
                    }
                    tc++;
                    continue;
                    /*}
                    sel=j;
                    nl[tc++]=l[sel];*/
                }
                Gene++;
                FileIO.FWrite(1);
            }
            Console.Write("END");
            return;
        }

        public static void P2()
        {
            double TempWeight;
            //5.0 0.5 0.2 -8.0 -0.5 -0.5 default

            GetWeights.Add(TWeight.AttackLine);
            GetWeights.Add(TWeight.AdjacentWall);
            GetWeights.Add(TWeight.Adjacent);
            GetWeights.Add(TWeight.BlockEmpty);
            GetWeights.Add(TWeight.EmptyBlock);
            GetWeights.Add(TWeight.MaxHeight);
            GetWeights.Add(TWeight.fourway);
            GetWeights.Add(TWeight.threeway);

            int i, j, k, ii, jj, kk, tc, t, top = 0, flag = 0, godown, sel, si, sj;
            List<Point> ls = new List<Point>();
            List<data> Rank = new List<data>();

            Random r = new Random();

            //가중치가 없어서 잠시 1로
            top = 30;
            int Gene = 1, tom;
            long cnt=0,maxcnt=0;
            double p;
            while (Gene <= MaxGene)
            { //입력받은 세대 수 보다 현재 세대가 작은 동안
                //initialize
                maxcnt = 0;
                Rank.Clear();
                StreamReader sr = new StreamReader(@"2P.txt");
                for (i = 0; i < 30; i++)
                {
                    SumUp[i] = 0;
                    nl[i] = new List<double>();
                    l[i] = new List<double>();
                }
                top = 0;
                while (sr.Peek() >= 0 && top < 30)
                {
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
                    top++;
                } //파일 입력
                sr.Close();

                for (tc = 0; tc < top; tc++) //개체 수가 최대 개체 수 보다 작으 동안
                {
                    Console.Clear();
                    TMap.Initialize();

                    TMap.maxheight = 20;
                    Console.ForegroundColor = ConsoleColor.Gray;
                    
                    Console.SetCursorPosition(0, 0);
                    for (i = 1; i <= 12; i++) Console.Write("■");
                    if (show != 0) TMap.Print(); //debug
                    Console.ForegroundColor = ConsoleColor.Gray;
                    for (i = 1; i <= 12; i++) Console.Write("■");
                    TMap.maxheight = 0;
                    Console.SetCursorPosition(2, 22);
                    //Print
                    //Console.WriteLine(Gene + "세대, " + tc + "번째 개체 : ");
                    while (!TMap.isDied())
                    {
                        Thread.Sleep(300); ////set set set
                        TBlock.Read();
                        PlayOneBlock(tc);
                        cnt--;
                        cnt+=TWeight.AttackLine()*30;
                        tom = TMap.cll();
                        if(show!=0 && tom>0) TMap.Print();
                        TMap.maxheight -= tom;

                        TMap.ColorSet(0);
                        Console.SetCursorPosition(20, 22);
                        Console.Write(cnt + "   ");


                    }
                    cnt/=30;
                    //Thread.Sleep(203); //show
                    if (show != 0) { 
                        Console.SetCursorPosition(12, 23);
                        Console.Write("DIED");
                    }
                    Rank.Add(new data(tc, cnt));
                    if (tc != 0) SumUp[tc] += SumUp[tc - 1];
                    SumUp[tc] += cnt;
                    if (maxcnt < cnt) maxcnt = cnt;
                    cnt = 0;
                }
                //Print
                Console.Write("\n 최고 기록 : " + maxcnt);
                Rank.Sort(delegate (data A, data B) //점수 내림차순으로 정렬
                {
                    if (A.num < B.num) return 1;
                    else if (A.num > B.num) return -1;
                    return 0;
                });
                tc = 0;
                //유전자 변이 ! 
                //1. 가장 좋은 해 5개 교차  
                /*for (i = 0; i < 3; i++)
                {
                    nl[tc++]=
                }*/
                for (i = 0; i < 5; i++)
                {
                    for (k = 0; k < l[i].Count(); k++)
                    {
                        si = Rank[i].i;
                        nl[tc].Add(l[si][k]);
                    }
                    tc++;
                }
                for (i = 0; i < 5; i++)
                {
                    for (j = 0; j < i; j++)
                    {
                        for (k = 0; k < l[i].Count(); k++)
                        {
                            si = Rank[i].i; sj = Rank[j].i;
                            p = (double)Rank[si].num / (Rank[si].num + Rank[sj].num); //랭크가 높은 것이 더 많은 영향 미침.
                            nl[tc].Add(l[si][k] * p + l[sj][k] * (1 - p));
                        }
                        tc++;
                    }
                }
                //요소간 룰렛 휠 선택 (5개)
                //1. 룰렛 휠 선택 & 변이 (10개)
                for (i = 0; i < 15; i++)
                {
                    t = r.Next(1, (int)SumUp[top - 1]);
                    for (j = 0; j < top; j++)
                    {
                        if (t < SumUp[j]) break;
                    }
                    if (j == top) sel = 1;
                    sel = j;
                    sel = Rank[sel].i;
                    /*if (SumUp[j]-t <= 100) //변이 , 처음에는 변이할 확률이 높고 점점 변이할 확률이 낮아지는 방식
                    {*/
                    for (k = 0; k < l[sel].Count(); k++)
                    {
                        p = 0.5 + (r.NextDouble() + r.NextDouble() + r.NextDouble() + r.NextDouble()) / 4; //0.5~1.5배로 변이
                        nl[tc].Add(l[sel][k] * p);
                    }
                    tc++;
                    continue;
                    /*}
                    sel=j;
                    nl[tc++]=l[sel];*/
                }
                Gene++;
                FileIO.FWrite(2);
            }
            return;
        }




        private static void PlayOneBlock(int tc)
        //HoldTBlock을 사용하는 경우와 수 깊이 추가해야 됨
        {

            double TempWeight = -1e9;
            BestWeight = -1e9;

            int i, j, k, ii, jj, kk, top = 0, flag = 0, godown;
            List<Point> ls = new List<Point>(), bs = new List<Point>();


            //CurTBlock을 사용하는 경우
            for (i = -1; i <= 9; i++)
            {
                for (j = 0; j < 4; j++)
                {
                    if (TBlock.shape[TBlock.CurTBlockNum, j, 0, 0] == -1) continue;
                    flag = 1;
                    for (k = 0; k < 4; k++)
                    {
                        for (ii = 0; ii < 4; ii++)
                        {
                            if (TBlock.shape[TBlock.CurTBlockNum, j, k, ii] == 1)
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
                                if (TBlock.shape[TBlock.CurTBlockNum, j, k, ii] == 1)
                                {
                                    //godown - k - 1<0은 테스트때는 추가 안 했었던 것 같음
                                    if (godown < 1 || godown - k - 1 < 0 || TMap.map[godown - k - 1, i + ii] == 1) flag = -1;
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
                            if (TBlock.shape[TBlock.CurTBlockNum, j, k, ii] == 1)
                            {
                                TMap.map[godown - k, i + ii] = 2; //임시로 테트리스 놓기
                                ls.Add(new Point(godown - k, i + ii));
                            }
                        }
                    }
                    //TMap.Print();
                    TempWeight = GetW(tc);
                    if (TMap.isDied()) TempWeight -= 1000000;
                    if (BestWeight < TempWeight)
                    {
                        BestWeight = TempWeight;
                        BestMove = i;
                        BestRotate = j;
                        bs.Clear();
                        foreach (Point p in ls)
                        {
                            TMap.map[p.X, p.Y] = 1;
                            bs.Add(p); //복사를 어떻게 하는 지 까먹어서
                        }
                        TMap.bestmap = (int[,])TMap.map.Clone();
                        //TMap.Print();
                    }
                    //가중치 얻고 가장 좋은 것 저장하기
                    foreach (Point p in ls)
                    {
                        TMap.map[p.X, p.Y] = 0;
                    }
                    ls.Clear();
                }
            }
             //여기서 Print
            foreach (Point p in bs)
            {
                TMap.col[p.X, p.Y] = TBlock.CurTBlockNum;
                if (20 - p.X + 1 < 0) continue;
                //Print 
                if (show != 0) { 
                Console.SetCursorPosition((p.Y - 1) * 2 + 2, 20 - p.X + 1); //??????
     
                TMap.ColorSet(TBlock.CurTBlockNum);
                Console.Write("■");
                }
                
                if (p.X > TMap.maxheight) TMap.maxheight = p.X;
            }
            TMap.map = TMap.bestmap;

            //가장 좋은 블럭을 Tmap에 실제로 넣기
            /* Winform을 공부하고 난 다음에 실제로 움직이게 하기
             //move blocks(rotate and down)
            if (BestMove == 1) SendKeys.Send("{UP}");
            if (BestMove == 2) SendKeys.Send("a");
            if (BestMove == 3) SendKeys.Send("z");
            string s = "";
            if (BestMove < 4)
            {
                while (BestMove < 4)
                {
                    s += "{LEFT}";
                    BestMove++;
                }
            }
            else if (BestMove > 4)
            {
                while (BestMove > 4)
                {
                    s += "{RIGHT}";
                    BestMove--;
                }
            }
            Console.Write(ConsoleKey.LeftArrow);
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
    }
}

