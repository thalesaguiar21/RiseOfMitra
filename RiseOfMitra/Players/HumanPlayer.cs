using System;
using System.Collections.Generic;
using System.Text;

using Boards;

using Units;

using Utils;
using Utils.Types;
using Utils.Space;

using RiseOfMitra.MonteCarlo;
using RiseOfMitra.Players.Commands;

namespace RiseOfMitra.Players
{
    public class HumanPlayer : Player
    {
        public HumanPlayer(ECultures native)
        {
            Culture = native;
            Pawns = new List<APawn>();
            CultCenter = null;
            Cursor = new Coord(1, 1);
        }

        public override Node PrepareAction(Node currState, Player oponent)
        {
            var partialCommand = GetCmd(currState.Boards, oponent);
            var state = new Node(currState.Boards, partialCommand);
            return state;
        }

        public override Player Copy(Board board)
        {
            var human = new HumanPlayer(GetCulture());
            var tmpCursor = new Coord(GetCursor().X, GetCursor().Y);
            for (int i = 0; i < GetPawns().Count; i++) {
                var tmpPawn = GetPawns()[i].Copy(board);
                human.AddPawn(tmpPawn);
            }
            human.SetCultCenter(CultCenter.Copy(board));
            human.SetCursor(tmpCursor);
            return human;
        }

        private ACommand GetCmd(Board boards, Player oponent)
        {
            Console.Write("{0} turn...", this.GetCulture());
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
                        playerCommand = SetUpMove(boards, oponent);
                        validCmd = true;
                        break;
                    case Command.INSPECT:
                        playerCommand = SetUpInspect(boards, oponent);
                        validCmd = true;
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

        private void SetUpHelp()
        {
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

        private AttackCommand SetUpAttack(Board boards, Player oponent)
        {
            var validCells = new List<Coord>();
            var selPos = boards.SelectUnit(ref validCells, Pawns, Command.ATTACK);
            var cursorCp = new Coord(Cursor.X, Cursor.Y);
            Cursor = selPos;
            var enemyPos = boards.SelectPosition(cursorCp, selPos, Command.ATTACK, validCells);

            return new AttackCommand(selPos, enemyPos, this, oponent);
        }

        private MoveCommand SetUpMove(Board boards, Player oponent)
        {
            var validCells = new List<Coord>();
            var selPos = boards.SelectUnit(ref validCells, Pawns, Command.MOVE);
            var cursorCp = new Coord(Cursor.X, Cursor.Y);
            Cursor = selPos;
            var target = boards.SelectPosition(cursorCp, selPos, Command.MOVE, validCells);

            return new MoveCommand(selPos, target, this, oponent);
        }

        private InspectCommand SetUpInspect(Board boards, Player oponent)
        {
            var allUnits = new List<Unit>();
            var validPositions = new List<Coord>();
            allUnits.AddRange(GetUnits());
            allUnits.AddRange(oponent.GetUnits());
            var selPos = boards.SelectUnit(ref validPositions, allUnits, "");
            Cursor = selPos;

            return new InspectCommand(selPos);
        }
    }
}
