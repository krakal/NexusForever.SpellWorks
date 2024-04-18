using MahApps.Metro.Controls.Dialogs;
using NexusForever.SpellWorks.Models;

namespace NexusForever.SpellWorks.Services
{
    public interface ISpellModelService
    {
        Dictionary<uint, ISpellBaseModel> SpellBaseModels { get; }
        List<ISpellModel> SpellModels { get; }
        Dictionary<uint, List<ISpellEffectModel>> SpellEffectModels { get; }

        Task Initialise(ProgressDialogController controller);
    }
}