using System;
using System.Collections.Generic;
using Types;
using Cells;

namespace RiseOfMitra
{
    class Player
    {
        private ECultures Culture;
        private List<ABasicPawn> Pawns;
        private CulturalCenter Center;
        private Coord Cursor;

        public Player() {
            Culture = ECultures.DEFAULT;
            Pawns = new List<ABasicPawn>();
            Center = null;
            Cursor = new Coord(1, 1);
        }

        public ABasicPawn GetPawnAt(Coord pos) {
            for (int i = 0; i < Pawns.Count; i++) {
                if (Pawns[i].GetPos().Equals(pos))
                    return Pawns[i];
            }
            return null;
        }

        public ECultures GetCulture() { return Culture; }
        public List<ABasicPawn> GetPawns() { return Pawns; }
        public CulturalCenter GetCenter() { return Center; }
        public Coord GetCursor() { return Cursor; }

        public List<Unit> GetUnits() {
            List<Unit> playerUnits = new List<Unit>();
            playerUnits.AddRange(Pawns);
            playerUnits.Add(Center);
            return playerUnits;
        }

        public Unit GetUnitAt(Coord pos) {
            Unit unit = null;
            if (pos != null) {
                List<Unit> units = GetUnits();
                for (int i = 0; i < units.Count; i++) {
                    if (units[i].InUnit(pos)) {
                        unit = units[i];
                        break;
                    }

                }
            }
            return unit;
        }

        public void SetCulture(ECultures cult) {
            Culture = cult;
        }

        public void SetPawns(List<ABasicPawn> pawns) {
            if (pawns != null)
                Pawns = pawns;
        }

        public void SetCulturalCenter(CulturalCenter center) {
            if (center != null)
                Center = center;
        }

        public void SetCursor(Coord nCursor) {
            if (Coord.IsValid(nCursor))
                Cursor = nCursor;
        }

        public void AddPawn(ABasicPawn pawn) {
            if (pawn != null && pawn.NativeOf() == Culture) {
                Pawns.Add(pawn);
            }
        }

        public bool RemoveUnitAt(Coord pos) {
            bool found = false;

            if (Center.InUnit(pos)) {
                Center = null;
                found = true;
            } else {
                for (int i = 0; i < Pawns.Count; i++) {
                    if (Pawns[i].InUnit(pos)) {
                        Pawns.RemoveAt(i);
                        found = true;
                    }
                }
            }
            return found;
        }

        public void PeformMove(string[,] board) {
            Coord selPos = RoMBoard.SelectPosition(board, Cursor);
            ABasicPawn pawn = GetPawnAt(selPos);

            if (pawn != null) {
                pawn.Attack(Cursor);
            } else {
                Console.WriteLine("This position is not a valid unit!");
            }
        }
    }
}
