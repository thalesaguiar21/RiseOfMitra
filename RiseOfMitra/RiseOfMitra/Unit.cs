using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoMUtils;

namespace RiseOfMitra
{
    class Unit
    {
        protected string BOARD_CHAR;
        private int CurrLife;
        private int Def;
        private Coord Pos;
        private int Size;
        private ECultures native;
        private const int MAX_LIFE = 3000;
        private const int MAX_DEF = 30;
        private const int MAX_SIZE = 5;


        // Verify if the target is contained in the Unit
        public bool InUnit(Coord target)
        {
            for (int i = 0; i < Size-1; i++)
            {
                for (int j = 0; j < Size-1; j++)
                {
                    if (Pos.X + i == target.X && Pos.Y + j == target.Y)
                        return true;
                }
            }
            return false;
        }

        public override string ToString()
        {
            return BOARD_CHAR;
        }

        public int GetCurrLife() { return this.CurrLife; }
        public int GetDef() { return this.Def; }
        public Coord GetPos() { return this.Pos; }
        public int GetSize() { return this.Size; }
        public ECultures NativeOf() { return this.native; }

        public void SetCurrLife(int life)
        {
            if(life > MAX_LIFE)
            {
                Console.WriteLine("Unit life can't exceed " + MAX_LIFE);
            }
            else
            {
                this.CurrLife = life;
            }
        }

        public void SetDef(int def)
        {
            if (def > MAX_DEF || def < 0)
                Console.WriteLine("Unit's defense can't exceed " + MAX_DEF + " or be lower than 0!");
            else
            {
                this.Def = def;
            }
        }

        public void SetPos(Coord pos)
        {
            if (pos.X >= GameConsts.BOARD_LIN || pos.X < 0
                || pos.Y >= GameConsts.BOARD_COL || pos.Y < 0)
                Console.WriteLine(pos + " isn't a valid position!");
            else
            {
                this.Pos = pos;
            }
        }

        public void SetSize(int size)
        {
            if (size <= 0 || size > MAX_SIZE)
                Console.WriteLine(size + " isn't a valid size!");
            else
            {
                this.Size = size;
            }
        }

        public void SetCulture(ECultures culture)
        {
            this.native = culture;
        }
    }
}
