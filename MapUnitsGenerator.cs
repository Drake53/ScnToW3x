using System.Collections.Generic;

using GenieLib;

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
            _unitTypes.Add(AoeEntityType.Villager, "hpea");
        }

        public static MapUnits Generate(Scenario scenario)
        {
            var units = new List<MapUnitData>();
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

                    if (TryConvertType(unitType, out var convertedType))
                    {
                        var unit = new MapUnitData(UnpackString(convertedType), entity.X * 128, entity.Y * 128, entity.Rotation * 16f, 1f, owner, (int)entity.CreationNumber);
                        units.Add(unit);
                    }
                }
            }

            return new MapUnits(units);
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