namespace GenieLib
{
    // https://github.com/dderevjanik/agescx-js/blob/master/packages/data/Terrain.ts
    public enum AoeTerrainType : byte
    {
        Grass = 0,
        Water = 1,
        Beach = 2,
        Shallows = 4,
        Desert = 6,
        Forest = 10,
        PalmDesert = 13,
        PineForest = 19,
        Jungle = 20,
        DeepWater = 22,

        Undefined = 255,
    }
}