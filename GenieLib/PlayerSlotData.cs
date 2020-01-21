namespace GenieLib
{
    public sealed class PlayerSlotData
    {
        private readonly int _index;
        private readonly bool _enabled;
        private readonly bool _human;
        private readonly int _civilization;
        private readonly int _unk;

        private int _gold;
        private int _wood;
        private int _food;
        private int _stone;
        private int _age;

        public PlayerSlotData(int i, int enable, int human, int civ, int unk)
        {
            _index = i;
            _enabled = enable != 0;
            _human = human != 0;
            _civilization = civ;
            _unk = unk;
        }

        public int Index => _index;

        public bool Enabled => _enabled;

        public bool Human => _human;

        public int Civilization => _civilization;

        public int Gold => _gold;

        public int Wood => _wood;

        public int Food => _food;

        public int Stone => _stone;

        public int Age => _age;

        public void SetStartingResources(int gold, int wood, int food, int stone)
        {
            _gold = gold;
            _wood = wood;
            _food = food;
            _stone = stone;
        }

        public void SetStartingAge(int age)
        {
            _age = age;
        }
    }
}