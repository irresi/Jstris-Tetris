using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace Tetris
{
    static class Instruction
    {
        public static void explain()
        {
            Console.Clear();
            Console.WriteLine("_____________________________");
            Console.WriteLine("유전 알고리즘을 이용한\n테트리스 학습");
            Console.WriteLine("_____________________________");
            Console.WriteLine("[기능]");
            Console.WriteLine("오프라인 학습하기");
            Console.WriteLine("jstris에서 플레이하기");
            Console.WriteLine("_____________________________");
            Console.WriteLine("[학습 종류]");
            Console.WriteLine("1P 플레이 : 혼자 플레이");
            Console.WriteLine("1 vs 少 : 적은 사람과 함께 플레이");
            Console.WriteLine("1 vs 多 : 5명 이상의 많은 사람과 플레이" );
            Console.WriteLine("_____________________________");
            Console.WriteLine("크롬에서 북마크바 없이 실행");

        }
    }
}
