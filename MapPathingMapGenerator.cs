﻿using System;
using System.Collections.Generic;
using System.Linq;

using GenieLib;

using War3Net.Build.Environment;

namespace ScenarioConverter
{
    public static class MapPathingMapGenerator
    {
        private const PathingType PaddedPathingType = PathingType.Walk | PathingType.Fly | PathingType.Build | PathingType.Water;

        private static readonly Dictionary<AoeTerrainType, PathingType> _pathingTypes;

        static MapPathingMapGenerator()
        {
            _pathingTypes = new Dictionary<AoeTerrainType, PathingType>();
            _pathingTypes.Add(AoeTerrainType.Grass, PathingType.Water);
            _pathingTypes.Add(AoeTerrainType.Water, PathingType.Walk | PathingType.Build);
            _pathingTypes.Add(AoeTerrainType.Beach, PathingType.Build);
            _pathingTypes.Add(AoeTerrainType.Shallows, PathingType.Build);
            _pathingTypes.Add(AoeTerrainType.Desert, PathingType.Water);
            _pathingTypes.Add(AoeTerrainType.Forest, PathingType.Water);
            _pathingTypes.Add(AoeTerrainType.PalmDesert, PathingType.Water);
            _pathingTypes.Add(AoeTerrainType.PineForest, PathingType.Water);
            _pathingTypes.Add(AoeTerrainType.Jungle, PathingType.Water);
            _pathingTypes.Add(AoeTerrainType.DeepWater, PathingType.Walk | PathingType.Build);
        }

        public static MapPathingMap Generate(ScenarioMap map)
        {
            var w = map.width - 1;
            var h = map.height - 1;
            var width = w * 4;
            var height = h * 4;
            var pathTiles = new PathingType[height, width];

            for (uint x = 0; x < w; x++)
            {
                for (uint y = 0; y < h; y++)
                {
                    var tileType = map.terrain[x, y].cnst;
                    if (!_pathingTypes.TryGetValue(tileType, out var pathingType))
                    {
                        pathingType = PaddedPathingType;
                    }

                    for (var i = (x == 0 ? 2 : 0); i < (x == w ? 2 : 4); i++)
                    {
                        for (var j = (y == 0 ? 2 : 0); j < (y == h ? 2 : 4); j++)
                        {
                            pathTiles[4 * y + j - 2, 4 * x + i - 2] = pathingType;
                        }
                    }
                }
            }

            var tiles = new PathingType[width * height];
            Buffer.BlockCopy(pathTiles, 0, tiles, 0, (int)(width * height) * sizeof(PathingType));

            return new MapPathingMap(MapPathingMapFormatVersion.Normal)
            {
                Width = width,
                Height = height,
                Cells = tiles.ToList(),
            };
        }
    }
}