using NexusForever.SpellWorks.Models;

namespace NexusForever.SpellWorks.Services
{
    public interface ISpellTooltipParseService
    {
        string GetRawTooltip(ISpellModel spell);
        string Parse(ISpellModel spell);
    }
}