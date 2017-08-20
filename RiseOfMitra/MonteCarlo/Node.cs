using System;
using System.Collections.Generic;

using Boards;

using Utils;
using Utils.Space;
using Utils.Types;

using Units.Pawns;
using Units.Centers;

using RiseOfMitra.Players.Commands;

namespace RiseOfMitra.MonteCarlo
{
    /// <summary>
    /// This class represents a Game Tree node, which is reachable by Given a a State(Board) applying
    /// the given Command.
    /// </summary>
    public class Node
    {
        public int VisitCount;
        public int WinRate;
        private double value;
        public Board Boards;
        public ACommand Command;
        public List<Node> Childs;

        public double Value {
            get { return this.value; }
            set { this.value = value; }
        }

        public Node(Board boards, ACommand cmd) {

            Boards = Validate<Board>.IsNotNull(Validate<Board>.BOARD_NULL, boards);
            Command = Validate<ACommand>.IsNotNull(Validate<ACommand>.COMMAND_NULL, cmd);
            Childs = new List<Node>();
            value = 0.0;
            VisitCount = 0;
        }

        public double GetValue() {

            if (Command is MoveCommand move) {
                return ComputeMoveValue(move);
            } else if (Command is AttackCommand attack) {
                return ComputeAttackValue(attack);
            } else {
                throw new ArgumentException("Can not compute value for command of type " + Command);
            }
        }

        private double ComputeMoveValue(MoveCommand move) {

            double total = 1;
            if (Coord.Distance(move.Target, move.Oponent.GetCultCenter().Position) <
                Coord.Distance(move.Origin, move.Oponent.GetCultCenter().Position)) {
                total += 2 * (1 + 1.0 / Coord.Distance(move.Origin, move.Oponent.GetCultCenter().Position));
            }

            ETerrain terrainAtTarget = Boards.TerrainAt(move.Target);
            foreach (ETerrain terrain in move.CurPlayer.GetPawnAt(move.Origin).PositiveTerrains) {
                if (terrainAtTarget == terrain) {
                    total += 1;
                    break;
                }
            }

            foreach (ABasicPawn enemy in move.Oponent.GetAttackers()) {
                if (Coord.Distance(enemy.Position, move.CurPlayer.GetCultCenter().Position) < BoardConsts.MAX_COL / 2
                    && Coord.Distance(move.Target, enemy.Position) < Coord.Distance(move.Origin, enemy.Position)) {
                    total += 3.0 * (1 + 1.0 / Coord.Distance(move.Origin, enemy.Position));
                }
            }

            return total;
        }

        private double ComputeAttackValue(AttackCommand attack) {

            double total = 10;
            if (attack.Oponent.GetUnitAt(attack.Target) is CulturalCenter) {
                total += 1.0;
            }

            double remainingHealth = attack.Oponent.GetUnitAt(attack.Target).CurrLife / attack.Oponent.GetUnitAt(attack.Target).TotalLife;
            if (remainingHealth < 0.5)
                total += 3.0;

            if (Coord.Distance(attack.Target, attack.CurPlayer.GetCultCenter().Position) < BoardConsts.MAX_COL / 2) {
                total += 1 + 100 / Coord.Distance(attack.Target, attack.CurPlayer.GetCultCenter().Position);
            }

            return total;
        }

        public override bool Equals(object obj) {
            if (obj is Node other) {
                return Boards.Equals(other.Boards) && Command.Equals(other.Command);
            } else {
                return false;
            }
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }

        public static List<Node> FromRange(Board board, List<ACommand> commands) {

            var nodes = new List<Node>();
            foreach (ACommand cmd in commands) {
                nodes.Add(new Node(board, cmd));
            }

            return nodes;
        }

        public static bool ValidateNode(Node node) {
            bool valid = true;
            if (node == null) {
                valid = false;
            } else if (node.VisitCount < 0) {
                valid = false;
            } else if (node.Boards == null) {
                valid = false;
            } else if (node.Command == null) {
                valid = false;
            }
            return valid;
        }
    }
}
