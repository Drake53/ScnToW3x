using System;
using System.Drawing;
using System.IO;

using GenieLib;

using War3Net.Build.Environment;
using War3Net.Build.Extensions;
using War3Net.Build.Info;
using War3Net.Build.Widget;

namespace ScenarioConverter
{
    public static class MapConverter
    {
        public static void Convert(string inputFile, string outputFolder)
        {
            var scenario = new Scenario(inputFile);
            var map = scenario.Map;

            if (scenario.FileVersion != AoeVersion.AOE1)
            {
                throw new NotSupportedException();
            }

            static uint RoundUpSize(int size)
            {
                return 1 + (uint)((size + 30) / 32) * 32;
            }

            var newWidth = RoundUpSize((int)map.width);
            var newHeight = RoundUpSize((int)map.height);
            var expandedMap = map.Expand(newWidth, newHeight, out var padding);

            var minimap = MinimapGenerator.Generate(map);
            var scaledBitmap = new Bitmap(256, 256);
            using (var graphics = Graphics.FromImage(scaledBitmap))
            {
                graphics.DrawImage(minimap, 0, 0, 256, 256);
            }

            var mapEnvironment = MapEnvironmentGenerator.Generate(expandedMap);
            mapEnvironment.Left = padding.Left * -128;
            mapEnvironment.Bottom = padding.Bottom * -128;

            Directory.CreateDirectory(outputFolder);

            minimap.Save(Path.Combine(outputFolder, "minimap.png"));

            var tga = TGASharpLib.TGA.FromBitmap(scaledBitmap, true);
            tga.Save(Path.Combine(outputFolder, "war3mapMap.tga"));

            var mapInfo = MapInfoGenerator.Generate(scenario, padding);
            using var mapInfoStream = File.Create(Path.Combine(outputFolder, MapInfo.FileName));
            using var mapInfoWriter = new BinaryWriter(mapInfoStream);
            mapInfoWriter.Write(mapInfo);

            using var mapEnvironmentStream = File.Create(Path.Combine(outputFolder, MapEnvironment.FileName));
            using var mapEnvironmentWriter = new BinaryWriter(mapEnvironmentStream);
            mapEnvironmentWriter.Write(mapEnvironment);

            var mapPathingMap = MapPathingMapGenerator.Generate(expandedMap);
            using var mapPathingMapStream = File.Create(Path.Combine(outputFolder, MapPathingMap.FileName));
            using var mapPathingMapWriter = new BinaryWriter(mapPathingMapStream);
            mapPathingMapWriter.Write(mapPathingMap);

            var mapUnits = MapUnitsGenerator.Generate(mapInfo, scenario);
            using var mapUnitsStream = File.Create(Path.Combine(outputFolder, MapUnits.FileName));
            using var mapUnitsWriter = new BinaryWriter(mapUnitsStream);
            mapUnitsWriter.Write(mapUnits);

            var mapDoodads = MapDoodadsGenerator.Generate(scenario);
            using var mapDoodadsStream = File.Create(Path.Combine(outputFolder, MapDoodads.FileName));
            using var mapDoodadsWriter = new BinaryWriter(mapDoodadsStream);
            mapDoodadsWriter.Write(mapDoodads);

            var mapIcons = MinimapGenerator.GenerateIcons(mapInfo, mapEnvironment, mapUnits);
            using var mapIconsStream = File.Create(Path.Combine(outputFolder, MapPreviewIcons.FileName));
            using var mapIconsWriter = new BinaryWriter(mapIconsStream);
            mapIconsWriter.Write(mapIcons);
        }
    }
}