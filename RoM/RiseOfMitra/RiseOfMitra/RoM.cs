using System;

public class RoM
{
    const int BOARD_SIZE = 35;

    private int[,] TerrainBoard;
    private char[,] ControlablesBoard;

    public RoM()
    {
        TerrainBoard = new int[BOARD_SIZE, BOARD_SIZE];
        ControlablesBoard = new char[BOARD_SIZE, BOARD_SIZE];

        for (int i = 0; i < BOARD_SIZE; i++)
        {
            for (int j = 0; j < BOARD_SIZE; j++)
            {
                TerrainBoard[i, j] = 0;
                ControlablesBoard[i, j] = 'a';
            }
        }
    }

    private void PrintTerrain<T>(T[,] terrain)
    {
        for (int i = 0; i < BOARD_SIZE; i++)
        {
            for (int j = 0; j < BOARD_SIZE; j++)
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
}
