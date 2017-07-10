using System;
using System.Text;
using System.Runtime.CompilerServices;
using Boards;
using Utils.Types;
using Utils.Space;

namespace Units
{
    public class Unit
    {
        protected string BOARD_CHAR;
        private int CurrLife;
        private int TotalLife;
        private int Def;
        private Coord Pos;
        private int Size;
        private ECultures native;
        private const int MAX_LIFE = 3000;
        private const int MAX_DEF = 30;
        private const int MAX_SIZE = 5;

        public virtual string GetStatus() {
            StringBuilder msg = new StringBuilder();
            msg.Append("Life: " + CurrLife + "/" + TotalLife + "\n");
            msg.Append("Def: " + Def + "\n");
            msg.Append("Position: " + Pos + "\n");
            msg.Append("Size: " + Size + "\n");
            msg.Append("Culture: " + native + "\n");
            return msg.ToString();
        }
        
        public bool InUnit(Coord target) {
            for (int i = 0; i < Size; i++) {
                for (int j = 0; j < Size; j++) {
                    if (Pos.X + i == target.X && Pos.Y + j == target.Y)
                        return true;
                }
            }
            return false;
        }

        public void Erase(Board boards) {
            Coord tmpPos = new Coord(Pos.X - Size / 2, Pos.Y - Size / 2);
            for (int i = 0; i < GetSize(); i++) {
                for (int k = 0; k < GetSize(); k++) {
                    Coord pos = new Coord(tmpPos.X + i, tmpPos.Y + k);
                    boards.SetCellAt(pos, BoardConsts.EMPTY);
                }
            }
        }

        public void Place(Board boards) {
            Coord tmpPos = new Coord(Pos.X - Size / 2, Pos.Y - Size / 2);
            for (int i = 0; i < GetSize(); i++) {
                for (int k = 0; k < GetSize(); k++) {
                    Coord pos = new Coord(tmpPos.X + i, tmpPos.Y + k);
                    boards.SetCellAt(pos, ToString());
                }
            }
        }

        public override string ToString() {
            return BOARD_CHAR;
        }

        public int GetCurrLife() { return CurrLife; }
        public int GetTotalLife() { return TotalLife; }
        public int GetDef() { return Def; }
        public Coord GetPos() { return Pos; }
        public int GetSize() { return Size; }
        public ECultures NativeOf() { return native; }

        public void SetCurrLife(int life) {
            if (life > MAX_LIFE) {
                Console.WriteLine("Unit life can't exceed " + MAX_LIFE);
            } else {
                CurrLife = life;
            }
        }

        public void SetTotalLife(int totalLife) {
            if (totalLife >= 0 && totalLife <= MAX_LIFE)
                TotalLife = totalLife;
        }

        public void SetDef(int def) {
            if (def > MAX_DEF || def < 0)
                Console.WriteLine("Unit's defense can't exceed " + MAX_DEF + " or be lower than 0!");
            else {
                Def = def;
            }
        }

        public void SetPos(Coord pos) {
            if (pos.X >= BoardConsts.MAX_LIN || pos.X < 0
                || pos.Y >= BoardConsts.MAX_COL || pos.Y < 0)
                Console.WriteLine(pos + " isn't a valid position!");
            else {
                Pos = pos;
            }
        }

        protected void SetSize(int size) {
            if (size <= 0 || size > MAX_SIZE)
                Console.WriteLine(size + " isn't a valid size!");
            else {
                Size = size;
            }
        }

        public void SetCulture(ECultures culture) {
            native = culture;
        }
    }
}
