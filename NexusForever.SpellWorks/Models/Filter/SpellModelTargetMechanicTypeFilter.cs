using NexusForever.SpellWorks.GameTable.Static;

namespace NexusForever.SpellWorks.Models.Filter
{
    public class SpellModelTargetMechanicTypeFilter : ISpellModelFilter
    {
        public SpellTargetMechanicType TargetMechanicType { get; set; }

        public bool Filter(ISpellModel model)
        {
            return model.SpellBaseModel?.TargetMechanics.TargetType == TargetMechanicType;
        }
    }
}
