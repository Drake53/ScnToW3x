using System;
using System.IO;

using War3Net.Build.Common;

namespace GenieLib
{
    public class ScenarioMap
    {
        public uint width, height;

        public struct Terrain
        {
            public AoeTerrainType cnst;
            public byte elev;

            public override string ToString()
            {
                return $"{cnst} {elev}";
            }
        };

        public Terrain[,] terrain;

        public ScenarioMap Expand(uint width, uint height, out RectangleMargins padding)
        {
            if (width < this.width)
            {
                throw new ArgumentOutOfRangeException(nameof(width));
            }

            if (height < this.height)
            {
                throw new ArgumentOutOfRangeException(nameof(height));
            }

            if (width == this.width && height == this.height)
            {
                throw new ArgumentException("Cannot expand when both width and height are same size as original.");
            }

            var map = new ScenarioMap();
            map.width = width;
            map.height = height;
            map.terrain = new Terrain[width, height];

            var padx = width - this.width;
            var pady = height - this.height;

            // padding priority goes to -x and +y due to camera angle
            var padright = padx / 2;
            var padbottom = pady / 2;
            var padleft = padx - padright;
            var padtop = pady - padbottom;

            padding = new RectangleMargins((int)padleft, (int)padright, (int)padbottom, (int)padtop);

            padright = width - padright;
            padtop = height - padtop;

            for (uint x = 0; x < width; x++)
            {
                for (uint y = 0; y < height; y++)
                {
                    if (x < padleft || x >= padright || y < padbottom || y >= padtop)
                    {
                        map.terrain[x, y].cnst = AoeTerrainType.Undefined;
                        map.terrain[x, y].elev = 1;
                    }
                    else
                    {
                        var originalTile = terrain[x - padleft, y - padbottom];
                        map.terrain[x, y].cnst = originalTile.cnst;
                        map.terrain[x, y].elev = originalTile.elev;
                    }
                }
            }

            return map;
        }

        public void Read(BinaryReader reader, AoeVersion version)
        {
            if (version >= AoeVersion.AOE2TC)
            {
                // aitype
                reader.ReadInt32();
            }

            width = reader.ReadUInt32();
            height = reader.ReadUInt32();
            terrain = new Terrain[width, height];

            for (uint x = 0; x < width; x++)
            {
                for (uint y = 0; y < height; y++)
                {
                    terrain[x, y].cnst = (AoeTerrainType)reader.ReadByte();
                    terrain[x, y].elev = reader.ReadByte();
                    var unk = reader.ReadByte(); // always 0?
                }
            }
        }
    }
}