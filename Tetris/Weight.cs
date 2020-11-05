using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    static class Weight //가중치를 반환
    {
        private static int[,] w{get; }=new int[4,2]{
            {1,0}, //아래
            {-1,0}, //위
            {0,1}, //오른쪽
            {0,-1} //왼쪽
        };
        private static int[] attack {get; } = new int[11]
        {
            -1,-1,1,2,4,2,4,6,0,10,1 
            //0line, 1line, 2line, 3line, 4line
            // Single, T-spin Double , Triple, Minisingle
            //Perfect Clear, Back to Back
        };
        private static int[] H { get;set;} = new int[11];
        private static int cl=0;

        public static int ClearLine()
        {
            int i,j,ret=0;
            for (i = 20; i >= 1; i--)
            {
                for(j=1;j<=10;j++) if(Map.map[i,j]!=1) break;
                if(j>10) ret++; 
            }
            return ret;
        }
        public static int AttackLine()
        {
            return attack[ClearLine()]; //나중에 T-spin 등등 추가
        }
        public static int MaxHeight()
        {
            int i,j,ret=20;
            for (i = 20; i >= 1; i--, ret--)
            {
                for(j=1;j<=10;j++) if(Map.map[i,j]>0) break;
                if(j<=10) break;
            }
            return ret;
        }
        public static int Adjacent()
        {
            int i,j,k,ret=0;
            for (i = 20; i >= 1; i--)
            {
                for (j = 1; j <= 10; j++) {
                    if (Map.map[i, j] == 2)
                    {
                        for(k=0;k<4;k++)
                            if(Map.map[i+w[k,0],j+w[k,1]]!=0) ret++;
                    }
                }
            }
            return ret;
        }
        public static int Differences()
        {
            int i,j,ret=0;
            for (i = 1; i <= 10; i++)
            {
                H[i]=20;
                for (j = 20; j>=1; j--)
                {
                    H[i]--;
                    if(Map.map[j,i]!=0) break;
                }

            }
            for (i = 1; i <= 9; i++)
            {
                ret+=(H[i]-H[i+1])* (H[i] - H[i + 1]);
            }
            return ret;
        }
        public static int fourway()
        {
            //floodfill을 이용하여 그룹의 수 리턴
            int[,] chk = new int[22,12];
            int i,j,k,ret=0;
            Point nP ,cP;
            Queue<Point> Q = new Queue<Point>();
            for (i = 1; i <= 20; i++)
            {
                for (j = 1; j <= 10; j++)
                {
                    if(Map.map[i, j] == 0 && chk[i,j]==0)
                    {
                        ret++;
                        Q.Enqueue(new Point(i , j ));
                        while (Q.Any())
                        {
                            nP = Q.Dequeue();
                            for (k = 0; k < 4; k++)
                            {
                                cP = new Point(nP.X+w[k,0],nP.Y+w[k,1]);
                                if(cP.X<=20 && Map.map[cP.X,cP.Y]==0 && chk[cP.X, cP.Y] == 0)
                                {
                                    chk[cP.X,cP.Y]=1;
                                    Q.Enqueue(cP);

                                }
                            }
                        }
                    }
                }
            }

            return ret-1;
        }
        public static int threeway()
        {
            int i,j,ret=0;
            for (j = 1; j <= 10; j++)
            {
                for (i = 19; i>=1; i--)
                {
                    if(Map.map[i+1,j]!=0 && Map.map[i, j] == 0) //블럭 및 빈칸
                    {
                        ret++;
                    }
                }
            }
            return ret;
        }
        public static int AdjacentWall()
        {
            int i, j, ret = 0;
            for(i=1;i<=20;i++) {
                if(Map.map[i,1]==2) ret++;
                if(Map.map[i,10]==2) ret++;
            }
            for(i=1;i<=10;i++) if(Map.map[1,i]==2) ret++;
            return ret;
        }
        public static int BlockEmpty() //블럭 밑 빈칸의 수
        {
            int i, j, k, ret = 0;
            int []flag = new int[11]{0,0,0,0,0,0,0,0,0,0,0};
            for (i = 20; i >= 0; i--)
            {
                for (j = 1; j <= 10; j++)
                {
                    if (Map.map[i, j] != 0)    flag[j]++;
                    else if(flag[j]>0) ret+=flag[j]; //블럭이 있고 밑에 빈 칸이 있다면 
                }
            }
            return ret;
        }
        public static int EmptyBlock() //빈칸 위 블럭의 수
        {
            int i, j, k, ret = 0;
            int[] flag = new int[11] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            for (i = 1; i >= 20; i++)
            {
                for (j = 1; j <= 10; j++)
                {
                    if (Map.map[i, j] == 0) flag[j]++;
                    else if (flag[j] > 0) ret += flag[j]; //블럭이 있고 밑에 빈 칸이 있다면 
                }
            }
            return ret;
        }

    }
}
