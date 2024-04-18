using System.Windows.Controls;
using NexusForever.SpellWorks.GameTable.Model;
using NexusForever.SpellWorks.GameTable.Static;

namespace NexusForever.SpellWorks.Models
{
    public class SpellEffectModel : ISpellEffectModel
    {
        public Spell4EffectsEntry Entry { get; private set; }
        public SpellEffectType Type => Entry.EffectType;
        public ISpellEffectDataModel Data { get; private set; }

        


        public List<ISpellEffectDataModel> DataGrid => [Data];

        public void Initialise(Spell4EffectsEntry entry)
        {
            Entry = entry;

            Data = new SpellEffectDataModel();
            Data.Initialise(entry);
        }

        private void OnAutoGeneratingColoumns(DataGridAutoGeneratingColumnEventArgs e)
        {
            // Do something
        }
    }
}
