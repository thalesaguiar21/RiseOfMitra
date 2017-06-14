using Types;

namespace Units
{
    interface IAdaptable
    {
        void Adapt(ETerrain prevTerrain, ETerrain nextTerrain);
    }
}
