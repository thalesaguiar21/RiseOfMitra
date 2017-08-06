using System;
using System.Text;
using System.Runtime.CompilerServices;
using Boards;
using Utils;
using Utils.Types;
using Utils.Space;

namespace Units
{
    public class Unit
    {
        protected string BOARD_CHAR;
        private int totalLife;
        private int currLife;
        private int def;
        private int size;
        private Coord position;
        private const int MAX_LIFE = 3000;
        private const int MAX_DEF = 30;
        private const int MAX_SIZE = 5;

        public int TotalLife
        {
            get { return totalLife; }
            set
            {
                if ((value >= 0) && (value <= MAX_LIFE))
                    totalLife = value;
            }
        }


        public int CurrLife
        {
            get { return currLife; }
            set
            {
                if (value <= TotalLife)
                    currLife = value;
            }
        }

        public int Def
        {
            get { return def; }
            set
            {
                if ((value >= 0) && (value <= MAX_DEF))
                    def = value;
            }
        }

        public int Size
        {
            get { return size; }
            set
            {
                if ((value >= 0) && (value % 2 == 1))
                    size = value;
            }
        }

        public Coord Position
        {
            get { return position; }
            set
            {
                if (Coord.IsValid(value))
                    position = value;
            }
        }

        public ECultures Culture { get; set; }

        public virtual string GetStatus() {
            StringBuilder msg = new StringBuilder();
            msg.Append("Life: " + CurrLife + "/" + TotalLife + "\n");
            msg.Append("Def: " + Def + "\n");
            msg.Append("Position: " + Position + "\n");
            msg.Append("Size: " + Size + "\n");
            msg.Append("Culture: " + Culture + "\n");
            return msg.ToString();
        }

        public bool InUnit(Coord target) {
            int pX = Position.X - (size / 2);
            int pY = Position.Y - (size / 2);
            for (int i = 0; i < Size; i++) {
                for (int j = 0; j < Size; j++) {
                    if (pX + i == target.X && pY + j == target.Y)
                        return true;
                }
            }
            return false;
        }

        public void Erase(Board boards) {
            Coord tmpPos = new Coord(Position.X - Size / 2, Position.Y - Size / 2);
            for (int i = 0; i < Size; i++) {
                for (int k = 0; k < Size; k++) {
                    Coord pos = new Coord(tmpPos.X + i, tmpPos.Y + k);
                    boards.SetCellAt(pos, BoardConsts.EMPTY);
                }
            }
        }

        public void Place(Board boards) {
            Coord tmpPos = new Coord(Position.X - Size / 2, Position.Y - Size / 2);
            for (int i = 0; i < Size; i++) {
                for (int k = 0; k < Size; k++) {
                    Coord pos = new Coord(tmpPos.X + i, tmpPos.Y + k);
                    boards.SetCellAt(pos, ToString());
                }
            }
        }

        public override string ToString() {
            return BOARD_CHAR;
        }
    }
}
