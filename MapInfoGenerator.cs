using System;
using System.Drawing;
using System.Linq;
using System.Numerics;

using GenieLib;

using War3Net.Build;
using War3Net.Build.Common;
using War3Net.Build.Extensions;
using War3Net.Build.Info;

namespace ScenarioConverter
{
    public static class MapInfoGenerator
    {
        public static MapInfo Generate(Scenario scenario, RectangleMargins padding)
        {
            var map = scenario.Map;

            // var mapInfo = MapInfo.Default;
            var mapInfo = new MapInfo(MapInfoFormatVersion.Lua)
            {
                MapVersion = 1,
                EditorVersion = 6072,
                GameVersion = GamePatch.v1_31_1.ToVersion(),

                MapName = scenario.ScenarioName,
                RecommendedPlayers = scenario.PlayerCount.ToString(),

                CameraBounds = new Quadrilateral(0, map.width * 128, map.height * 128, 0),
                CameraBoundsComplements = padding,
                PlayableMapAreaWidth = (int)map.width,
                PlayableMapAreaHeight = (int)map.height,

                MapFlags = MapFlags.UseItemClassificationSystem | MapFlags.HasMapPropertiesMenuBeenOpened | MapFlags.MeleeMap,

                Tileset = Tileset.LordaeronSummer,

                LoadingScreenBackgroundNumber = -1,

                GameDataSet = GameDataSet.Unset,

                FogStyle = FogStyle.Linear,
                FogStartZ = 3000f,
                FogEndZ = 5000f,
                FogDensity = 0.5f,
                FogColor = Color.Black,

                GlobalWeather = WeatherType.None,
                WaterTintingColor = Color.White,

                ScriptLanguage = ScriptLanguage.Lua,
            };

            const string NamePrefix = "MULTIPLAYER ";
            const string NameSuffix = ").SCN";
            if (scenario.ScenarioName.StartsWith(NamePrefix, StringComparison.OrdinalIgnoreCase))
            {
                var name = scenario.ScenarioName[NamePrefix.Length..];
                var split = name.Split('(');
                if (split.Length == 2)
                {
                    mapInfo.MapName = split[0].Trim();
                    mapInfo.MapAuthor = "Ensemble Studios";
                    if (split[1].EndsWith(NameSuffix, StringComparison.OrdinalIgnoreCase))
                    {
                        mapInfo.RecommendedPlayers = split[1].Substring(0, split[1].Length - NameSuffix.Length);
                    }
                }
            }

            // TODO: escape \r \n chars
            // info.MapDescription = scenario.Messages[0];

            //mapInfo.MapFlags |= MapFlags.MeleeMap;
            //mapInfo.MapFlags &= ~(MapFlags.UseCustomForces | MapFlags.ShowWaterWavesOnCliffShores | MapFlags.ShowWaterWavesOnRollingShores | MapFlags.MaskedAreasArePartiallyVisible);

            var players = new PlayerData[scenario.PlayerCount];
            for (var i = 0; i < scenario.PlayerCount; i++)
            {
                var player = new PlayerData();
                player.Id = i;
                player.Name = $"Player {i + 1}";
                var playerData = scenario.PlayerData[i];

                // todo: unlock tech depending on starting age
                // info.SetTechData(i, playerData.Age);

                player.Controller = PlayerController.User;
                // player.Controller = playerData.IsHuman ? PlayerController.User : PlayerController.Computer;
                player.Race = PlayerRace.Human;
                player.Flags = PlayerFlags.FixedStartPosition;

                var sloc = scenario.Entities[i + 1].FirstOrDefault(entity => entity.Type == AoeEntityType.TownCenter);
                if (sloc is null)
                {
                    sloc = scenario.Entities[i + 1].First();
                }

                player.StartPosition = new Vector2(sloc.X * 128, sloc.Y * 128);

                player.AllyLowPriorityFlags = new Bitmask32();
                player.AllyHighPriorityFlags = new Bitmask32();

                players[i] = player;
            }

            var force = new ForceData();
            force.Name = "Force 1";
            force.SetPlayers(players);

            mapInfo.Players.AddRange(players);
            mapInfo.Forces.Add(force);

            return mapInfo;
        }
    }
}