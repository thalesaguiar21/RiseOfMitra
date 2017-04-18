using RiseOfMitra;
using RoMUtils;
using System;
using System.Collections.Generic;

public class RoM
{
    private char[,] MainBoard;
    private Dictionary<Pair, AUnit> AvaiableUnits;
    private int boardSize;
    private int nextPlayer;
    private bool Play;

    public RoM()
    {
        boardSize = GameConsts.BOARD_SIZE;
        nextPlayer = 0;
        MainBoard = new char[boardSize, boardSize];
        Play = true;

        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                MainBoard[i, j] = 'a';
            }
        }
    }

    public void Start()
    {
        Console.WriteLine("Starting Rise of Mitra...");
        do
        {
            Console.WriteLine("Player " + (nextPlayer + 1) + " turn..\n");
            PrintMainBoard();
            OptionsMenu();
            Console.Write("Press enter to finish...");
            SetNextPlayer();
            Console.ReadLine();
            Console.Clear();
        } while (Play);
    }

    private void OptionsMenu()
    {
        Console.WriteLine("Avaiable options: ");
        Console.Write("1) Move pawn\t2) Attack enemy pawn\t3) Exit game\n");
        int userOp = 0;
        try
        {
            userOp = int.Parse(Console.ReadLine());
            switch (userOp)
            {
                case 1:
                    Console.WriteLine("Moving pawn at () to ()");
                    break;
                case 2:
                    Console.WriteLine("Attacking enemy pawn at ()");
                    break;
                case 3:
                    Play = false;
                    break;
                default:
                    throw new FormatException();
            }
        }
        catch (FormatException)
        {
            Console.WriteLine(userOp + " isn't a valid option!");
        }
    }

    private void PrintTerrain<T>(T[,] terrain)
    {
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                Console.Write(terrain[i, j] + " ");
            }
            Console.Write("\n");
        }
        Console.Write("\n\n");
    }

    public void PrintMainBoard()
    {
        PrintTerrain<char>(MainBoard);
    }

    public void SetControlablesboard(int[,] terrain)
    {
        throw new NotImplementedException();
    }

    public void SetNextPlayer()
    {
        nextPlayer = ++nextPlayer % 2;
    }
}
