using System;
using System.Collections.Generic;
using System.Linq;

using GenieLib;

using War3Net.Build.Common;
using War3Net.Build.Environment;
using War3Net.Build.Providers;

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
            var mapTiles = new TerrainTile[map.width, map.height];

            var rnd = new Random();

            for (uint x = 0; x < width; x++)
            {
                for (uint y = 0; y < height; y++)
                {
                    var tile = map.terrain[x, y];
                    var tileType = tile.cnst;

                    var mapTile = new TerrainTile();
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
                    mapTile.Height = tile.elev * 0.25f;

                    mapTile.WaterHeight = mapTile.Height + 0.6f;
                    mapTile.IsWater = true;

                    if (tileType == AoeTerrainType.Beach)
                    {
                        mapTile.Height -= 0.2f;
                    }

                    if (tileType == AoeTerrainType.Shallows)
                    {
                        mapTile.Height -= 0.4f;
                    }

                    if (tileType == AoeTerrainType.Water)
                    {
                        mapTile.Height -= 0.6f;
                    }

                    if (tileType == AoeTerrainType.DeepWater)
                    {
                        mapTile.Height -= 0.8f;
                    }
                }
            }

            var terrainTiles = new TerrainTile[width * height];
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    terrainTiles[x + y * width] = mapTiles[x, y];
                }
            }

            return new MapEnvironment(MapEnvironmentFormatVersion.Normal)
            {
                TerrainTiles = terrainTiles.ToList(),
                Tileset = Tileset.LordaeronSummer,
                TerrainTypes = _tileset,
                CliffTypes = TerrainTypeProvider.GetCliffTypes(Tileset.LordaeronSummer).ToList(),
                Width = (uint)(width - 1),
                Height = (uint)(height - 1),
            };
        }
    }
}