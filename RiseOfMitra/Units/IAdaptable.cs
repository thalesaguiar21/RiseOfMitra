using Utils.Types;

namespace Units
{
    /// <summary>
    /// This class represents every unit that can move across the board and is able to
    /// change its attributes accordingly to its actual position and terrain.
    /// </summary>
    interface IAdaptable  
    {
        /// <summary>
        /// This method most be used to remove any bonuses acquired in the unit's
        /// previous position.
        /// </summary>
        void UnAdapt(ETerrain terrain);

        /// <summary>
        /// This method most be used to give all the bonuses to the unit's actual/new position.
        /// </summary>
        void ReAdapt(ETerrain terrain);

        /// <summary>
        /// This method will adapt an unit by removing any previous bonuses from its last position
        /// and giving it new ones accordingly to its new position.
        /// </summary>
        void Adapt(ETerrain prev, ETerrain actual);
    }
}
