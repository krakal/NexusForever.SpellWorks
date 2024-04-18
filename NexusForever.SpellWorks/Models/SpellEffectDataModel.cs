using NexusForever.SpellWorks.GameTable.Model;

namespace NexusForever.SpellWorks.Models
{
    public class SpellEffectDataModel : ISpellEffectDataModel
    {
        public virtual string Data00ColumnName => "Data00";
        public virtual string Data01ColumnName => "Data01";
        public virtual string Data02ColumnName => "Data02";
        public virtual string Data03ColumnName => "Data03";
        public virtual string Data04ColumnName => "Data04";
        public virtual string Data05ColumnName => "Data05";
        public virtual string Data06ColumnName => "Data06";
        public virtual string Data07ColumnName => "Data07";
        public virtual string Data08ColumnName => "Data08";
        public virtual string Data09ColumnName => "Data09";

        public virtual string Data00 => entry.DataBits00.ToString();
        public virtual string Data01 => entry.DataBits01.ToString();
        public virtual string Data02 => entry.DataBits02.ToString();
        public virtual string Data03 => entry.DataBits03.ToString();
        public virtual string Data04 => entry.DataBits04.ToString();
        public virtual string Data05 => entry.DataBits05.ToString();
        public virtual string Data06 => entry.DataBits06.ToString();
        public virtual string Data07 => entry.DataBits07.ToString();
        public virtual string Data08 => entry.DataBits08.ToString();
        public virtual string Data09 => entry.DataBits09.ToString();

        protected Spell4EffectsEntry entry;

        public void Initialise(Spell4EffectsEntry entry)
        {
            this.entry = entry;
        }
    }
}
