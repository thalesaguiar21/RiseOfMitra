using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoMUtils;

namespace RiseOfMitra
{
    class RahkarsPawn : APawn
    {
        public override void AdaptForTerrain(ETerrainType currTerrain)
        {
            switch (currTerrain)
            {
                case ETerrainType.Mountain:
                    this.SetMovePoints(this.GetMovePoints() + 1);
                    break;
                case ETerrainType.Plain:
                    SetDef(GetDef() + 1);
                    break;
                case ETerrainType.River:
                    SetMovePoints(GetMovePoints() - 1);
                    break;
                case ETerrainType.Field:
                    SetDef(GetDef() + 1);
                    break;
                case ETerrainType.Marsh:
                    SetAtk(GetAtk() + 2);
                    break;
                case ETerrainType.Forest:
                    SetAtk(GetAtk() + 1);
                    break;
                case ETerrainType.Desert:
                    SetAtk(GetAtk() - 1);
                    break;
                default:
                    break;
            }
        }

        public override bool Attack(APawn enemy)
        {
            // Verifica se o inimigo está no alcançe de ataque
            if (Pair.Distance(enemy.GetPosition(), this.GetPosition()) <= this.GetRange())
            {
                int result = this.GetAtk() - enemy.GetDef();
                int currLife = (result > 0) ? (enemy.GetLifePoints() - result) : (0);
                enemy.SetLifePoints(currLife);
                return true;
            }
            return false;
        }

        public override Pair Move(Pair target)
        {
            throw new NotImplementedException();
        }

        public override Pair Move(Pair[] targetPath)
        {
            throw new NotImplementedException();
        }
    }
}
