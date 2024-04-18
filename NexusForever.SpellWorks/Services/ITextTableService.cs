using MahApps.Metro.Controls.Dialogs;

namespace NexusForever.SpellWorks.Services
{
    public interface ITextTableService
    {
        Task Initialise(ProgressDialogController progressController);

        string GetText(uint id);
    }
}