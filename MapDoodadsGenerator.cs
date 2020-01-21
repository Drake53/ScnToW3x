using System;
using System.Collections.Generic;

using GenieLib;

using War3Net.Build.Widget;

namespace ScenarioConverter
{
    public static class MapDoodadsGenerator
    {
        private static readonly Dictionary<AoeTerrainType, char[]> _treeTypes;

        static MapDoodadsGenerator()
        {
            _treeTypes = new Dictionary<AoeTerrainType, char[]>();
            _treeTypes.Add(AoeTerrainType.Forest, new[] { 'L', 'T', 'l', 't' });
            _treeTypes.Add(AoeTerrainType.PalmDesert, new[] { 'B', 'T', 't', 'w' });
            _treeTypes.Add(AoeTerrainType.PineForest, new[] { 'A', 'T', 't', 'r' });
            _treeTypes.Add(AoeTerrainType.Jungle, new[] { 'B', 'T', 't', 'w' });
        }

        public static MapDoodads Generate(Scenario scenario)
        {
            var map = scenario.Map;
            var width = (int)map.width;
            var height = (int)map.height;
            var doodads = new List<MapDoodadData>();

            var rnd = new Random();

            for (uint x = 0; x < width; x++)
            {
                for (uint y = 0; y < height; y++)
                {
                    var tileType = map.terrain[x, y].cnst;
                    if (_treeTypes.TryGetValue(tileType, out var treeType))
                    {
                        doodads.Add(new MapDoodadData(treeType, rnd.Next(tileType == AoeTerrainType.PineForest ? 5 : 10), x * 128, y * 128, 0, 1, doodads.Count));
                    }
                }
            }

            // TODO: convert scenario entities to doodads

            return new MapDoodads(doodads);
        }
    }
}