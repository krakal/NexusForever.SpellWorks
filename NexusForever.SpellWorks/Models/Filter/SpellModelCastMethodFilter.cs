using NexusForever.SpellWorks.GameTable.Static;

namespace NexusForever.SpellWorks.Models.Filter
{
    public class SpellModelCastMethodFilter : ISpellModelFilter
    {
        public CastMethod CastMethod { get; set; }

        public bool Filter(ISpellModel model)
        {
            return model.SpellBaseModel.Entry.CastMethod == CastMethod;
        }
    }
}
