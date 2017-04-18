using System;
using System.Collections.Generic;
using RoMUtils;
using RiseOfMitra;

public class RoM
{
    private int[,] TerrainBoard;
    private char[,] MainBoard;
    private Dictionary<Pair, AUnit> AvaiableUnits;
    private int boardSize;
    private int nextPlayer;
    private bool Play;

    public RoM()
    {
        boardSize = GameConsts.BOARD_SIZE;
        nextPlayer = 0;
        TerrainBoard = new int[boardSize, boardSize];
        MainBoard = new char[boardSize, boardSize];
        Play = true;

        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                TerrainBoard[i, j] = 0;
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
            PrintBoth();
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

    private void PrintSideBySide<T, U>(T[,] table1, U[,] table2)
    {
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                Console.Write(table1[i, j] + " ");
            }

            Console.Write("\t");

            for (int j = 0; j < boardSize; j++)
            {
                Console.Write(table2[i, j] + " ");
            }

            Console.Write("\n");
        }
        Console.Write("\n\n");
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


    public void PrintTerrainBoard()
    {
        PrintTerrain<int>(TerrainBoard);
    }

    public void PrintControlablesBoard()
    {
        PrintTerrain<char>(MainBoard);
    }

    public void PrintBoth()
    {
        PrintSideBySide<int, char>(TerrainBoard, MainBoard);
    }

    public void SetTerrainBoard(int[,] terrain)
    {
        bool validTerrain = true;
        if (validTerrain)
        {
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    TerrainBoard[i, j] = terrain[i, j];
                }
            }
        }
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
