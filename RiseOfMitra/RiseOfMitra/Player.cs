using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoMUtils;

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
            Pawns = null;
            Center = null;
            Cursor = new Coord(0, 0);
        }

        public ABasicPawn PawnAt(Coord pos)
        {
            for (int i = 0; i < Pawns.Capacity; i++)
            {
                if (Pawns[i].GetPos() == pos) return Pawns[i];
            }
            return null;
        }

        public ECultures GetCulture() { return this.Culture; }
        public List<ABasicPawn> GetPawns() { return this.Pawns; }
        public CulturalCenter GetCenter() { return this.Center; }

        public void SetCulture(ECultures cult)
        {
            this.Culture = cult;
        }

        public void SetPawns(List<ABasicPawn> pawns)
        {
            if(pawns != null)
                this.Pawns = pawns;
        }

        public void SetCulturalCenter(CulturalCenter center)
        {
            if (center != null)
                this.Center = center;
        }
    }
}
