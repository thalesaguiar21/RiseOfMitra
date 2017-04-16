using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiseOfMitra
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the world of Mitra!");
            RoM rom = new RoM();
            Console.Write("\n");
            rom.PrintTerrainBoard();
            rom.PrintControlablesBoard();


            Console.ReadLine();
        }
    }
}
