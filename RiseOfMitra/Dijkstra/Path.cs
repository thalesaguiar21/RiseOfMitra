using System.Collections.Generic;
using Cells;
using Consts;

namespace ShortestPath
{
    public class BFS
    {
        private string[,] Board;
        private Coord Origin;
        private int MaxDist;
        private List<Coord> validCells;

        public BFS(string[,] board, Coord origin, int maxDist)
        {
            Board = board;
            Origin = origin;
            MaxDist = maxDist;
            validCells = new List<Coord>();
            validCells.Add(Origin);
        }

        private bool IsValidMoveNeighbor(Coord np, string cmd)
        {
            bool isValid = true;
            if (!Coord.IsValid(np))
                isValid = false;
            else if (validCells.Contains(np))
                isValid = false;
            else if (cmd == Commands.MOVE && Board[np.X, np.Y] != BoardConsts.EMPTY)
                isValid = false;
            else if (Coord.Distance(np, Origin) > MaxDist)
                isValid = false;
            return isValid;
        }

        private List<Coord> GetNeighbors(Coord cell, string cmd)
        {
            List<Coord> neighbors = new List<Coord>();
            List<Coord> tmpNeighbors = new List<Coord>();

            tmpNeighbors.Add(new Coord(cell.X + 1, cell.Y));
            tmpNeighbors.Add(new Coord(cell.X - 1, cell.Y));
            tmpNeighbors.Add(new Coord(cell.X, cell.Y - 1));
            tmpNeighbors.Add(new Coord(cell.X, cell.Y + 1));

            for (int i = 0; i < tmpNeighbors.Count; i++)
            {
                if (IsValidMoveNeighbor(tmpNeighbors[i], cmd))
                    neighbors.Add(tmpNeighbors[i]);
            }

            return neighbors;
        }

        public List<Coord> GetValidPaths(string cmd)
        {
            List<Coord> tmpValidCells = new List<Coord>();
            tmpValidCells.Add(Origin);
            while (tmpValidCells.Count > 0)
            {
                List<Coord> validNeighbors = GetNeighbors(tmpValidCells[0], cmd);
                for (int i = 0; i < validNeighbors.Count; i++)
                {
                    tmpValidCells.Add(validNeighbors[i]);
                    validCells.Add(validNeighbors[i]);
                }
                tmpValidCells.RemoveAt(0);
            }
            validCells.RemoveAt(0);
            return validCells;
        }
    }
}
