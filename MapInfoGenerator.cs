using System;
using System.Linq;

using GenieLib;

using War3Net.Build.Common;
using War3Net.Build.Info;

namespace ScenarioConverter
{
    public static class MapInfoGenerator
    {
        public static MapInfo Generate(Scenario scenario, RectangleMargins padding)
        {
            var map = scenario.Map;
            var info = MapInfo.Default;

            info.CameraBounds = new Quadrilateral(0, map.width * 128, map.height * 128, 0);
            info.CameraBoundsComplements = padding;

            info.MapName = scenario.ScenarioName;
            info.RecommendedPlayers = scenario.PlayerCount.ToString();

            const string NamePrefix = "MULTIPLAYER ";
            const string NameSuffix = ").SCN";
            if (scenario.ScenarioName.StartsWith(NamePrefix, StringComparison.OrdinalIgnoreCase))
            {
                var name = scenario.ScenarioName.Substring(NamePrefix.Length);
                var split = name.Split('(');
                if (split.Length == 2)
                {
                    info.MapName = split[0].Trim();
                    info.MapAuthor = "Ensemble Studios";
                    if (split[1].EndsWith(NameSuffix, StringComparison.OrdinalIgnoreCase))
                    {
                        info.RecommendedPlayers = split[1].Substring(0, split[1].Length - NameSuffix.Length);
                    }
                }
            }

            // TODO: escape \r \n chars
            // info.MapDescription = scenario.Messages[0];

            info.MapFlags |= MapFlags.MeleeMap;
            info.MapFlags &= ~(MapFlags.UseCustomForces | MapFlags.ShowWaterWavesOnCliffShores | MapFlags.ShowWaterWavesOnRollingShores | MapFlags.MaskedAreasArePartiallyVisible);

            var players = new PlayerData[scenario.PlayerCount];
            for (var i = 0; i < scenario.PlayerCount; i++)
            {
                var player = PlayerData.Create(i);
                var playerData = scenario.PlayerData[i];

                // todo: unlock tech depending on starting age
                // info.SetTechData(i, playerData.Age);

                player.PlayerController = PlayerController.User;
                // player.PlayerController = playerData.IsHuman ? PlayerController.User : PlayerController.Computer;
                player.PlayerRace = PlayerRace.Human;
                player.IsRaceSelectable = false;
                player.FixedStartPosition = true;

                var sloc = scenario.Entities[i + 1].FirstOrDefault(entity => entity.Type == AoeEntityType.TownCenter);
                if (sloc is null)
                {
                    sloc = scenario.Entities[i + 1].First();
                }

                player.StartPosition = new System.Drawing.PointF(sloc.X * 128, sloc.Y * 128);

                players[i] = player;
            }

            info.SetPlayerData(players);

            var force = new ForceData();
            force.SetPlayers(players);

            return info;
        }
    }
}