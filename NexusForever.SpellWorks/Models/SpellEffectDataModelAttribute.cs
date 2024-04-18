using NexusForever.SpellWorks.GameTable.Static;

namespace NexusForever.SpellWorks.Models
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SpellEffectDataModelAttribute : Attribute
    {
        public SpellEffectType Type { get; }

        public SpellEffectDataModelAttribute(SpellEffectType type)
        {
            Type = type;
        }
    }
}
