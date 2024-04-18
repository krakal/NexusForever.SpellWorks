using NexusForever.SpellWorks.Models;

namespace NexusForever.SpellWorks.Services
{
    public interface ISpellModelFilterService
    {
        IEnumerable<ISpellModel> Filter(IEnumerable<ISpellModelFilter> filters, IEnumerable<ISpellModel> models);
    }
}