using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Text;

namespace GenieLib
{
    public class Scenario
    {
        private const uint Sect = 0xFFFFFF9D;
        private const int PlayerSlotCount = 16;

        public readonly AoeVersion FileVersion;
        public readonly float SubVersion;
        public readonly int PlayerCount;
        public readonly string ScenarioName;

        public readonly PlayerSlotData[] PlayerData;
        public readonly ScenarioMap Map;
        public readonly Dictionary<int, ImmutableArray<Entity>> Entities;

        public Scenario(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("File not found.", path);
            }

            MemoryStream decompressedData;
            using (var fileStream = File.Open(path, FileMode.Open, FileAccess.Read))
            {
                using (var binaryReader = new BinaryReader(fileStream))
                {
                    var version = Encoding.ASCII.GetString(binaryReader.ReadBytes(4));
                    FileVersion = version switch
                    {
                        "1.10" => AoeVersion.AOE1,
                        "1.11" => AoeVersion.AOE1,
                        "1.18" => AoeVersion.AOE2,
                        "1.21" => AoeVersion.AOE2TC,
                        _ => throw new Exception("Unknown scenario version: " + version),
                    };

                    var headerLength = binaryReader.ReadInt32();
                    var checkSum = binaryReader.ReadInt32();
                    var timeStamp = binaryReader.ReadInt32();
                    var instructionsLength = binaryReader.ReadInt32();
                    var instructions = binaryReader.ReadBytes(instructionsLength);
                    binaryReader.ReadInt32();
                    PlayerCount = binaryReader.ReadInt32();

                    // var compressedlength = binaryReader.BaseStream.Length - (headerLength + 8);

                    decompressedData = Decompress(binaryReader);
                    decompressedData.Position = 0;
                }
            }

            using var reader = new BinaryReader(decompressedData);

            long next_uid = reader.ReadInt32();
            SubVersion = reader.ReadSingle();

            for (var i = 0; i < PlayerSlotCount; i++)
            {
                var name = reader.ReadBytes(256); // player name
            }

            if (FileVersion != AoeVersion.AOE1)
            {
                for (var i = 0; i < PlayerSlotCount; i++)
                    reader.ReadInt32(); // string table
            }

            PlayerData = new PlayerSlotData[PlayerSlotCount];
            for (var i = 0; i < PlayerSlotCount; i++)
            {
                PlayerData[i] = new PlayerSlotData(i, reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32());
            }

            reader.ReadInt32();
            reader.ReadByte();
            reader.ReadSingle();

            ScenarioName = Encoding.ASCII.GetString(reader.ReadBytes(reader.ReadUInt16()));

            var messageCount = (FileVersion != AoeVersion.AOE1 ? 6 : 5);
            long[] mstrings = new long[6];
            string[] messages = new string[messageCount];
            string[] cinem = new string[4];
            string[] unk = new string[32];
            string[] ai = new string[PlayerSlotCount];

            if (FileVersion != AoeVersion.AOE1)
            {
                for (int i = 0; i < 6; i++)
                    mstrings[i] = reader.ReadInt32();
            }

            for (var i = 0; i < messageCount; i++)
            {
                messages[i] = Encoding.ASCII.GetString(reader.ReadBytes(reader.ReadUInt16()));
            }

            for (var i = 0; i < 4; i++)
            {
                cinem[i] = Encoding.ASCII.GetString(reader.ReadBytes(reader.ReadUInt16()));
            }

            // bitmap
            var bitmap = reader.ReadInt32();
            var bitmapx = reader.ReadInt32();
            var bitmapy = reader.ReadInt32();
            var bitmapunk = reader.ReadInt16();
            if (bitmap != 0)
            {
                // read some big ass bitmap file if bitmap != 0
                throw new NotSupportedException();
            }

            for (var i = 0; i < 32; i++)
            {
                unk[i] = Encoding.ASCII.GetString(reader.ReadBytes(reader.ReadUInt16()));
            }

            for (var i = 0; i < PlayerSlotCount; i++)
            {
                ai[i] = Encoding.ASCII.GetString(reader.ReadBytes(reader.ReadUInt16()));
            }

            for (var i = 0; i < PlayerSlotCount; i++)
            {
                var part1len = reader.ReadInt32();
                var part2len = reader.ReadInt32();
                var part3len = reader.ReadUInt32();
                var totalLength = part1len + part2len + part3len;
                var text = Encoding.ASCII.GetString(reader.ReadBytes((int)totalLength));
            }

            if (FileVersion != AoeVersion.AOE1)
            {
                for (var i = 0; i < PlayerSlotCount; i++)
                    Console.WriteLine(i + ": aimode=" + reader.ReadByte());
            }

            if (reader.ReadUInt32() != Sect)
            {
                throw new InvalidDataException();
            }

            // Resources
            for (var i = 0; i < PlayerSlotCount; i++)
            {
                PlayerData[i].SetStartingResources(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32());

                if (FileVersion != AoeVersion.AOE1)
                {
                    Console.WriteLine("\tOrex: " + reader.ReadInt32());
                    Console.WriteLine("\tUnknown: " + reader.ReadInt32());
                }
            }

            if (reader.ReadUInt32() != Sect)
            {
                throw new InvalidDataException();
            }

            reader.ReadBytes(40); // victory stuff

            for (var i = 0; i < PlayerSlotCount; i++)
            {
                for (var j = 0; j < PlayerSlotCount; j++)
                {
                    var diplomacy = reader.ReadInt32(); // 0=ally?, 3=enemy?

                    /*Console.Write(diplomacy);
                    if (j != (PlayerSlotCount - 1))
                    {
                        Console.Write(" ");
                    }
                    else
                    {
                        Console.WriteLine();
                    }*/
                }
            }

            reader.ReadBytes(0x2D00); // idc

            if (reader.ReadUInt32() != Sect)
            {
                throw new InvalidDataException();
            }

            if (FileVersion == AoeVersion.AOE1)
            {
                for (var i = 0; i < PlayerSlotCount; i++)
                    for (var j = 0; j < 21; j++)
                        reader.ReadInt32();
            }
            else
            {
                for (var i = 0; i < PlayerSlotCount; i++)
                    reader.ReadInt32();
                for (var i = 0; i < PlayerSlotCount; i++)
                    reader.ReadInt32();
                for (var i = 0; i < PlayerSlotCount; i++)
                    for (var j = 0; j < 30; j++)
                        reader.ReadInt32();
                for (var i = 0; i < PlayerSlotCount; i++)
                    reader.ReadInt32();
                for (var i = 0; i < PlayerSlotCount; i++)
                    for (var j = 0; j < 30; j++)
                        reader.ReadInt32();
                for (var i = 0; i < PlayerSlotCount; i++)
                    reader.ReadInt32();
                for (var i = 0; i < PlayerSlotCount; i++)
                    for (var j = 0; j < 20; j++)
                        reader.ReadInt32();

                Console.WriteLine(reader.ReadInt32());
                Console.WriteLine(reader.ReadInt32());
            }

            var alltech = reader.ReadInt32() != 0;

            for (var i = 0; i < PlayerSlotCount; i++)
            {
                PlayerData[i].SetStartingAge(reader.ReadInt32());
            }

            var unk1 = reader.ReadUInt32();
            var unk2 = reader.ReadUInt32();

            if (reader.ReadUInt32() != Sect)
            {
                throw new InvalidDataException();
            }

            // Map
            Map = new ScenarioMap();
            Map.Read(reader, FileVersion);

            // Units
            // https://github.com/dderevjanik/agescx-js/blob/master/packages/io/Structures/UnitsStruct.ts
            var unitSections = reader.ReadInt32();
            if (unitSections != 9)
            {
                throw new Exception();
            }

            for (var i = 0; i < unitSections - 1; i++)
            {
                reader.ReadSingle();
                reader.ReadSingle();
                reader.ReadSingle();
                reader.ReadSingle();
            }

            Entities = new Dictionary<int, ImmutableArray<Entity>>();
            for (var i = 0; i < unitSections; i++)
            {
                var unitCount = reader.ReadUInt32();
                var units = new Entity[unitCount];
                for (var j = 0; j < unitCount; j++)
                {
                    // var x = reader.ReadSingle();
                    var y = reader.ReadSingle();
                    var x = reader.ReadSingle();
                    if (x < 0 || x > Map.width || y < 0 || y > Map.height)
                    {
                        throw new InvalidDataException();
                    }

                    var unitunk = reader.ReadSingle();
                    var identifier = reader.ReadUInt32();
                    var unittype = (AoeEntityType)reader.ReadUInt16();
                    var status = reader.ReadByte();
                    if (status != 2 && status != 7)
                    {
                        throw new InvalidDataException();
                    }

                    var rotation = reader.ReadSingle();
                    if (rotation < 0 || rotation > 23)
                    {
                        throw new InvalidDataException();
                    }

                    if (FileVersion != AoeVersion.AOE1)
                    {
                        var initialFrame = reader.ReadUInt16();
                        var garissonid = reader.ReadInt32();
                    }

                    units[j] = new Entity(unittype, x, y, rotation, status, identifier);
                }

                Entities.Add(i, units.ToImmutableArray());
            }

            // more data...
        }

        private static MemoryStream Decompress(BinaryReader reader)
        {
            var deflateStream = new Ionic.Zlib.DeflateStream(reader.BaseStream, Ionic.Zlib.CompressionMode.Decompress, true);
            var memoryStream = new MemoryStream();
            deflateStream.CopyTo(memoryStream);
            return memoryStream;
        }
    }
}