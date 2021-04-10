using System;
using System.Collections.Generic;
using System.Drawing;

using GenieLib;

using War3Net.Build.Environment;
using War3Net.Build.Info;
using War3Net.Build.Providers;
using War3Net.Build.Widget;

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

        public static MapPreviewIcons GenerateIcons(MapInfo info, MapEnvironment environment, MapUnits units)
        {
            var padding = info.CameraBoundsComplements;

            var minX = environment.Left + (128 * padding.Left);
            var width = environment.Right - (128 * padding.Right) - minX;

            var minY = environment.Bottom + (128 * padding.Bottom);
            var height = environment.Top - (128 * padding.Top) - minY;

            var ratio = width / height;
            var sizeX = 255 * (ratio > 1 ? 1 : ratio);
            var sizeY = 255 * (ratio > 1 ? 1 / ratio : 1);

            var lowerX = 1 + (0.5f * (255 - sizeX));
            var upperY = 1 + (0.5f * (255 + sizeY));

            sizeX /= width;
            sizeY /= height;

            var icons = new List<PreviewIcon>();
            if (units != null)
            {
                foreach (var unit in units.Units)
                {
                    if (MapPreviewIconProvider.TryGetIcon(unit.TypeId, unit.OwnerId, out var iconType, out var iconColor))
                    {
                        icons.Add(new PreviewIcon()
                        {
                            IconType = iconType,
                            // X = (byte)(lowerX + (sizeX * (unit.Position.X - minX))),
                            // Y = (byte)(upperY - (sizeY * (unit.Position.Y - minY))),
                            X = (byte)(0.0f + lowerX + (sizeX * (unit.Position.X - minX))),
                            Y = (byte)(0.5f + upperY - (sizeY * (unit.Position.Y - minY))),
                            Color = iconColor,
                        });
                    }
                }
            }

            return new MapPreviewIcons(MapPreviewIconsFormatVersion.Normal)
            {
                Icons = icons,
            };
        }
    }
}