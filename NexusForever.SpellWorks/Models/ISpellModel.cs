using NexusForever.SpellWorks.GameTable.Model;

namespace NexusForever.SpellWorks.Models
{
    public interface ISpellModel
    {
        Spell4Entry Entry { get; }
        uint Id { get; }
        string Description { get; }
        ISpellBaseModel SpellBaseModel { get; }
        List<ISpellEffectModel> Effects { get; }

        void Initialise(Spell4Entry entry);
    }
}