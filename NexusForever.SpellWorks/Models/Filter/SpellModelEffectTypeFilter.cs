using NexusForever.SpellWorks.GameTable.Static;

namespace NexusForever.SpellWorks.Models.Filter
{
    public class SpellModelEffectTypeFilter : ISpellModelFilter
    {
        public SpellEffectType SpellEffectType { get; set; }

        public bool Filter(ISpellModel model)
        {
            return model.Effects.Any(effect => effect.Type == SpellEffectType);
        }
    }
}
