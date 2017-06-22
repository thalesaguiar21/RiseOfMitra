using System;
using System.Collections.Generic;
using Boards;
using Utils.Space;
using Utils.Types;
using Units.Pawns;

namespace Players.Commands
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

        public override bool Execute() {
            bool valid = false;
            if (Validate()) {
                APawn allyPawn = CurPlayer.GetPawnAt(AllyPos);
                Dijkstra didi = new Dijkstra(Boards.GetBoard(), AllyPos, allyPawn.GetMovePoints());
                List<Coord> moveRange = didi.GetValidPaths(Command.MOVE);
                if (moveRange.Contains(Target)) {
                    valid = true;
                    allyPawn.Erase();
                    allyPawn.SetPos(Target);
                    allyPawn.Place();
                } else {
                    ErrorMsg = OUT_OF_RANGE;
                }
            }
            if (!ErrorMsg.Equals("")) Console.Write(ErrorMsg);
            return valid;
        }

        public override double Value() {
            double total = 0;

            total += 3.0 / Coord.Distance(Target, Oponent.GetCenter().GetPos());
            ETerrain terrainAtTarget = (ETerrain)Boards.TerrainAt(Target);
            foreach (ETerrain terrain in CurPlayer.GetPawnAt(AllyPos).GetPositiveTerrains()) {
                if(terrainAtTarget == terrain) {
                    total += 1;
                    break;
                }
            }
            foreach (ABasicPawn pawn in Oponent.GetAttackers()) {
                if(Coord.Distance(pawn.GetPos(), CurPlayer.GetCenter().GetPos()) < 10
                    && Coord.Distance(pawn.GetPos(), AllyPos) < 10) {
                    total += 3.0 / Coord.Distance(Target, pawn.GetPos());
                }
            }

            return total;
        }

        public override string GetShort() {
            return "MOV";
        }

        public void SetUp(Player curPlayer, Coord allypos, Coord target, Board boards) {
            SetCurPlayer(curPlayer);
            SetAllyPos(allypos);
            SetTarget(target);
            SetBoards(boards);
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
