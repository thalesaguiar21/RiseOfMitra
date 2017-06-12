using System;
using Cells;
using ShortestPath;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    class AttackCommand : ACommand
    {
        string ErrorMsg, HitMsg;
        Player CurPlayer, Oponent;
        Coord AllyPos, EnemyPos;
        Board Boards;

        public AttackCommand() {
            AllyPos = null;
            EnemyPos = null;
            CurPlayer = null;
            Oponent = null;
            Boards = null;
            ErrorMsg = "";
            HitMsg = "";
        }

        public override bool Execute() {
            bool valid = false;
            if (Validate()) {
                ABasicPawn allyPawn = (ABasicPawn) CurPlayer.GetPawnAt(AllyPos);
                Dijkstra didi = new Dijkstra(Boards.GetBoard(), AllyPos, allyPawn.GetAtkRange());
                List<Coord> atkRange = didi.GetValidPaths(Consts.Commands.ATTACK);
                if (PosValidate(atkRange)) {
                    valid = true;
                    Unit enemyUnit = Oponent.GetUnitAt(EnemyPos);
                    int damage = allyPawn.GetAtk() - enemyUnit.GetDef();
                    if(damage > 0) {
                        HitMsg = String.Format("YOU HAVE DEALT {0} DAMAGE!", damage);
                        enemyUnit.SetCurrLife(enemyUnit.GetCurrLife() - damage);
                        if(enemyUnit.GetCurrLife() <= 0) {
                            Oponent.RemoveUnitAt(EnemyPos);
                            HitMsg += " ENEMY KILLED!!";
                        }
                    } else {
                        HitMsg = "ENEMY HAS BLOCKED THE ATTACK!";
                    }
                } 
            }
            Console.Write((valid) ? (HitMsg) : (ErrorMsg));
            return valid;
        }

        protected override bool Validate() {
            bool valid = true;
            if(AllyPos == null) {
                ErrorMsg = "INVALID ALLY POSITION!";
                valid = false;
            } else if (EnemyPos == null) {
                ErrorMsg = "INVALID ENEMY POSITION!";
                valid = false;
            } else if (Oponent == null) {
                ErrorMsg = "INVALID OPONENT!";
                valid = false;
            } else if (CurPlayer == null) {
                ErrorMsg = "CURRENT PLAYER IS NOT VALID!";
                valid = false;
            } else if (Boards == null) {
                ErrorMsg = "INVALID BOARDS!";
                valid = false;
            } else {
                ABasicPawn allyPawn = CurPlayer.GetPawnAt(AllyPos) as ABasicPawn;
                if (allyPawn == null) {
                    ErrorMsg = "CURRENT PLAYER DO NOT HAVE A VALID PAWN AT THE GIVEN POSITION!";
                    valid = false;
                } else if (Oponent.GetUnitAt(EnemyPos) == null) {
                    ErrorMsg = "OPONENT DO NOT HAVE A UNIT" +
                        " AT THIS POSITION!";
                    valid = false;
                }
            }            
            return valid;
        }

        private bool PosValidate(List<Coord> atkRange) {
            if(atkRange.Count == 0) {
                ErrorMsg = "THIS PAWN CAN NOT ATTACK";
                return false;
            } else if (!atkRange.Contains(EnemyPos)) {
                ErrorMsg = "ALLY PAWN CAN NOT REACH SELECTED ENEMY POSITION";
                return false;
            }
            return true;
        }
        
        public void SetAllyPos(Coord allyPos) {
            if (allyPos != null)
                AllyPos = allyPos;
            else
                Console.Write("INVALID ALLY POSITION!");
        }

        public void SetEnemyPos(Coord enemyPos) {
            if (enemyPos != null)
                EnemyPos = enemyPos;
            else
                Console.Write("INVALID ENEMY POSITION!");
        }

        public void SetCurPlayer(Player player) {
            if (player != null)
                CurPlayer = player;
            else
                Console.Write("CURRENT PLAYER IS INVALID!");
        }

        public void SetOponent(Player oponent) {
            if (oponent != null)
                Oponent = oponent;
            else
                Console.Write("CURRENT OPONENT IS INVALID!");
        }

        public void SetBoards(Board boards) {
            if (boards != null)
                Boards = boards;
            else
                Console.Write("INVALID BOARDS!");
        }
    }
}
