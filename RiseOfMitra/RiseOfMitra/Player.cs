﻿using System;
using System.Collections.Generic;
using Types;
using Cells;

namespace Game
{
    class Player
    {
        private ECultures Culture;
        private List<APawn> Pawns;
        private CulturalCenter Center;
        private Coord Cursor;
        private int Turn;

        public Player(ECultures native) {
            Culture = native;
            Pawns = new List<APawn>();
            Center = null;
            Cursor = new Coord(1, 1);
        }

        public APawn GetPawnAt(Coord pos) {
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

        public void AddPawn(APawn pawn) {
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

        public bool PeformMove(Board boards) {
            Coord selPos = boards.SelectPosition(Cursor);
            APawn pawn = GetPawnAt(selPos);
            bool valid = false;

            if (pawn != null) {
                valid = pawn.Move(Cursor);
            } else {
                Console.Write("This position is not a valid unit! ");
            }
            return valid;
        }

        public Coord PerformAttack(Board boards, List<Unit> enemies) {
            Coord selPos = boards.SelectPosition(Cursor);
            APawn pawn = GetPawnAt(selPos);
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
                APawn pawn = Center.GeneratePawn();
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
        public List<APawn> GetPawns() { return Pawns; }
        public CulturalCenter GetCenter() { return Center; }
        public Coord GetCursor() { return Cursor; }

        public void SetCulture(ECultures cult) {
            Culture = cult;
        }

        public void SetPawns(List<APawn> pawns) {
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