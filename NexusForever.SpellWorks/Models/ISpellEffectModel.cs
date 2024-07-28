using NexusForever.SpellWorks.GameTable.Model;
using NexusForever.SpellWorks.GameTable.Static;

namespace NexusForever.SpellWorks.Models
{
    public interface ISpellEffectModel
    {
        SpellEffectType Type { get; }
        uint DamageType { get; }
        uint DelayTime { get; }
        uint TickTime { get; }
        uint DurationTime { get; }
        uint Flags { get; }
        ISpellEffectColumnData ColumnData { get; }
        List<ISpellEffectRowData> RowData { get; }

        Spell4EffectsEntry Entry { get; }

        void Initialise(Spell4EffectsEntry entry);
    }
}
