using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

using War3Net.Build.Common;
using War3Net.Build.Info;

namespace ScenarioConverter
{
    public static class MenuMinimapGenerator
    {
        // note: these may not be correct
        private const int Min = 0x10;
        private const int Max = 0xF0;

        private static readonly Dictionary<int, Color> _colors;

        static MenuMinimapGenerator()
        {
            _colors = new Dictionary<int, Color>();
            _colors.Add(0, Color.FromArgb(unchecked((int)0xFFFF0303)));
            _colors.Add(1, Color.FromArgb(unchecked((int)0xFF0042FF)));
            _colors.Add(2, Color.FromArgb(unchecked((int)0xFF1CE6B9)));
            _colors.Add(3, Color.FromArgb(unchecked((int)0xFF540081)));
            _colors.Add(4, Color.FromArgb(unchecked((int)0xFFFFFC00)));
            _colors.Add(5, Color.FromArgb(unchecked((int)0xFFFE8A0E)));
            _colors.Add(6, Color.FromArgb(unchecked((int)0xFF20C000)));
            _colors.Add(7, Color.FromArgb(unchecked((int)0xFFE55BB0)));
        }

        public static MemoryStream Generate(MapInfo info, uint width, uint height, RectangleMargins padding)
        {
            var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream, new UTF8Encoding(false, true), true);

            writer.Write(0); // version?
            writer.Write(info.PlayerDataCount); // number of icons
            for (var i = 0; i < info.PlayerDataCount; i++)
            {
                var playerData = info.GetPlayerData(i);

                writer.Write(2); // icon type: player start location
                writer.Write(Min + ConvertPosition(playerData.StartPosition.X, (uint)padding.Left, width));
                writer.Write(Max - ConvertPosition(playerData.StartPosition.Y, (uint)padding.Bottom, height));
                writer.Write(_colors[i].ToArgb());
            }

            stream.Position = 0;
            return stream;
        }

        private static int ConvertPosition(float p, uint min, uint size)
        {
            return (int)((p - (min * 128f)) / (size * 128f) * (Max - Min));
        }
    }
}