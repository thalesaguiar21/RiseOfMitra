using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Types;
using Cells;
using Consts;
using ShortestPath;
using Units;

namespace Game
{
    class HumanPlayer : Player
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
            PawnFactory pFac = new PawnFactory();
            for (int i = 0; i < GetPawns().Count; i++) {
                APawn tmpPawn = GetPawns()[i].Copy();
                human.AddPawn(tmpPawn);
            }
            human.SetCulturalCenter(Center.Copy());
            human.SetCursor(tmpCursor);
            return human;
        }

        private ACommand GetCmd(Board boards, Player oponent) {
            Console.Write(String.Format("{0} TURN.\nType in a command: ", GetCulture()));
            bool validCmd = false;
            ACommand playerCommand = null;

            while (!validCmd) {
                string userCmd = Console.ReadLine().Trim().ToUpper();
                switch (userCmd) {
                    case Commands.ATTACK:
                        playerCommand = SetUpAttack(boards, oponent);
                        validCmd = true;
                        break;
                    case Commands.MOVE:
                        playerCommand = SetUpMove(boards);
                        validCmd = true;
                        break;
                    case Commands.INSPECT:
                        playerCommand = SetUpInspect(boards, oponent);
                        validCmd = true;
                        break;
                    case Commands.EXIT:
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
            ABasicPawn ally = GetPawnAt(selPos) as ABasicPawn;
            if(ally != null) {
                Dijkstra didi = new Dijkstra(boards.GetBoard(), selPos, ally.GetAtkRange());
                List<Coord> atkRange = didi.GetValidPaths(Commands.ATTACK);
                Coord enemyPos = boards.SelectPosition(Cursor, selPos, Commands.ATTACK, atkRange);

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
                List<Coord> moveRange = didi.GetValidPaths(Commands.MOVE);
                Coord target = boards.SelectPosition(Cursor, selPos, Commands.MOVE, moveRange);

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
