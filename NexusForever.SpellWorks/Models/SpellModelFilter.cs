namespace NexusForever.SpellWorks.Models
{
    public class SpellModelFilter : ISpellModelFilter
    {
        public string Description { get; set; }

        public bool Filter(ISpellModel model)
        {
            if (Description != null && !model.Description.Contains(Description, StringComparison.InvariantCultureIgnoreCase))
                return false;

            return true;
        }
    }
}
