using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoMUtils;
namespace RiseOfMitra
{
    abstract class APawn : AUnit, IMovable
    {
        private int MovePoints;
        private int Atk;
        private int Def;
        private ETerrainType[] MasterTerrains;

        public int GetMovePoints() { return this.MovePoints; }
        public int GetAtk() { return this.Atk; }
        public int GetDef() { return this.Def; }
        public ETerrainType[] GetMasterTerrains() { return this.MasterTerrains; }

        public abstract bool Attack(APawn enemy);

        public abstract void AdaptForTerrain(ETerrainType currTerrain);

        public abstract Pair Move(Pair target);

        public abstract Pair Move(Pair[] targetPath);
    }
}
