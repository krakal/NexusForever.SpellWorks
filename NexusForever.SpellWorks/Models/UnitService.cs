namespace NexusForever.SpellWorks.Models
{
    public class UnitService : IUnitService
    {
        public uint Level
        {
            get => _level;
            set => _level = value;
        }
        public uint EffectiveLevel
        {
            get => _effectiveLevel;
            set => _effectiveLevel = value;
        }
        public uint Health
        {
            get => _health;
            set => _health = value;
        }
        public uint MaxHealth
        {
            get => _maxHealth;
            set => _maxHealth = value;
        }
        public uint MissingHealth
        {
            get => _missingHealth;
            set => _missingHealth = value;
        }
        public uint ShieldCapacity
        {
            get => _shieldCapacity;
            set => _shieldCapacity = value;
        }
        public uint MaxShieldCapacity
        {
            get => _maxShieldCapacity;
            set => _maxShieldCapacity = value;
        }
        public uint MissingShield
        {
            get => _missingShield;
            set => _missingShield = value;
        }
        public uint Brutality
        {
            get => _brutality;
            set => _brutality = value;
        }
        public uint Finesse
        {
            get => _finesse;
            set => _finesse = value;
        }
        public uint Tech
        {
            get => _tech;
            set => _tech = value;
        }
        public uint Moxie
        {
            get => _moxie;
            set => _moxie = value;
        }
        public uint Insight
        {
            get => _insight;
            set => _insight = value;
        }
        public uint Grit
        {
            get => _grit;
            set => _grit = value;
        }
        public uint AssaultPower
        {
            get => _assaultPower;
            set => _assaultPower = value;
        }
        public uint SupportPower
        {
            get => _supportPower;
            set => _supportPower = value;
        }

        private uint _level;
        private uint _effectiveLevel;
        private uint _health;
        private uint _maxHealth;
        private uint _missingHealth;
        private uint _shieldCapacity;
        private uint _maxShieldCapacity;
        private uint _missingShield;
        private uint _brutality;
        private uint _finesse;
        private uint _tech;
        private uint _moxie;
        private uint _insight;
        private uint _grit;
        private uint _assaultPower;
        private uint _supportPower;
    }
}
