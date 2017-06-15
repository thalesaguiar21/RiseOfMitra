using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cells;
using Units;
using Boards;

namespace Commands
{
    public class InspectCommand : ACommand
    {
        List<Unit> Units;

        public InspectCommand() {
            ErrorMsg = "";
            Units = new List<Unit>();
            Target = null;
            Boards = null;
        }

        public override bool Execute() {
            bool valid = Validate();
            if (valid) {
                foreach (Unit unit in Units) {
                    if (unit.InUnit(Target)) {
                        Console.Write(unit.GetStatus());
                        break;
                    }
                }
            } else {
                Console.Write(ErrorMsg);
            }            
            return false;
        }

        public void SetUp(Coord target, Board boards, List<Unit> units) {
            SetTarget(target);
            SetBoards(boards);
            SetUnits(units);
        }

        protected override bool Validate() {
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

        public override bool IsValid() {
            return Validate();
        }

        private void SetUnits(List<Unit> units) {
            if (units != null)
                Units = units;
        }

        private void SetTarget(Coord target) {
            if (Coord.IsValid(target))
                Target = target;
        }

        private void SetBoards(Board boards) {
            if (boards != null)
                Boards = boards;
        }
    }
}
