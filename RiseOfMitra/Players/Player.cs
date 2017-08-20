using System;
using System.Collections.Generic;

using Boards;

using Units.Pawns;
using Units.Centers;
using Units;

using Utils.Types;
using Utils.Space;
using Utils;

using RiseOfMitra.MonteCarlo;

namespace RiseOfMitra.Players
{
    /// <summary>
    /// This class represents a player in Rise of Mitra. Every type of playr must inherit from this
    /// class. With it, player can perform turn events like receiving more pawns and prepare actions
    /// to be performed by the game.
    /// </summary>
    public abstract class Player
    {
        protected ECultures Culture;
        protected List<APawn> Pawns;
        protected CulturalCenter CultCenter;
        protected Coord Cursor;
        protected int Turn;

        public APawn GetPawnAt(Coord pos)
        {
            for (int i = 0; i < Pawns.Count; i++) {
                if (Pawns[i].Position.Equals(pos))
                    return Pawns[i];
            }
            return null;
        }

        public ABasicPawn GetBasicPawnAt(Coord target)
        {
            for (int i = 0; i < Pawns.Count; i++) {
                if (Pawns[i].Position.Equals(target) && Pawns[i] is ABasicPawn)
                    return (ABasicPawn)Pawns[i];
            }
            return null;
        }

        public List<Unit> GetUnits()
        {
            List<Unit> playerUnits = new List<Unit>();
            playerUnits.AddRange(Pawns);
            playerUnits.Add(CultCenter);
            return playerUnits;
        }

        public Unit GetUnitAt(Coord pos)
        {
            Unit unit = null;
            if (pos != null) {
                List<Unit> units = GetUnits();
                for (int i = 0; i < units.Count; i++) {
                    if (units[i].InUnit(pos)) {
                        unit = units[i];
                        break;
                    }
                }
            }
            return unit;
        }

        public void AddPawn(APawn pawn)
        {
            if (pawn != null && pawn.Culture == Culture) {
                Pawns.Add(pawn);
            }
        }

        public bool RemoveUnitAt(Coord pos, Board boards)
        {
            bool found = false;
            if (CultCenter.InUnit(pos)) {
                CultCenter = null;
                found = true;
            } else {
                for (int i = 0; i < Pawns.Count; i++) {
                    if (Pawns[i].InUnit(pos)) {
                        Pawns[i].Erase(boards);
                        Pawns.RemoveAt(i);
                        found = true;
                    }
                }
            }
            return found;
        }

        /// <summary>
        /// This method will execute every turn event for the player. For instance, a pawn respawn
        /// rate.
        /// </summary>
        /// <param name="boards">The board where events should be executed.</param>
        public void ExecuteTurnEvents(Board boards)
        {
            CultCenter.Regen();
            if (Turn % CultCenter.GetUnitsPerTurn() == 0 && Pawns.Count < 6) {
                var pawn = CultCenter.GeneratePawn(boards);
                if (pawn != null) {
                    pawn.Place(boards);
                    AddPawn(pawn);
                } else {
                    UserUtils.PrintError("Can not create more pawns!");
                    Console.ReadLine();
                }
            }
        }
        /// <summary>
        /// This method allows the player to set up a certain command before executing it. This prevents
        /// some exceptions, and add a more defensive programming to the game.
        /// </summary>
        /// <param name="boards">Where the command should configurated.</param>
        /// <param name="oponent">The oponent of the current player.</param>
        /// <returns></returns>
        public abstract Node PrepareAction(Node currState, Player oponent);

        /// <summary>
        /// This is a copy constructor.
        /// </summary>
        /// <param name="board">The new board where units should be placed.</param>
        /// <returns></returns>
        public abstract Player Copy(Board board);

        /// <summary>
        /// Pawns may have a wide variety, but sometimes its only necessary to acess a determined
        /// type of pawns.
        /// </summary>
        /// <returns>A list of pawns that can perform attacks.</returns>
        public List<ABasicPawn> GetAttackers()
        {
            var attackers = new List<ABasicPawn>();

            foreach (APawn pawn in Pawns) {
                if (pawn is ABasicPawn)
                    attackers.Add((ABasicPawn)pawn);
            }
            return attackers;
        }

        public ECultures GetCulture()
        {
            return Culture;
        }

        public void SetCulture(ECultures cult)
        {
            Culture = cult;
        }

        public List<APawn> GetPawns() { return Pawns; }

        /// <summary>
        /// Assign a new list of pawns to the player attribute. The new list can not be null, int that
        /// case the list of pawns will not be assigned, that is, the list stays without modifications.
        /// </summary>
        /// <param name="pawns">The new list of pawns.</param>
        public void SetPawns(List<APawn> pawns)
        {
            if (pawns != null)
                Pawns = pawns;
        }

        public CulturalCenter GetCultCenter() { return CultCenter; }

        /// <summary>
        /// Assign a new cultural center to the player attribute. The new cultural center can not be null, int that
        /// case cultural center will not be assigned, that is, it stays without modifications.
        /// </summary>
        /// <param name="center">The new cultural center.</param>
        public void SetCultCenter(CulturalCenter center)
        {
            if (center != null)
                CultCenter = center;
        }

        public Coord GetCursor() { return Cursor; }

        /// <summary>
        /// Assign a new postion to the player's cursor. That attribute represents the player's 
        /// actual selected position. Only valid positions can be assigned to the cursor. 
        /// For informations on valid positions, see: <see cref="Coord"/>
        /// </summary>
        /// <param name="nCursor">The cursor new position.</param>
        public void SetCursor(Coord nCursor)
        {
            if (Coord.IsValid(nCursor))
                Cursor = nCursor;
        }

        /// <summary>
        /// This method will increase the player turn by 1.
        /// </summary>
        public void IncreaseTurn()
        {
            Turn += 1;
        }

        public override bool Equals(object obj)
        {
            if (obj is Player other) {
                return other.Culture == Culture;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}