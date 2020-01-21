namespace GenieLib
{
    public sealed class Entity
    {
        private readonly AoeEntityType _type;
        private readonly float _x;
        private readonly float _y;
        private readonly float _rotation;
        private readonly byte _status;
        private readonly uint _identifier;

        public Entity(AoeEntityType unitType, float x, float y, float rotation, byte status, uint identifier)
        {
            _type = unitType;
            _x = x;
            _y = y;
            _rotation = rotation;
            _status = status;
            _identifier = identifier;
        }

        public AoeEntityType Type => _type;

        public float X => _x;

        public float Y => _y;

        public float Rotation => _rotation;

        public byte Status => _status;

        public uint CreationNumber => _identifier;

        public override string ToString()
        {
            return $"{_type} ({_x}, {_y})";
        }
    }
}