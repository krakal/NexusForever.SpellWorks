using NexusForever.SpellWorks.GameTable.Model;

namespace NexusForever.SpellWorks.Models
{
    public interface ISpellEffectDataModel
    {
        string Data00ColumnName { get; }
        string Data01ColumnName { get; }
        string Data02ColumnName { get; }
        string Data03ColumnName { get; }
        string Data04ColumnName { get; }
        string Data05ColumnName { get; }
        string Data06ColumnName { get; }
        string Data07ColumnName { get; }
        string Data08ColumnName { get; }
        string Data09ColumnName { get; }

        string Data00 { get; }
        string Data01 { get; }
        string Data02 { get; }
        string Data03 { get; }
        string Data04 { get; }
        string Data05 { get; }
        string Data06 { get; }
        string Data07 { get; }
        string Data08 { get; }
        string Data09 { get; }

        void Initialise(Spell4EffectsEntry entry);
    }
}