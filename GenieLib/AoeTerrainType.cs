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
        PalmDesert = 13,
        Jungle = 20,
        DeepWater = 20,

        Undefined = 255,
    }
}