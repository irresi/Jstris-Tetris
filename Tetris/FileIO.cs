using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    static class FileIO
    {
        public static StreamWriter outputFile;

        public static void FWrite(int num)
        {
            int i;
            if (num == 1) { 
                File.WriteAllText(@"1P.txt", String.Empty);
                outputFile = new StreamWriter(@"1P.txt");
            }
            else if(num==2) {
                File.WriteAllText(@"2P.txt", String.Empty);
                outputFile = new StreamWriter(@"2P.txt");
            }
            else if(num==3) {
                File.WriteAllText(@"3P.txt", String.Empty);
                outputFile = new StreamWriter(@"3P.txt");
            }
            else throw new NotImplementedException(); 
            for (i = 0; i < 30; i++)
            {
                foreach(double t in GeneticAlgorithm.nl[i])
                {
                    outputFile.Write(t+" ");
                }
                outputFile.WriteLine();
            }
            outputFile.Close();
        }
    }
}
 