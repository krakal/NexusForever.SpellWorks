using NexusForever.SpellWorks.GameTable.Static;

namespace NexusForever.SpellWorks.Models.Effect
{
    [SpellEffectDataModel(SpellEffectType.CCStateSet)]
    public class CCStateSpellEffectDataModel : SpellEffectDataModel
    {
        public override string Data00ColumnName => "CCState";
    }
}
