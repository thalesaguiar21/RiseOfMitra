using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoMUtils;

namespace RiseOfMitra
{
    class DalrionPawn : APawn
    {
        public DalrionPawn(Pair position)
        {
            SetAtk(3);
            SetDef(2);
            SetLifePoints(5);
            SetMovePoints(3);
            SetPosition(position);
            SetRange(1);
            SetSize(1);
        }

        public override void AdaptForTerrain(ETerrainType currTerrain)
        {
            switch (currTerrain)
            {
                case ETerrainType.Mountain:
                    this.SetMovePoints(this.GetMovePoints() - 1);
                    break;
                case ETerrainType.Plain:
                    this.SetAtk(this.GetAtk() + 1);
                    break;
                case ETerrainType.River:
                    this.SetAtk(this.GetAtk() + 1);
                    break;
                case ETerrainType.Field:
                    this.SetDef(this.GetDef() + 1);
                    break;
                case ETerrainType.Marsh:
                    this.SetDef(this.GetDef() - 1);
                    break;
                case ETerrainType.Forest:
                    this.SetMovePoints(this.GetMovePoints() + 1);
                    break;
                case ETerrainType.Desert:
                    this.SetMovePoints(this.GetMovePoints() + 2);
                    break;
                default:
                    Console.WriteLine("Terreno inválido, não foi possível adaptar o peão.");
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
