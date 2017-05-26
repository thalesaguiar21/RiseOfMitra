using System.Collections.Generic;
using Cells;
using Consts;

namespace ShortestPath
{
    public class Dijkstra
    {
        private string[,] Board;
        private Coord Origin;
        private int MaxDist;
        private List<Coord> validCells;

        public Dijkstra(string[,] board, Coord origin, int maxDist)
        {
            Board = board;
            Origin = origin;
            MaxDist = maxDist;
            validCells = new List<Coord>();
            validCells.Add(Origin);
        }

        private bool IsValidNeighbor(Coord np)
        {
            bool isValid = true;
            if (!Coord.IsValid(np))
                isValid = false;
            else if (validCells.Contains(np))
                isValid = false;
            else if (Board[np.X, np.Y] != ".")
                isValid = false;
            else if (Coord.Distance(np, Origin) > MaxDist)
                isValid = false;
            return isValid;
        }

        private List<Coord> GetNeighbors(Coord cell)
        {
            List<Coord> neighbors = new List<Coord>();

            neighbors.Add(new Coord(cell.X + 1, cell.Y));
            neighbors.Add(new Coord(cell.X - 1, cell.Y));
            neighbors.Add(new Coord(cell.X, cell.Y - 1));
            neighbors.Add(new Coord(cell.X, cell.Y + 1));

            foreach (Coord c in neighbors)
            {
                if (!IsValidNeighbor(c))
                    neighbors.Remove(c);
            }

            return neighbors;
        }

        public List<Coord> GetValidPaths()
        {
            List<Coord> tmpValidCells = new List<Coord>();
            tmpValidCells.Add(Origin);
            while (tmpValidCells.Count > 0)
            {
                foreach (Coord cell in GetNeighbors(tmpValidCells[0]))
                {
                    tmpValidCells.Add(cell);
                    validCells.Add(cell);
                }
                tmpValidCells.RemoveAt(0);
            }
            return validCells;
        }
    }
}
