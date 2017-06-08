namespace Types
{
    public enum ETerrain
    {
        MOUNTAIN,
        PLAIN,
        RIVER,
        FIELD,
        MARSH,
        FOREST,
        DESERT
    }

    public static class TerrainUtil
    {
        public static string Convert(this ETerrain terrain) {
            switch (terrain) {
                case ETerrain.MOUNTAIN:
                    return "MTN";
                case ETerrain.PLAIN:
                    return "PLN";
                case ETerrain.RIVER:
                    return "RVR";
                case ETerrain.FIELD:
                    return "FLD";
                case ETerrain.MARSH:
                    return "MRS";
                case ETerrain.FOREST:
                    return "FST";
                case ETerrain.DESERT:
                    return "DSR";
            }
            return "ERROR";
        }
    }
}
