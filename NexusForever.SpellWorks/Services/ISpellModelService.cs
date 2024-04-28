using MahApps.Metro.Controls.Dialogs;
using NexusForever.SpellWorks.Models;

namespace NexusForever.SpellWorks.Services
{
    public interface ISpellModelService
    {
        Dictionary<uint, ISpellBaseModel> SpellBaseModels { get; }
        Dictionary<uint, ISpellModel> SpellModels { get; }
        Dictionary<uint, List<ISpellEffectModel>> SpellEffectModels { get; }
        Dictionary<uint, List<ISpellProcModel>> SpellProcModels { get; }
        Dictionary<uint, List<uint>> SpellProcReferences { get; }

        Task Initialise(ProgressDialogController controller);
    }
}