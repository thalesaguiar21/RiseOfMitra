using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Boards;
using Units;
using Players.Commands;
using Utils.Types;
using Utils.Space;
using Units.Pawns;

namespace Players
{
    public class HumanPlayer : Player
    {
        public HumanPlayer(ECultures native) {
            Culture = native;
            Pawns = new List<APawn>();
            Center = null;
            Cursor = new Coord(1, 1);
        }

        public override ACommand PrepareAction(Board boards, Player oponent) {
            ACommand partialCommand = GetCmd(boards, oponent);
            return partialCommand;
        }

        public override Player Copy(Board board) {
            Player human = new HumanPlayer(GetCulture());
            Coord tmpCursor = new Coord(GetCursor().X, GetCursor().Y);
            for (int i = 0; i < GetPawns().Count; i++) {
                APawn tmpPawn = GetPawns()[i].Copy(board);
                human.AddPawn(tmpPawn);
            }
            human.SetCulturalCenter(Center.Copy(board));
            human.SetCursor(tmpCursor);
            return human;
        }

        private ACommand GetCmd(Board boards, Player oponent) {
            Console.Write(String.Format("YOUR TURN.\nType in a command: ", GetCulture()));
            bool validCmd = false;
            ACommand playerCommand = null;

            while (!validCmd) {
                string userCmd = Console.ReadLine().Trim().ToUpper();
                switch (userCmd) {
                    case Command.ATTACK:
                        playerCommand = SetUpAttack(boards, oponent);
                        validCmd = true;
                        break;
                    case Command.MOVE:
                        playerCommand = SetUpMove(boards);
                        validCmd = true;
                        break;
                    case Command.INSPECT:
                        playerCommand = SetUpInspect(boards, oponent);
                        validCmd = true;
                        break;
                    case Command.EXIT:
                        break;
                    default:
                        Console.Write(userCmd + " IS NOT A VALID COMMAND! Please, try again...");
                        Console.ReadLine();
                        break;
                }

            }
            return playerCommand;
        }

        private AttackCommand SetUpAttack(Board boards, Player oponent) {
            AttackCommand attackCmd = new AttackCommand();
            Coord selPos = boards.SelectPosition(Cursor);
            if (GetPawnAt(selPos) is ABasicPawn ally) {
                Dijkstra didi = new Dijkstra(boards.GetBoard(), selPos, ally.GetAtkRange());
                List<Coord> atkRange = didi.GetValidPaths(Command.ATTACK);
                Coord enemyPos = boards.SelectPosition(Cursor, selPos, Command.ATTACK, atkRange);

                // Set up command variables
                attackCmd.SetUp(selPos, enemyPos, this, oponent, boards);
            }
            return attackCmd;
        }

        private MoveCommand SetUpMove(Board boards) {
            MoveCommand move = new MoveCommand();
            Coord selPos = boards.SelectPosition(Cursor);
            APawn ally = GetPawnAt(selPos);
            if(ally != null) {
                Dijkstra didi = new Dijkstra(boards.GetBoard(), selPos, ally.GetMovePoints());
                List<Coord> moveRange = didi.GetValidPaths(Command.MOVE);
                Coord target = boards.SelectPosition(Cursor, selPos, Command.MOVE, moveRange);

                // Set up command variables
                move.SetUp(this, selPos, target, boards);
            }
            return move;
        }

        private InspectCommand SetUpInspect(Board boards, Player oponent) {
            InspectCommand inspect = new InspectCommand();
            Coord selPos = boards.SelectPosition(Cursor);
            List<Unit> allUnits = new List<Unit>();
            allUnits.AddRange(GetUnits());
            allUnits.AddRange(oponent.GetUnits());

            inspect.SetUp(selPos, boards, allUnits);
            return inspect;
        }
    }
}
