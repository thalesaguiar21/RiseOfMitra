using System;
using System.Collections.Generic;

public class RiseOfMitra
{
	// Parameters
	private static int BOARD_SIZE = 35;
	private int CONTROLABLE_BOARD = 1;
	private int VISIBILITY_BOARD = 2;
	private int TERRAIN_BOARD = 3;

	// Boards
	private List<int> Units;
	private int[,] MainBoard;
	private int[,] VisibilityBoard;
	private int[,] TerrainBoard;

	public RiseOfMitra()
	{
		Units = new List<int>();
		MainBoard = new int[BOARD_SIZE, BOARD_SIZE];
		VisibilityBoard = new int[BOARD_SIZE, BOARD_SIZE];
		TerrainBoard = new int[BOARD_SIZE, BOARD_SIZE];

		for (int i = 0; i < BOARD_SIZE; i++)
		{
			Units.Add(i);
			for (int j = 0; j < BAORD_SIZE; j++)
			{
				VisibilityBoard[i, j] = 0;
				TerrainBoard[i, j] = 0;
			}
		}
	}

	public void PrintUnits()
	{
		foreach (var item in Units)
		{
			Console.WriteLine(Units[i]);
		}

	}

	public void PrintVisibilityBoard()
	{
		for (int i = 0; i < BOARD_SIZE; i++)
		{
			for (int j = 0; j < BOARD_SIZE; j++)
			{
				Console.Write(VisibilityBoard[i, j] + " ");
			}
			Console.WriteLine();
		}

	}

	public void PrintTerrainBoard()
	{
		for (int i = 0; i < BOARD_SIZE; i++)
		{
			for (int j = 0; j < BOARD_SIZE; j++)
			{
				Console.Write(TerrainBoard[i, j] + " ");
			}
			Console.WriteLine();
		}
	}
}
