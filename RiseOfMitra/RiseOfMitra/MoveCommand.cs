using System;
using Cells;
using Consts;
using ShortestPath;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    class MoveCommand : ACommand
    {
        private Player CurPlayer;
        private Board Boards;
        private Coord AllyPos, Target;
        private string ErrorMsg;

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
                List<Coord> moveRange = didi.GetValidPaths(Commands.MOVE);
                if (moveRange.Contains(Target)) {
                    valid = true;
                    allyPawn.Erase();
                    allyPawn.SetPos(Target);
                    allyPawn.Place();
                } else {
                    ErrorMsg = "TARGET IS NOT IN RANGE!";
                }
            }
            if (!ErrorMsg.Equals("")) Console.Write(ErrorMsg);
            return valid;
        }

        protected override bool Validate() {
            bool valid = true;
            if (AllyPos == null) {
                ErrorMsg = "INVALID ALLY POSITION!";
                valid = false;
            } else if (Target == null) {
                ErrorMsg = "INVALID TARGET!";
                valid = false;
            } else if (CurPlayer == null) {
                ErrorMsg = "CURRENT PLAYER IS NOT VALID!";
                valid = false;
            } else if (!Coord.IsValid(AllyPos) || !Coord.IsValid(Target)) {
                ErrorMsg = "ONE OF THE POSITIONS ARE INVALID!";
                valid = false;
            } else if (CurPlayer.GetPawnAt(AllyPos) == null) {
                ErrorMsg = "THE CURRENT PLAYER HAS NO PAWN AT " + AllyPos + "!";
                valid = false;
            } else if (!Boards.CellAt(Target).Equals(BoardConsts.EMPTY)) {
                ErrorMsg = "THE TARGETED CELL IS NOT EMPTY!";
                valid = false;
            }
            return valid;
        }

        public void SetCurPlayer(Player player) {
            if (player != null)
                CurPlayer = player;
            else
                Console.Write("CURRENT PLAYER IS INVALID!");
        }

        public void SetAllyPos(Coord allypos) {
            if (Coord.IsValid(allypos))
                AllyPos = allypos;
            else
                Console.Write("INVALID ALLY POSITION!");
        }

        public void SetTarget(Coord target) {
            if (Coord.IsValid(target))
                Target = target;
            else
                Console.Write("INVALID TARGET POSITION!");
        }

        public void SetBoards(Board boards) {
            if (boards != null)
                Boards = boards;
            else
                Console.Write("INVALID BOARDS!");
        }
    }
}
