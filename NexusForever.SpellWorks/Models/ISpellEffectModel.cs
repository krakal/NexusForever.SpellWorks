using NexusForever.SpellWorks.GameTable.Model;
using NexusForever.SpellWorks.GameTable.Static;

namespace NexusForever.SpellWorks.Models
{
    public interface ISpellEffectModel
    {
        Spell4EffectsEntry Entry { get; }
        SpellEffectType Type { get; }
        ISpellEffectDataModel Data { get; }
        List<ISpellEffectDataModel> DataGrid { get; }

        void Initialise(Spell4EffectsEntry entry);
    }
}