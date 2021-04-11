using System;
using System.Collections.Generic;
using System.Numerics;

using GenieLib;

using War3Net.Build.Common;
using War3Net.Build.Widget;
using War3Net.Common.Extensions;

namespace ScenarioConverter
{
    public static class MapDoodadsGenerator
    {
        private static readonly Dictionary<AoeTerrainType, int> _treeTypes;

        static MapDoodadsGenerator()
        {
            _treeTypes = new Dictionary<AoeTerrainType, int>
            {
                { AoeTerrainType.Forest, "LTlt".FromRawcode() },
                { AoeTerrainType.PalmDesert, "BTtw".FromRawcode() },
                { AoeTerrainType.PineForest, "ATtr".FromRawcode() },
                { AoeTerrainType.Jungle, "BTtw".FromRawcode() },
            };
        }

        public static MapDoodads Generate(ScenarioMap map, RectangleMargins padding)
        {
            var width = (int)map.width;
            var height = (int)map.height;
            var doodads = new List<DoodadData>();

            var rnd = new Random();

            for (uint x = 0; x < width; x++)
            {
                for (uint y = 0; y < height; y++)
                {
                    var tileType = map.terrain[x, y].cnst;
                    if (_treeTypes.TryGetValue(tileType, out var treeType))
                    {
                        doodads.Add(new DoodadData
                        {
                            TypeId = treeType,
                            Variation = rnd.Next(tileType == AoeTerrainType.PineForest ? 5 : 10),
                            Position = 128f * new Vector3(x - padding.Left, y - padding.Bottom, 0f),
                            Rotation = 0f,
                            Scale = Vector3.One,
                            CreationNumber = doodads.Count,

                            State = DoodadState.Normal,
                            Life = 100,
                        });
                    }
                }
            }

            // TODO: convert scenario entities to doodads

            return new MapDoodads(MapWidgetsFormatVersion.TFT, MapWidgetsSubVersion.V11, false)
            {
                Doodads = doodads,
            };
        }
    }
}