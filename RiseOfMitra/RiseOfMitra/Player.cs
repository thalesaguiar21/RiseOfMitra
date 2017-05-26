﻿using System;
using System.Collections.Generic;
using Types;
using Cells;

namespace RiseOfMitra
{
    class Player
    {
        private ECultures Culture;
        private List<ABasicPawn> Pawns;
        private CulturalCenter Center;
        private Coord Cursor;
        // Lista de templos

        public Player()
        {
            Culture = ECultures.DEFAULT;
            Pawns = new List<ABasicPawn>();
            Center = null;
            Cursor = new Coord(1, 1);
        }

        public ABasicPawn PawnAt(Coord pos)
        {
            for (int i = 0; i < Pawns.Count; i++)
            {
                if (Pawns[i].GetPos().IsSame(pos))
                    return Pawns[i];
            }
            return null;
        }

        public ECultures GetCulture() { return Culture; }
        public List<ABasicPawn> GetPawns() { return Pawns; }
        public CulturalCenter GetCenter() { return Center; }
        public Coord GetCursor() { return Cursor; }

        public void SetCulture(ECultures cult)
        {
            Culture = cult;
        }

        public void SetPawns(List<ABasicPawn> pawns)
        {
            if(pawns != null)
                Pawns = pawns;
        }

        public void SetCulturalCenter(CulturalCenter center)
        {
            if (center != null)
                Center = center;
        }

        public void SetCursor(Coord nCursor)
        {
            if (Coord.IsValid(nCursor))
                Cursor = nCursor;
        }

        public void AddPawn(ABasicPawn pawn)
        {
            if(pawn != null && pawn.NativeOf() == Culture)
            {
                Pawns.Add(pawn);
            }
        }
    }
}
