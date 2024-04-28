using NexusForever.SpellWorks.Models;

namespace NexusForever.SpellWorks.Services
{
    public interface ISpellTooltipParseService
    {
        string Parse(ISpellModel spell);
    }
}