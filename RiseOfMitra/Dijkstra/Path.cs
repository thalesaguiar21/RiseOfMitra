using System.Collections.Generic;
using Cells;
using Consts;
using Types;

namespace ShortestPath
{
    public class Dijkstra
    {
        private string[,] Board;
        private Coord Origin;
        private int MaxDist;
        private List<Coord> validCells;
        private ECultures Ally;

        public Dijkstra(string[,] board, Coord origin, int maxDist)
        {
            Board = board;
            Origin = origin;
            MaxDist = maxDist;
            validCells = new List<Coord>();
            validCells.Add(Origin);
        }

        private bool IsValidMoveNeighbor(Coord np)
        {
            bool isValid = true;
            if (!Coord.IsValid(np))
                isValid = false;
            else if (validCells.Contains(np))
                isValid = false;
            else if (Board[np.X, np.Y] != BoardStrings.EMPTY)
                isValid = false;
            else if (Coord.Distance(np, Origin) > MaxDist)
                isValid = false;
            return isValid;
        }

        private bool IsValidAtkNeighbor(Coord np)
        {
            bool isValid = true;
            if (!Coord.IsValid(np))
                isValid = false;
            else if (validCells.Contains(np))
                isValid = false;
            else if (Coord.Distance(np, Origin) > MaxDist)
                isValid = false;
            else
            {
                switch (Ally)
                {
                    case ECultures.DALRIONS:
                        isValid = BoardStrings.IsRahkar(Board[np.X, np.Y]);                        
                        break;
                    case ECultures.RAHKARS:
                        isValid = BoardStrings.IsDalrion(Board[np.X, np.Y]);
                        break;
                    default:
                        isValid = false;
                        break;
                }
            }
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
                if (cmd == Commands.MOVE)
                {
                    if (IsValidMoveNeighbor(tmpNeighbors[i]))
                        neighbors.Add(tmpNeighbors[i]);
                }
                else if (cmd == Commands.ATTACK)
                {
                    if (IsValidAtkNeighbor(tmpNeighbors[i]))
                        neighbors.Add(tmpNeighbors[i]);
                }
                else
                {
                    return null;
                }
            }

            return neighbors;
        }

        public List<Coord> GetValidPaths()
        {
            List<Coord> tmpValidCells = new List<Coord>();
            tmpValidCells.Add(Origin);
            while (tmpValidCells.Count > 0)
            {
                List<Coord> validNeighbors = GetNeighbors(tmpValidCells[0], Commands.MOVE);
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

        public List<Coord> GetAtkRange(ECultures ally)
        {
            List<Coord> tmpValidCells = new List<Coord>();
            tmpValidCells.Add(Origin);
            Ally = ally;
            while (tmpValidCells.Count > 0)
            {
                List<Coord> validNeighbors = GetNeighbors(tmpValidCells[0], Commands.ATTACK);
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
