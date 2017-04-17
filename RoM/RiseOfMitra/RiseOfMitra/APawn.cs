using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoMUtils;
namespace RiseOfMitra
{
    abstract class APawn : AUnit
    {
        private int MovePoints;
        private ECultures NativeCulture;
        private int Atk;
        private int Def;
        private int Range;
        private ETerrainType[] MasterTerrains;
        private const int MAX_DEF = 30;
        private const int MAX_ATK = 35;
        private const int MAX_MOV = 5;

        // Main methods
        public abstract bool Attack(APawn enemy);
        public abstract void AdaptForTerrain(ETerrainType currTerrain);
        public abstract Pair Move(Pair target);
        public abstract Pair Move(Pair[] targetPath);

        // Getters
        public int GetMovePoints() { return this.MovePoints; }
        public ECultures GetNativeCulture() { return this.NativeCulture; }
        public int GetAtk() { return this.Atk; }
        public int GetDef() { return this.Def; }
        public int GetRange() { return this.Range; }
        public ETerrainType[] GetMasterTerrains() { return this.MasterTerrains; }

        // Setters
        public void SetMovePoints(int movePoints)
        {
            bool validMov = movePoints >= 0 && movePoints <= MAX_MOV;
            if (validMov)
            {
                MovePoints = movePoints;
            }
            else
            {
                Console.WriteLine("Os pontos de movimentação de um peão devem estar entre 0 e " + MAX_MOV);
            }
        }

        public void SetNativeCulture(ECultures culture)
        {
            this.NativeCulture = culture;
        }

        public void SetAtk(int atk)
        {
            bool validAtk = atk >= 0 && atk <= MAX_ATK;
            if (validAtk)
            {
                Atk = atk;
            }
            else
            {
                Console.WriteLine("O ataque de um peão de estar entre 0 e " + MAX_ATK);
            }
        }


        public void SetDef(int def)
        {
            bool validDef = def >= 0 && def <= MAX_DEF;
            if (validDef)
            {
                Def = def;
            }
            else
            {
                Console.WriteLine("A defesa do peão deve estar entre 0 e " + MAX_DEF);
            }
        }

        public void SetRange(int range)
        {
            if(range > 0 || range > 0 || range < GameConsts.BOARD_SIZE || range < GameConsts.BOARD_SIZE)
            {
                this.Range = range;
            }
            else
            {
                Console.WriteLine("Alcançe da unidade fora dos limites!");
            }
        }


        public void SetMasterTerrains(ETerrainType[] terrains)
        {
            if (terrains != null)
            {
                MasterTerrains = terrains;
            }
            else
            {
                Console.WriteLine("Terrenos da unidade nulos!");
            }
        }
    }
}
