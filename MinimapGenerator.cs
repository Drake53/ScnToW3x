using System;
using System.Collections.Generic;
using System.Drawing;

using GenieLib;

namespace ScenarioConverter
{
    public static class MinimapGenerator
    {
        private static readonly Dictionary<AoeTerrainType, Color> _colors;

        static MinimapGenerator()
        {
            _colors = new Dictionary<AoeTerrainType, Color>();
            _colors.Add(AoeTerrainType.Grass, Color.FromArgb(unchecked((int)0xFF339927)));
            _colors.Add(AoeTerrainType.Water, Color.FromArgb(unchecked((int)0xFF007BBD)));
            _colors.Add(AoeTerrainType.Beach, Color.FromArgb(unchecked((int)0xFFE1E372)));
            _colors.Add(AoeTerrainType.Shallows, Color.FromArgb(unchecked((int)0xFF5492B0)));
            _colors.Add(AoeTerrainType.Desert, Color.FromArgb(unchecked((int)0xFFEE9B5E)));
            _colors.Add(AoeTerrainType.Forest, Color.FromArgb(unchecked((int)0xFF157615)));
            _colors.Add(AoeTerrainType.PalmDesert, Color.FromArgb(unchecked((int)0xFFFFE094)));
            _colors.Add(AoeTerrainType.PineForest, Color.FromArgb(unchecked((int)0xFF208020)));
            _colors.Add(AoeTerrainType.Jungle, Color.FromArgb(unchecked((int)0xFF357615)));
            _colors.Add(AoeTerrainType.DeepWater, Color.FromArgb(unchecked((int)0xFF003587)));
        }

        public static Bitmap Generate(ScenarioMap map)
        {
            return Generate(map, (tt) => _colors.TryGetValue(tt, out var color) ? color : Color.Black);
        }

        public static Bitmap Generate(ScenarioMap map, Func<AoeTerrainType, Color> palette)
        {
            var width = (int)map.width;
            var height = (int)map.height;
            var bitmap = new Bitmap(width, height);

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    bitmap.SetPixel(x, height - y - 1, palette(map.terrain[x, y].cnst));
                }
            }

            return bitmap;
        }
    }
}