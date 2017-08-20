using System;
using System.Collections.Generic;

using Boards;

using RiseOfMitra.MonteCarlo;
using RiseOfMitra.Players.Commands;

using Units.Centers;

using Utils;
using Utils.Space;
using Utils.Types;

using Units;

namespace RiseOfMitra.Players
{
    public class RandomPlayer : Player
    {
        Game CurGame;
        double Threshold;

        public RandomPlayer(CulturalCenter center, ECultures cult, Game curGame, double threshold = 0.5)
        {
            CurGame = Validate<Game>.IsNotNull("Current game can not be null!", CurGame);
            CultCenter = Validate<CulturalCenter>.IsNotNull("Cultural center can not be null!", center);
            Pawns = new List<APawn>();
            Cursor = new Coord(1, 1);
            Culture = cult;
            Threshold = threshold;
        }

        public override Player Copy(Board board)
        {
            var random = new RandomPlayer(CultCenter.Copy(board), Culture, CurGame, Threshold);
            Coord tmpCursor = new Coord(1, 1);
            for (int i = 0; i < GetPawns().Count; i++) {
                APawn tmpPawn = GetPawns()[i].Copy(board);
                random.AddPawn(tmpPawn);
            }
            random.SetCultCenter(CultCenter);
            random.SetCursor(tmpCursor);
            return random;
        }

        public override Node PrepareAction(Node currState, Player oponent)
        {

            var rand = new Random();
            var validStates = Node.FromRange(CurGame.GetBoards(), CurGame.GetValidCommands());

            if (currState != null && validStates != null && validStates.Count > 0) {
                double highest = 0.0;
                foreach (Node state in validStates) {
                    if (state.Value > highest)
                        highest = state.Value;
                }

                var filteredStates = new List<Node>();
                for (int i = 0; i < validStates.Count; i++) {
                    if (validStates[i].Value >= 0.5 * highest)
                        filteredStates.Add(validStates[i]);
                }

                int selected = rand.Next(filteredStates.Count);
                return filteredStates[selected];
            } else {
                return null;
            }
        }
    }
}
