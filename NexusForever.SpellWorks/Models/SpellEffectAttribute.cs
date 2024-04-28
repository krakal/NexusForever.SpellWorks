using NexusForever.SpellWorks.GameTable.Static;

namespace NexusForever.SpellWorks.Models
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SpellEffectAttribute : Attribute
    {
        public SpellEffectType Type { get; }

        public SpellEffectAttribute(SpellEffectType type)
        {
            Type = type;
        }
    }
}
