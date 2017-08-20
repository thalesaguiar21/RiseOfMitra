using System;
using System.Collections.Generic;

using Boards;

using Units;

using Utils;
using Utils.Space;
using Utils.Types;


namespace RiseOfMitra.Players.Commands
{
    public class MoveCommand : ACommand
    {
        public MoveCommand(Coord origin, Coord target, Player curr, Player oponent)
        {
            this.curPlayer = Validate<Player>.IsNotNull("Current Player cannot be null!", curr);
            this.oponent = Validate<Player>.IsNotNull("Current oponent can not be null!", oponent);
            this.origin = Validate<Coord>.IsNotNull("Origin cell cannot be null!", origin);
            this.target = Validate<Coord>.IsNotNull("Target cell cannot be null!", target);
            ErrorMsg = "";
        }

        public override bool Execute(Board board, bool isSimualtion = false)
        {
            Validate<Board>.IsNotNull(Validate<Board>.BOARD_NULL, board);
            bool valid = IsValid(board);
            if (valid) {
                var allyPawn = CurPlayer.GetPawnAt(origin);
                allyPawn.Erase(board);
                allyPawn.Position = target;
                allyPawn.Place(board);
                allyPawn.Adapt(board.TerrainAt(origin), board.TerrainAt(target));
            } else if (!isSimualtion) {
                UserUtils.PrintError(ErrorMsg);
                Console.ReadLine();
            }

            return valid;
        }

        public override string GetShort()
        {
            return "MOV";
        }

        public override bool IsValid(Board board)
        {
            Validate<Board>.IsNotNull(Validate<Board>.BOARD_NULL, board);
            bool valid = true;
            if (!Coord.IsValid(target) || !Coord.IsValid(target)) {
                ErrorMsg = INVALID_POS;
                valid = false;
            } else if (CurPlayer.GetPawnAt(origin) == null) {
                ErrorMsg = NO_PAWN;
                valid = false;
            } else {
                var dijkstra = new Dijkstra(board.GetBoard(), origin, CurPlayer.GetPawnAt(origin).MovePoints);
                var validCells = dijkstra.GetValidPaths(Command.MOVE);
                valid = validCells.Contains(target);
                if (!valid) ErrorMsg = OUT_OF_RANGE;
            }
            return valid;
        }

        public override string ToString()
        {
            string msg = base.ToString();
            string cult = "";
            if (CurPlayer.GetCulture() == ECultures.DALRIONS)
                cult = "D";
            else
                cult = "R";
            msg += String.Format("Culture: {0}\n", cult);
            msg += String.Format("Ally: {0}\n", Origin);
            msg += "Move: MOV\n";
            return msg;
        }

        public override bool Equals(ACommand otherCmd)
        {
            if (otherCmd is MoveCommand other) {
                return (Origin.Equals(other.Origin)) && (Target.Equals(other.Target));
            } else {
                return false;
            }
        }
    }
}
