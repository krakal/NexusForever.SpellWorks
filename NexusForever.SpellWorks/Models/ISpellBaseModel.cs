using NexusForever.SpellWorks.GameTable.Model;

namespace NexusForever.SpellWorks.Models
{
    public interface ISpellBaseModel
    {
        Spell4BaseEntry Entry { get; }
        string Name { get; }

        void Initialise(Spell4BaseEntry entry);
    }
}