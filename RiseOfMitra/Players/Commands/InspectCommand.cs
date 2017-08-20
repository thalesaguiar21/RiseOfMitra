using System;
using System.Collections.Generic;
using Units;
using Boards;
using Utils;
using Utils.Space;

namespace RiseOfMitra.Players.Commands
{
    public class InspectCommand : ACommand
    {
        List<Unit> Units;

        public InspectCommand(Coord target)
        {
            ErrorMsg = "";
            Units = new List<Unit>();
            this.target = Validate<Coord>.IsNotNull("Target can not be null!", target);
        }

        public override bool Execute(Board board, bool isSimulation = false)
        {

            Validate<Board>.IsNotNull(Validate<Board>.BOARD_NULL, board);
            bool valid = IsValid(board);
            if (valid) {
                foreach (Unit unit in Units) {
                    if (unit.InUnit(Target)) {
                        board.Status = unit.GetStatus().Split('\n');
                        break;
                    }
                }
            } else {
                UserUtils.PrintError(ErrorMsg);
            }
            return false;
        }

        public override string GetShort()
        {
            return "INSP";
        }

        public override bool IsValid(Board board)
        {
            Validate<Board>.IsNotNull(Validate<Board>.BOARD_NULL, board);
            bool valid = true;
            if (!Coord.IsValid(Target)) {
                ErrorMsg = INVALID_POS;
                valid = false;
            } else {
                valid = false;
                foreach (Unit unit in Units) {
                    if (unit.InUnit(Target)) {
                        valid = true;
                        break;
                    }
                }
                if (!valid) ErrorMsg = INVALID_POS;
            }
            return valid;
        }

        public override bool Equals(ACommand otherCmd)
        {
            return otherCmd is InspectCommand;
        }
    }
}
