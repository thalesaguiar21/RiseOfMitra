using Types;

namespace Game
{
    interface IAdaptable
    {
        void Adapt(ETerrain prevTerrain, ETerrain nextTerrain);
    }
}
