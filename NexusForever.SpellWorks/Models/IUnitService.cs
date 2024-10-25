using CommunityToolkit.Mvvm.ComponentModel;

namespace NexusForever.SpellWorks.Models
{
    public interface IUnitService
    {
        public uint  Level { get; set; }
        public uint  EffectiveLevel { get; set; }
        public uint  Health { get; set; }
        public uint  MaxHealth { get; set; }
        public uint  MissingHealth { get; set; }
        public uint  ShieldCapacity { get; set; }
        public uint  MaxShieldCapacity { get; set; }
        public uint  MissingShield { get; set; }
        public uint  Brutality { get; set; }
        public uint  Finesse { get; set; }
        public uint  Tech { get; set; }
        public uint  Moxie { get; set; }
        public uint  Insight { get; set; }
        public uint  Grit { get; set; }
        public uint  AssaultPower { get; set; }
        public uint  SupportPower { get; set; }
    }
}
