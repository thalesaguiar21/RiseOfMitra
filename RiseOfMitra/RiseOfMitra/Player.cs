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
        private int Turn;

        public Player(ECultures native) {
            Culture = native;
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

        public bool PeformMove(string[,] board) {
            Coord selPos = RoMBoard.SelectPosition(board, Cursor);
            ABasicPawn pawn = GetPawnAt(selPos);
            bool valid = false;

            if (pawn != null) {
                valid = pawn.Move(Cursor);
            } else {
                Console.Write("This position is not a valid unit! ");
            }
            return valid;
        }

        public Coord PerformAttack(string[,] board, List<Unit> enemies) {
            Coord selPos = RoMBoard.SelectPosition(board, Cursor);
            ABasicPawn pawn = GetPawnAt(selPos);
            Coord target = null;

            if (pawn != null) {
                target = pawn.Attack(Cursor, enemies);
            } else {
                Console.Write("This position is not a valid unit! ");
            }

            return target;
        }

        public void ExecuteTurnEvents(string[,] board) {
            Center.Regen();
            if (Turn % Center.GetUnitsPerTurn() == 0) {
                ABasicPawn pawn = Center.GeneratePawn();
                if(pawn != null) {
                    pawn.Place();
                    AddPawn(pawn);
                } else {
                    Console.Write("Can not create more pawns!");
                    Console.ReadLine();
                }
            }
        }

        public ECultures GetCulture() { return Culture; }
        public List<ABasicPawn> GetPawns() { return Pawns; }
        public CulturalCenter GetCenter() { return Center; }
        public Coord GetCursor() { return Cursor; }

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

        public void SetTurn() {
            Turn += 1;
        }
    }
}