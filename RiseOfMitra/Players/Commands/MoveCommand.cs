using System;
using System.Collections.Generic;
using Boards;
using Units;
using Utils;
using Utils.Space;
using Utils.Types;
using Units.Pawns;

namespace RiseOfMitra.Players.Commands
{
    public class MoveCommand : ACommand
    {
        private Coord AllyPos;

        public MoveCommand() {
            CurPlayer = null;
            Boards = null;
            AllyPos = null;
            Target = null;
            ErrorMsg = "";
        }

        public override bool Execute(bool isSimualtion = false) {
            bool valid = false;
            if (Validate()) {
                APawn allyPawn = CurPlayer.GetPawnAt(AllyPos);
                Dijkstra didi = new Dijkstra(Boards.GetBoard(), AllyPos, allyPawn.MovePoints);
                List<Coord> moveRange = didi.GetValidPaths(Command.MOVE);
                if (moveRange.Contains(Target)) {
                    valid = true;
                    allyPawn.Erase(Boards);
                    allyPawn.Position = Target;
                    allyPawn.Place(Boards);
                    //allyPawn.Adapt(Boards.TerrainAt(AllyPos), Boards.TerrainAt(Target));
                } else {
                    ErrorMsg = OUT_OF_RANGE;
                }
            }
            if (!ErrorMsg.Equals("") && !isSimualtion) {
                UserUtils.PrintError(ErrorMsg);
                Console.ReadLine();
            }
                
            return valid;
        }

        public override bool Execute(List<Coord> validCells, bool isSimualtion = false) {
            bool valid = false;
            if (Validate()) {
                APawn allyPawn = CurPlayer.GetPawnAt(AllyPos);
                List<Coord> moveRange = validCells;
                if (moveRange.Contains(Target)) {
                    valid = true;
                    allyPawn.Erase(Boards);
                    allyPawn.Position = Target;
                    allyPawn.Place(Boards);
                    //allyPawn.Adapt(Boards.TerrainAt(AllyPos), Boards.TerrainAt(Target));
                } else {
                    ErrorMsg = OUT_OF_RANGE;
                }
            }
            if (!ErrorMsg.Equals("") && !isSimualtion) {
                UserUtils.PrintError(ErrorMsg);
                Console.ReadLine();
            }

            return valid;
        }

        public override double Value() {
            double total = 1;
            
            if(Coord.Distance(Target, Oponent.GetCultCenter().Position) < 
                Coord.Distance(AllyPos, Oponent.GetCultCenter().Position)) {
                total += 2 * (1 + 1.0 / Coord.Distance(AllyPos, Oponent.GetCultCenter().Position));
            }

            ETerrain terrainAtTarget = Boards.TerrainAt(Target);
            foreach (ETerrain terrain in CurPlayer.GetPawnAt(AllyPos).PositiveTerrains) {
                if(terrainAtTarget == terrain) {
                    total += 1;
                    break;
                }
            }

            foreach (ABasicPawn enemy in Oponent.GetAttackers()) {
                if(Coord.Distance(enemy.Position, CurPlayer.GetCultCenter().Position) < BoardConsts.MAX_COL / 2
                    && Coord.Distance(Target, enemy.Position) < Coord.Distance(AllyPos, enemy.Position)) {
                    total += 3.0 * (1 + 1.0 / Coord.Distance(AllyPos, enemy.Position));
                }
            }

            return total;
        }

        public override string GetShort() {
            return "MOV";
        }

        public void SetUp(Player curPlayer, Player oponent, Coord allypos, Coord target, Board boards) {
            SetCurPlayer(curPlayer);
            SetAllyPos(allypos);
            SetTarget(target);
            SetBoards(boards);
            SetOponent(oponent);
        }

        protected override bool Validate() {
            bool valid = true;
            if (!Coord.IsValid(AllyPos) || !Coord.IsValid(Target)) {
                ErrorMsg = INVALID_POS;
                valid = false;
            } else if (CurPlayer == null) {
                ErrorMsg = PLAYER;
                valid = false;
            } else if (CurPlayer.GetPawnAt(AllyPos) == null) {
                ErrorMsg = NO_PAWN;
                valid = false;
            } else if (!Boards.CellAt(Target).Equals(BoardConsts.EMPTY)) {
                ErrorMsg = OCCUPIED_CELL;
                valid = false;
            }
            return valid;
        }

        public override bool IsValid() {
            return Validate();
        }

        public override string ToString() {
            string msg = base.ToString();
            string cult = "";
            if (CurPlayer.GetCulture() == ECultures.DALRIONS)
                cult = "D";
            else
                cult = "R";
            msg += String.Format("Culture: {0}\n", cult);
            msg += String.Format("Ally: {0}\n", AllyPos);
            msg += "Move: MOV\n";
            return msg;
        }

        public override bool Equals(ACommand otherCmd) {
            if (otherCmd is MoveCommand other) {
                return (AllyPos.Equals(other.AllyPos)) && (Target.Equals(other.Target));
            } else {
                return false;
            }
        }

        private void SetAllyPos(Coord allypos) {
            if (Coord.IsValid(allypos))
                AllyPos = allypos;
        }

        private void SetTarget(Coord target) {
            if (Coord.IsValid(target))
                Target = target;
        }
    }
}
