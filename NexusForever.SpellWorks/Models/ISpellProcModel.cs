using NexusForever.SpellWorks.GameTable.Model;
using NexusForever.SpellWorks.GameTable.Static;

namespace NexusForever.SpellWorks.Models
{
    public interface ISpellProcModel
    {
        ProcType ProcType { get; }
        uint SpellId { get; }

        void Initialise(Spell4EffectsEntry entry);
    }
}