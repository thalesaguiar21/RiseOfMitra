using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiseOfMitra
{
    class UI
    {

        private List<EMenuOptions> menuOptions = 
            new List<EMenuOptions> { EMenuOptions.PLAY, EMenuOptions.HIST, EMenuOptions.EXIT };

        private void PrintLogo() {
            List<string> logo = new List<string>() {
                                 "###                                        #         #    #                  ",
                                 "#  #                               ##      # #     # #  #####                ",
                                 "#   #                             #  #     #  #   #  #    #                  ",
                                 "#  #   #  ###    ####       ##   #         #   # #   #  # #   #      ####    ",
                                 "##     # #      #    #    #    # ###       #    #    #  # #   ###   #    #   ",
                                 "# #    #  ###   #####     #    # #         #         #  # #   #    #      #  ",
                                 "#  #   #     #  #         #    # #         #         #  # #   #    #     # # ",
                                 "#   #  #  ###    #####      ##   #         #         #  #  ## #     ####    #"};

            int lineSize = logo[0].Length;
            int padLength = (Console.WindowWidth - lineSize) / 2;


            foreach (string logoLine in logo) {
                Console.WriteLine(logoLine.PadLeft(lineSize + padLength, ' '));
            }
            Console.WriteLine("\n\n");
        }
        
        public void PrintMenu() {

            int highlightedOption = 0;

            while (true) {
                PrintLogo();
                int padLength = (Console.WindowWidth - "MENU".Length) / 2;
                Console.WriteLine("MENU".PadLeft(padLength));

                for (int i = 0; i < menuOptions.Count; i++) {
                    string opt = menuOptions[i].ToString();
                    int tmpPadLength = (Console.WindowWidth - opt.Length) / 2 - opt.Length;
                    Console.Write("".PadLeft(tmpPadLength, ' '));
                    if (i == highlightedOption) {
                        Console.BackgroundColor = ConsoleColor.Green;
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    Console.Write(opt);
                    Console.WriteLine();
                    Console.ResetColor();
                }

                var selectedOption = Console.ReadKey(false).Key;
                highlightedOption = HoverOptions(highlightedOption, selectedOption);

                if (selectedOption == ConsoleKey.Enter) {
                    if(menuOptions[highlightedOption] == EMenuOptions.PLAY) {
                        break;
                    } else {
                        ChangeView(menuOptions[highlightedOption]);
                    }
                }
                Console.Clear();
            }
            Console.Clear();
        }

        private void ChangeView(EMenuOptions opt) {

            Console.Clear();

            switch (opt) {
                case EMenuOptions.HIST:
                    HistView();
                    break;
                case EMenuOptions.EXIT:
                    ExitView();
                    break;
                default:
                    throw new InvalidOperationException("Invalid menu optin! Plase, try again!");
            }
        }

        private void HistView() {

            PrintLogo();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            List<string> history = new List<string> { "Rise of Mitra tells the history of a battle for resources between",
                "two races in the small planet Mitra. The planet is the only place in galaxy w",
                "here Argyros Crystal grows. A small piece of it has an almost infinity energy ",
                "that can be used to sustain a large city for many centuries. The planet is inh",
                "abited by two races: Rakhars and Dalrions. The Rakhars are a technological rac",
                "e, they are half organic and mechanical species that have a knowledge superior",
                "than that the Dalrions. Whereas the Dalrions are a more archaic culture, vene",
                "rating the old gods and realizing sacrifices in their names. Both of them have",
                "their interests on Argyros, and they have been in war since ancient times for",
                "the control of this resource." };
            
            foreach (var line in history) {
                int padLength = (Console.BufferWidth - line.Length) / 2;
                Console.WriteLine(line.PadLeft(line.Length + padLength, ' '));
            }
            Console.ResetColor();

            string rodape = "Press ESC to exit";
            Console.WriteLine(rodape.PadLeft(Console.BufferWidth - rodape.Length/2, ' '));

            ConsoleKey exit;
            do {
                exit = Console.ReadKey(true).Key;
            } while (ConsoleKey.Escape != exit);
        }

        private void ExitView() {

            PrintLogo();
            string exitWarn = "Press ENTER close application or ESC to cancel!";
            int padLength = (Console.WindowWidth - exitWarn.Length) / 2;
            Console.WriteLine(exitWarn.PadLeft(exitWarn.Length + padLength, ' '));

            ConsoleKey keyPressed;
            do {
                keyPressed = Console.ReadKey(true).Key;
            } while (keyPressed != ConsoleKey.Escape && keyPressed != ConsoleKey.Enter);

            if (keyPressed == ConsoleKey.Enter) {
                Environment.Exit(0);
            }
        }

        private int HoverOptions(int position, ConsoleKey keyPressed) {
            
            if(ConsoleKey.LeftArrow == keyPressed || ConsoleKey.DownArrow == keyPressed) {
                position = (position + 1) % menuOptions.Count;
            } else if (ConsoleKey.RightArrow == keyPressed || ConsoleKey.UpArrow == keyPressed) {
                position = (position - 1) % menuOptions.Count;
                if (position < 0) {
                    position = menuOptions.Count - Math.Abs(position);
                }
            }
            return position;
        }
    }
}
