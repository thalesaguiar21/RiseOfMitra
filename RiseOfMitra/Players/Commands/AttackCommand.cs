using System;
using System.Collections.Generic;

using Units;
using Units.Pawns;
using Units.Centers;

using Boards;

using Utils;
using Utils.Space;
using Utils.Types;


namespace RiseOfMitra.Players.Commands
{
    public class AttackCommand : ACommand
    {

        public AttackCommand(Coord origin, Coord target, Player curr, Player oponent) {

            this.curPlayer = Validate<Player>.IsNotNull("Current Player cannot be null!", curr);
            this.oponent = Validate<Player>.IsNotNull("Oponent Player cannot be null!", oponent);
            this.origin = Validate<Coord>.IsNotNull("Origin cell cannot be null!", origin);
            this.target = Validate<Coord>.IsNotNull("Target cell cannot be null!", target);
            ErrorMsg = "";
        }

        public override bool Execute(Board board, bool isSimulation = false) {

            Validate<Board>.IsNotNull(Validate<Board>.BOARD_NULL, board);
            bool valid = IsValid(board);
            string hitMsg = "";
            if (valid) {
                var allyPawn = CurPlayer.GetBasicPawnAt(Origin);
                var enemyUnit = Oponent.GetUnitAt(Target);
                int damage = allyPawn.Atk - enemyUnit.Def;
                if(damage > 0) {
                    hitMsg = string.Format("{0} HAVE DEALT {1} DAMAGE!", allyPawn.Culture, damage);
                    enemyUnit.CurrLife = enemyUnit.CurrLife - damage;
                    if(enemyUnit.CurrLife <= 0) {
                        Oponent.RemoveUnitAt(target, board);
                        hitMsg += " ENEMY KILLED!!";
                    }
                } else {
                    hitMsg = BLOCK;
                }
            }else if (!isSimulation) {
                if (valid)
                    UserUtils.PrintSucess(hitMsg);
                else
                    UserUtils.PrintSucess(ErrorMsg);
                Console.ReadLine();
            }
            return valid;
        }

        public override bool IsValid(Board board) {

            Validate<Board>.IsNotNull(Validate<Board>.BOARD_NULL, board);
            bool valid = true;
            if (!Coord.IsValid(Origin) || !Coord.IsValid(Target)) {
                ErrorMsg = INVALID_POS;
                valid = false;
            } else if (Oponent.GetUnitAt(Target) == null) {
                ErrorMsg = NO_ENEMIES;
                valid = false;
            } else if (CurPlayer.GetBasicPawnAt(Origin) == null) {
                ErrorMsg = NO_PAWN;
                valid = false;
            } else {
                var dijkstra = new Dijkstra(board.GetBoard(), Origin, CurPlayer.GetBasicPawnAt(Origin).AtkRange);
                var atkRange = dijkstra.GetValidPaths(Command.ATTACK);
                if (!atkRange.Contains(Target)) {
                    ErrorMsg = OUT_OF_RANGE;
                    valid = false;
                }                
            }
            return valid;
        }

        public override string ToString() {
            string msg = base.ToString();
            string cult = "";
            if (CurPlayer.GetCulture() == ECultures.DALRIONS)
                cult = "D";
            else
                cult = "R";
            msg += String.Format("Culture: {0}\n", cult);
            msg += String.Format("Ally: {0}\n", Origin);
            msg += "Move: ATK\n";
            return msg;
        }

        public override string GetShort() {
            return "ATK";
        }

        public override bool Equals(ACommand otherCmd) {

            if (otherCmd is AttackCommand other) {
                bool isEqual = true;
                isEqual &= (Origin.Equals(other.Origin)) && (Target.Equals(other.Target));
                isEqual &= (CurPlayer.GetUnitAt(Origin).CurrLife) == (other.CurPlayer.GetUnitAt(other.Origin).CurrLife);
                isEqual &= (Oponent.GetUnitAt(Target).CurrLife) == (other.Oponent.GetUnitAt(other.Target).CurrLife);
                return isEqual;
            } else {
                return false;
            }
        }
    }
}
