using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Boards;
using Units;
using Utils;
using Utils.Types;
using Utils.Space;
using Units.Pawns;
using RiseOfMitra.MonteCarlo;
using RiseOfMitra.Players.Commands;

namespace RiseOfMitra.Players
{
    public class HumanPlayer : Player
    {
        public HumanPlayer(ECultures native) {
            Culture = native;
            Pawns = new List<APawn>();
            CultCenter = null;
            Cursor = new Coord(1, 1);
        }

        public override Node PrepareAction(Board boards, Player oponent) {
            ACommand partialCommand = GetCmd(boards, oponent);
            Node state = new Node(0, boards, partialCommand);
            return state;
        }

        public override Player Copy(Board board) {
            Player human = new HumanPlayer(GetCulture());
            Coord tmpCursor = new Coord(GetCursor().X, GetCursor().Y);
            for (int i = 0; i < GetPawns().Count; i++) {
                APawn tmpPawn = GetPawns()[i].Copy(board);
                human.AddPawn(tmpPawn);
            }
            human.SetCultCenter(CultCenter.Copy(board));
            human.SetCursor(tmpCursor);
            return human;
        }

        private ACommand GetCmd(Board boards, Player oponent) {
            Console.Write("YOUR TURN.");
            bool validCmd = false;
            ACommand playerCommand = null;
            string userCmd = "";

            while (!validCmd) {
                Console.Write("\nType in a command: ");
                userCmd = Console.ReadLine().Trim().ToUpper();
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
                    case Command.HELP:
                        SetUpHelp();
                        break;
                    default:
                        if (userCmd == "\n" || userCmd == "")
                            UserUtils.PrintError("Please, type in a command!");
                        else
                            UserUtils.PrintError(userCmd + " is not a valid command! Please, try again!");
                        break;
                }

            }
            return playerCommand;
        }

        private void SetUpHelp() {
            string str = "To interact with the game, type in one of the following commands.\n" +
                "The game command processing is not case sensitive. \n\n" +
                "Avaiable commands are: ";
            StringBuilder msg = new StringBuilder(str);
            int numOfCmds = Command.GetCommands().Count;
            for (int i = 0; i < numOfCmds; i++) {
                if (i == numOfCmds - 1)
                    msg.Append(Command.GetCommands()[i]);
                else
                    msg.Append(Command.GetCommands()[i] + " | ");
            }
            UserUtils.PrintSucess(msg.ToString());
        }

        private AttackCommand SetUpAttack(Board boards, Player oponent) {
            AttackCommand attackCmd = new AttackCommand();
            Coord selPos = boards.SelectUnit(Pawns);
            Coord cursorCp = new Coord(Cursor.X, Cursor.Y);
            Cursor = selPos;
            if (GetPawnAt(selPos) is ABasicPawn ally) {
                Dijkstra didi = new Dijkstra(boards.GetBoard(), selPos, ally.GetAtkRange());
                List<Coord> atkRange = didi.GetValidPaths(Command.ATTACK);
                Coord enemyPos = boards.SelectPosition(cursorCp, selPos, Command.ATTACK, atkRange);

                // Set up command variables
                attackCmd.SetUp(selPos, enemyPos, this, oponent, boards);
            }
            return attackCmd;
        }

        private MoveCommand SetUpMove(Board boards) {
            MoveCommand move = new MoveCommand();
            Coord selPos = boards.SelectUnit(Pawns);
            APawn ally = GetPawnAt(selPos);
            Coord cursorCp = new Coord(Cursor.X, Cursor.Y);
            Cursor = selPos;
            if (ally != null) {
                Dijkstra didi = new Dijkstra(boards.GetBoard(), selPos, ally.GetMovePoints());
                List<Coord> moveRange = didi.GetValidPaths(Command.MOVE);
                Coord target = boards.SelectPosition(cursorCp, selPos, Command.MOVE, moveRange);

                // Set up command variables
                move.SetUp(this, selPos, target, boards);
            }
            return move;
        }

        private InspectCommand SetUpInspect(Board boards, Player oponent) {
            InspectCommand inspect = new InspectCommand();
            List<Unit> allUnits = new List<Unit>();
            allUnits.AddRange(GetUnits());
            allUnits.AddRange(oponent.GetUnits());
            Coord selPos = boards.SelectUnit(allUnits);
            Cursor = selPos;

            inspect.SetUp(selPos, boards, allUnits);
            return inspect;
        }
    }
}
