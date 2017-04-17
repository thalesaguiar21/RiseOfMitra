using System;
using RoMUtils;

public class RoM
{
    private int[,] TerrainBoard;
    private char[,] ControlablesBoard;
    private int boardSize;
    private int nextPlayer;

    public RoM()
    {
        boardSize = GameConsts.BOARD_SIZE;
        nextPlayer = 0;
        TerrainBoard = new int[boardSize, boardSize];
        ControlablesBoard = new char[boardSize, boardSize];

        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                TerrainBoard[i, j] = 0;
                ControlablesBoard[i, j] = 'a';
            }
        }
    }

    public void Start()
    {
        bool play = true;

        while (play)
        {
            
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
        PrintTerrain<char>(ControlablesBoard);
    }

    public void PrintBoth()
    {
        PrintSideBySide<int, char>(TerrainBoard, ControlablesBoard);
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
        bool validControlables = true;

        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                if (GameConsts.validDalrionsUnits) validControlables = false;
            }
        }

        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                ControlablesBoard[i, j] = terrain[i, j];
            }
        }
    }

    public void SetNextPlayer()
    {
        nextPlayer = ++nextPlayer % 2;
    }
}
