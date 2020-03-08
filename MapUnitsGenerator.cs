using System.Collections.Generic;

using GenieLib;

using War3Net.Build.Info;
using War3Net.Build.Widget;

namespace ScenarioConverter
{
    public static class MapUnitsGenerator
    {
        private static readonly HashSet<AoeEntityType> _hostiles;
        private static readonly Dictionary<AoeEntityType, string> _unitTypes;

        static MapUnitsGenerator()
        {
            _hostiles = new HashSet<AoeEntityType>();
            _hostiles.Add(AoeEntityType.Lion);

            _unitTypes = new Dictionary<AoeEntityType, string>();
            _unitTypes.Add(AoeEntityType.Academy, "e000");
            _unitTypes.Add(AoeEntityType.Alligator, "e001");
            _unitTypes.Add(AoeEntityType.Bowman, "e004");
            _unitTypes.Add(AoeEntityType.ImprovedBowman, "e005");
            _unitTypes.Add(AoeEntityType.CompositeBowman, "e006");
            _unitTypes.Add(AoeEntityType.Discovery, "e010");
            _unitTypes.Add(AoeEntityType.Ballista, "e011");
            _unitTypes.Add(AoeEntityType.Barracks, "e012");
            _unitTypes.Add(AoeEntityType.FishingBoat, "e013");
            _unitTypes.Add(AoeEntityType.TradeBoat, "e015");
            _unitTypes.Add(AoeEntityType.MerchantShip, "e016");
            _unitTypes.Add(AoeEntityType.LightTransport, "e017");
            _unitTypes.Add(AoeEntityType.HeavyTransport, "e018");
            _unitTypes.Add(AoeEntityType.ScoutShip, "e019");
            _unitTypes.Add(AoeEntityType.WarGalley, "e020");
            _unitTypes.Add(AoeEntityType.Trireme, "e021");
            _unitTypes.Add(AoeEntityType.ElephantArcher, "e025");
            _unitTypes.Add(AoeEntityType.StoneThrower, "e035");
            _unitTypes.Add(AoeEntityType.Catapult, "e036");
            _unitTypes.Add(AoeEntityType.Cavalry, "e037");
            _unitTypes.Add(AoeEntityType.HeavyCavalry, "e038");
            _unitTypes.Add(AoeEntityType.HorseArcher, "e039");
            _unitTypes.Add(AoeEntityType.Chariot, "e040");
            _unitTypes.Add(AoeEntityType.ChariotArcher, "e041");
            _unitTypes.Add(AoeEntityType.Dock, "e045");
            _unitTypes.Add(AoeEntityType.WarElephant, "e046");
            _unitTypes.Add(AoeEntityType.Elephant, "e048");
            _unitTypes.Add(AoeEntityType.SiegeWorkshop, "e049");
            _unitTypes.Add(AoeEntityType.Farm, "e050");
            _unitTypes.Add(AoeEntityType.FishTuna, "e052");
            _unitTypes.Add(AoeEntityType.FishSalmon, "e053");
            _unitTypes.Add(AoeEntityType.BerryBush, "e059");
            _unitTypes.Add(AoeEntityType.LionTame, "e060");
            _unitTypes.Add(AoeEntityType.Gazelle, "e065");
            _unitTypes.Add(AoeEntityType.GoldMine, "e066");
            _unitTypes.Add(AoeEntityType.Granary, "e068");
            _unitTypes.Add(AoeEntityType.GuardTower, "e069");
            _unitTypes.Add(AoeEntityType.House, "e070");
            _unitTypes.Add(AoeEntityType.SmallWall, "e072");
            _unitTypes.Add(AoeEntityType.Clubman, "e073");
            _unitTypes.Add(AoeEntityType.Axeman, "e074");
            _unitTypes.Add(AoeEntityType.ShortSwordsman, "e075");
            _unitTypes.Add(AoeEntityType.BroadSwordsman, "e076");
            _unitTypes.Add(AoeEntityType.LongSwordsman, "e077");
            _unitTypes.Add(AoeEntityType.WatchTower, "e079");
            _unitTypes.Add(AoeEntityType.GovernmentCenter, "e082");
            _unitTypes.Add(AoeEntityType.Villager, "e083");
            _unitTypes.Add(AoeEntityType.Market, "e084");
            _unitTypes.Add(AoeEntityType.ArcheryRange, "e087");
            _unitTypes.Add(AoeEntityType.LionKing, "e089");
            _unitTypes.Add(AoeEntityType.ElephantKing, "e090");
            _unitTypes.Add(AoeEntityType.Hoplite, "e093");
            _unitTypes.Add(AoeEntityType.Phalanx, "e094");
            _unitTypes.Add(AoeEntityType.Eagle, "e096");
            _unitTypes.Add(AoeEntityType.Stable, "e101");
            _unitTypes.Add(AoeEntityType.StoneMine, "e102");
            _unitTypes.Add(AoeEntityType.StoragePit, "e103");
            _unitTypes.Add(AoeEntityType.Temple, "e104");
            _unitTypes.Add(AoeEntityType.WarChest, "e108");
            _unitTypes.Add(AoeEntityType.TownCenter, "e109");
            _unitTypes.Add(AoeEntityType.Flare, "e112");
            _unitTypes.Add(AoeEntityType.MediumWall, "e117");
            _unitTypes.Add(AoeEntityType.Priest, "e125");
            _unitTypes.Add(AoeEntityType.Lion, "e126");
            _unitTypes.Add(AoeEntityType.Fortification, "e155");
            _unitTypes.Add(AoeEntityType.Ruins, "e158");
            _unitTypes.Add(AoeEntityType.Artifact, "e159");
            _unitTypes.Add(AoeEntityType.DoublePoleFlag, "e162");
            _unitTypes.Add(AoeEntityType.SentryTower, "e199");
            _unitTypes.Add(AoeEntityType.BlindLamePriest, "e228");
            _unitTypes.Add(AoeEntityType.InvisibleDemon, "e229");
            _unitTypes.Add(AoeEntityType.Mercenary, "e240");
            _unitTypes.Add(AoeEntityType.CatapultTrireme, "e250");
            _unitTypes.Add(AoeEntityType.FishShore1, "e260");
            _unitTypes.Add(AoeEntityType.FishShore2, "e263");
            _unitTypes.Add(AoeEntityType.Wonder, "e276");
            _unitTypes.Add(AoeEntityType.Juggernaught, "e277");
            _unitTypes.Add(AoeEntityType.BallistaTower, "e278");
            _unitTypes.Add(AoeEntityType.Helepolis, "e279");
            _unitTypes.Add(AoeEntityType.HeavyCatapult, "e280");
            _unitTypes.Add(AoeEntityType.HeavyHorseArcher, "e281");
            _unitTypes.Add(AoeEntityType.Legion, "e282");
            _unitTypes.Add(AoeEntityType.Cataphract, "e283");
            _unitTypes.Add(AoeEntityType.Centurion, "e291");
            _unitTypes.Add(AoeEntityType.Raft, "e292");
            _unitTypes.Add(AoeEntityType.FlyingDutchman, "e297");
            _unitTypes.Add(AoeEntityType.Medusa, "e298");
            _unitTypes.Add(AoeEntityType.Scout, "e299");
            _unitTypes.Add(AoeEntityType.SinglePoleFlag, "e330");
            _unitTypes.Add(AoeEntityType.CamelRider, "e338");
            _unitTypes.Add(AoeEntityType.ScytheChariot, "e339");
            _unitTypes.Add(AoeEntityType.ArmoredElephant, "e345");
            _unitTypes.Add(AoeEntityType.Slinger, "e347");
            _unitTypes.Add(AoeEntityType.FireGalley, "e360");
            _unitTypes.Add(AoeEntityType.AlligatorKing, "e362");
            _unitTypes.Add(AoeEntityType.FishWhale, "e370");
            _unitTypes.Add(AoeEntityType.CleopatrasBarge, "e377");
            _unitTypes.Add(AoeEntityType.ZenobiasTower, "e380");
            _unitTypes.Add(AoeEntityType.MirrorTower, "e383");
            _unitTypes.Add(AoeEntityType.GazelleKing, "e384");
            _unitTypes.Add(AoeEntityType.BeachArticles, "e385");
        }

        public static MapUnits Generate(MapInfo mapInfo, Scenario scenario)
        {
            var units = new List<MapUnitData>();

            var playerCount = mapInfo.PlayerDataCount;
            for (var i = 0; i < playerCount; i++)
            {
                var data = mapInfo.GetPlayerData(i);
                var unit = new MapUnitData(UnpackString("sloc"), data.StartPosition.X, data.StartPosition.Y, 0f, 1f, data.PlayerNumber, i);
                units.Add(unit);
            }

            foreach (var pair in scenario.Entities)
            {
                foreach (var entity in pair.Value)
                {
                    var owner = pair.Key;
                    var unitType = entity.Type;
                    if (owner == 0)
                    {
                        owner = IsHostile(unitType) ? 26 : 27;
                    }
                    else
                    {
                        owner--;
                    }

                    if (TryConvertType(unitType, out var convertedType))
                    {
                        if (convertedType.Length != 4)
                        {
                            throw new System.Exception($"{unitType}");
                        }

                        var unit = new MapUnitData(UnpackString(convertedType), entity.X * 128, entity.Y * 128, entity.Rotation * 16f, 1f, owner, (int)entity.CreationNumber + playerCount);
                        units.Add(unit);
                    }
                    /*else if (!System.Enum.IsDefined(typeof(AoeEntityType), unitType))
                    {
                        throw new System.Exception($"{unitType}");
                    }*/
                }
            }

            return new MapUnits(units.ToArray());
        }

        private static bool IsHostile(AoeEntityType unitType)
        {
            return _hostiles.Contains(unitType);
        }

        private static bool TryConvertType(AoeEntityType unitType, out string convertedType)
        {
            return _unitTypes.TryGetValue(unitType, out convertedType);
        }

        private static char[] ConvertType(AoeEntityType unitType)
        {
            return UnpackString(_unitTypes[unitType]);
        }

        private static char[] UnpackString(string input)
        {
            var chars = new char[input.Length];
            for (var i = 0; i < input.Length; i++)
            {
                chars[i] = input[i];
            }

            return chars;
        }
    }
}