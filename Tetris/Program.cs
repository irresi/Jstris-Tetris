using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.Drawing;

namespace Tetris
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Console.CursorVisible = false;
            ConsoleKeyInfo keys;
            int flag=1;
            Color c = new Color();
            while (true)
            {
                if (flag == 1)
                {
                    Console.Clear();
                    Console.WriteLine("______________________________");
                    Console.WriteLine("  1. 오프라인 학습");
                    Console.WriteLine("  2. 학습 방법");
                    Console.WriteLine("  3. Jstris 플레이");
                    Console.WriteLine("______________________________");
                    flag=0;
                }
                keys = Console.ReadKey(true);
                if (keys.Key == ConsoleKey.D1)
                {
                    GeneticAlgorithm.Start();
                    break;
                }
                else if (keys.Key == ConsoleKey.D2)
                {
                    Instruction.explain();
                    Thread.Sleep(6000);
                    flag=1;
                }
                else if(keys.Key == ConsoleKey.D3)
                {
                    
                    Solve.start();
                    
                }
            }
            Console.ReadKey();
        }
    }
}
