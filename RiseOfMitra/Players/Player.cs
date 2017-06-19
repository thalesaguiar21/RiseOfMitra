using System;
using System.Collections.Generic;
using Boards;
using Units.Pawns;
using Players.Commands;
using Utils.Types;
using Utils.Space;
using Units.Centers;
using Units;

namespace Players
{
    public abstract class Player
    {
        protected ECultures Culture;
        protected List<APawn> Pawns;
        protected CulturalCenter Center;
        protected Coord Cursor;
        protected int Turn;

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
                        Pawns[i].Erase();
                        Pawns.RemoveAt(i);
                        found = true;
                    }
                }
            }
            return found;
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

        public abstract ACommand PrepareAction(Board boards, Player oponent);
        public abstract Player Copy(Board board);


        public ECultures GetCulture() { return Culture; }

        public List<APawn> GetPawns() {
            return Pawns;
        }

        public List<ABasicPawn> GetAttackers() {
            List<ABasicPawn> attackers = new List<ABasicPawn>();

            foreach (APawn pawn in Pawns) {
                if (pawn is ABasicPawn)
                    attackers.Add((ABasicPawn)pawn);
            }
            return attackers;
        }

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