using System;
using System.Collections.Generic;

using GenieLib;

using War3Net.Build.Common;
using War3Net.Build.Environment;

namespace ScenarioConverter
{
    public static class MapEnvironmentGenerator
    {
        private static readonly List<TerrainType> _tileset;
        private static readonly Dictionary<AoeTerrainType, int> _terrainTypes;

        static MapEnvironmentGenerator()
        {
            _tileset = new List<TerrainType>();
            _terrainTypes = new Dictionary<AoeTerrainType, int>();

            static void AddTerrainType(AoeTerrainType from, TerrainType to)
            {
                var terrainTypeIndex = _tileset.Count;

                if (!_tileset.Contains(to))
                {
                    _terrainTypes.Add(from, terrainTypeIndex);
                    _tileset.Add(to);
                }
                else
                {
                    _terrainTypes.Add(from, _tileset.IndexOf(to));
                }
            }

            AddTerrainType(AoeTerrainType.Grass, TerrainType.L_Grass);
            AddTerrainType(AoeTerrainType.Water, TerrainType.I_Ice);
            AddTerrainType(AoeTerrainType.Beach, TerrainType.Z_Sand);
            AddTerrainType(AoeTerrainType.Shallows, TerrainType.N_Snow);
            AddTerrainType(AoeTerrainType.Desert, TerrainType.B_Desert);
            AddTerrainType(AoeTerrainType.Forest, TerrainType.L_Grass);
            AddTerrainType(AoeTerrainType.PalmDesert, TerrainType.B_Desert);
            AddTerrainType(AoeTerrainType.PineForest, TerrainType.L_Grass);
            AddTerrainType(AoeTerrainType.Jungle, TerrainType.L_Grass);
            AddTerrainType(AoeTerrainType.DeepWater, TerrainType.I_DarkIce);

            AddTerrainType(AoeTerrainType.Undefined, TerrainType.O_Abyss);
        }

        public static MapEnvironment Generate(ScenarioMap map)
        {
            var width = (int)map.width;
            var height = (int)map.height;
            var mapTiles = new MapTile[map.width, map.height];

            var paddingMapTile = new MapTile();
            paddingMapTile.Texture = _terrainTypes[AoeTerrainType.Undefined];
            paddingMapTile.CliffLevel = 2;
            paddingMapTile.IsEdgeTile = true;

            var rnd = new Random();

            for (uint x = 0; x < width; x++)
            {
                for (uint y = 0; y < height; y++)
                {
                    var tile = map.terrain[x, y];
                    var tileType = tile.cnst;

                    var mapTile = new MapTile();
                    mapTiles[x, y] = mapTile;

                    if (rnd.Next(4) == 0)
                    {
                        mapTile.Variation = rnd.Next(16);
                    }

                    if (!_terrainTypes.TryGetValue(tileType, out var terrainTypeIndex))
                    {
                        terrainTypeIndex = _terrainTypes[AoeTerrainType.Undefined];
                    }

                    mapTile.Texture = terrainTypeIndex;
                    mapTile.CliffLevel = 2;
                    // todo: mapTile.IsWater
                    mapTile.Height = tile.elev * 0.25f;
                }
            }

            return new MapEnvironment(Tileset.LordaeronSummer, _tileset, mapTiles);
        }
    }
}