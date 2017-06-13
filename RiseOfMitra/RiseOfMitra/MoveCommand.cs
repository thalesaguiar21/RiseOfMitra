﻿using System;
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
                List<Coord> moveRange = didi.GetValidPaths(Commands.MOVE);
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

        private void SetCurPlayer(Player player) {
            if (player != null)
                CurPlayer = player;
        }

        private void SetAllyPos(Coord allypos) {
            if (Coord.IsValid(allypos))
                AllyPos = allypos;
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
