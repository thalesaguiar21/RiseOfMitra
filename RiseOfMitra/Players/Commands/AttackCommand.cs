﻿using System;
using System.Collections.Generic;
using System.Linq;
using Units;
using Boards;
using Utils.Space;
using Utils.Types;
using Units.Pawns;


namespace Players.Commands
{
    public class AttackCommand : ACommand
    {
        Coord AllyPos;

        public AttackCommand() {
            AllyPos = null;
            Target = null;
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
                List<Coord> atkRange = didi.GetValidPaths(Command.ATTACK);
                if (PosValidate(atkRange)) {
                    valid = true;
                    Unit enemyUnit = Oponent.GetUnitAt(Target);
                    int damage = allyPawn.GetAtk() - enemyUnit.GetDef();
                    if(damage > 0) {
                        HitMsg = String.Format("YOU HAVE DEALT {0} DAMAGE!", damage);
                        enemyUnit.SetCurrLife(enemyUnit.GetCurrLife() - damage);
                        if(enemyUnit.GetCurrLife() <= 0) {
                            Oponent.RemoveUnitAt(Target);
                            HitMsg += " ENEMY KILLED!!";
                        }
                    } else {
                        HitMsg = BLOCK;
                    }
                } 
            }
            Console.Write((valid) ? (HitMsg) : (ErrorMsg));
            return valid;
        }

        protected override bool Validate() {
            bool valid = true;
            if(!Coord.IsValid(AllyPos) || !Coord.IsValid(Target)) {
                ErrorMsg = INVALID_POS;
                valid = false;
            }  else if (Oponent == null) {
                ErrorMsg = NO_OPONENT;
                valid = false;
            } else if (CurPlayer == null) {
                ErrorMsg = PLAYER;
                valid = false;
            } else if (Boards == null) {
                ErrorMsg = NO_BOARDS;
                valid = false;
            } else {
                ABasicPawn allyPawn = CurPlayer.GetPawnAt(AllyPos) as ABasicPawn;
                if (allyPawn == null) {
                    ErrorMsg = NO_PAWN;
                    valid = false;
                } else if (Oponent.GetUnitAt(Target) == null) {
                    ErrorMsg = NO_PAWN;
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
            msg += String.Format("Ally: {0}\n", AllyPos);
            msg += "Move: ATK\n";
            return msg;
        }

        public override bool IsValid() {
            bool valid = Validate();
            if (valid) {
                ABasicPawn allyPawn = (ABasicPawn)CurPlayer.GetPawnAt(AllyPos);
                Dijkstra didi = new Dijkstra(Boards.GetBoard(), AllyPos, allyPawn.GetAtkRange());
                List<Coord> atkRange = didi.GetValidPaths(Command.ATTACK);
                valid = PosValidate(atkRange);
            }
            return valid;
        }

        private bool PosValidate(List<Coord> atkRange) {
            bool enmyInRange = false;

            foreach (Unit unit in Oponent.GetUnits()) {
                foreach (Coord cell in atkRange) {
                    if (unit.InUnit(cell)) {
                        enmyInRange = true;
                        break;
                    }
                    if (enmyInRange) break;
                }
            }

            if (!enmyInRange) { 
                ErrorMsg = NO_ENEMIES;
                return false;
            } else if (!atkRange.Contains(Target)) {
                ErrorMsg = OUT_OF_RANGE;
                return false;
            }
            return true;
        }

        public void SetUp(Coord allyPos, Coord enemyPos, Player player, Player oponent, Board boards) {
            SetAllyPos(allyPos);
            SetEnemyPos(enemyPos);
            SetCurPlayer(player);
            SetOponent(oponent);
            SetBoards(boards);
        }
        
        private void SetAllyPos(Coord allyPos) {
            if (allyPos != null)
                AllyPos = allyPos;
        }

        private void SetEnemyPos(Coord enemyPos) {
            if (enemyPos != null)
                Target = enemyPos;
        }
    }
}
