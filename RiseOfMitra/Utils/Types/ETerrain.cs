namespace Utils.Types
{
    public enum ETerrain { MOUNTAIN, PLAIN, RIVER, FIELD, MARSH, FOREST, DESERT }

    public static class TerrainUtil
    {
        public static string Convert(this ETerrain terrain)
        {
            switch (terrain) {
                case ETerrain.MOUNTAIN:
                    return "MOUNTAIN";
                case ETerrain.PLAIN:
                    return "PLAIN";
                case ETerrain.RIVER:
                    return "RIVER";
                case ETerrain.FIELD:
                    return "FIELD";
                case ETerrain.MARSH:
                    return "MARSH";
                case ETerrain.FOREST:
                    return "FOREST";
                case ETerrain.DESERT:
                    return "DESERT";
            }
            return "ERROR";
        }
    }
}
