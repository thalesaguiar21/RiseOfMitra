using System;
using RoMUtils;

public class RoM
{
    private int[,] TerrainBoard;
    private char[,] ControlablesBoard;
    private int boardSize;

    public RoM()
    {
        boardSize = GameConsts.BOARD_SIZE;
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
}
