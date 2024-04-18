using MahApps.Metro.Controls.Dialogs;

namespace NexusForever.SpellWorks.Services
{
    public class ResourceService : IResourceService
    {
        private readonly IArchiveService _archiveService;
        private readonly ITextTableService _textTableService;
        private readonly IGameTableService _gameTableService;
        private readonly ISpellModelService _spellModelService;

        public ResourceService(
            IArchiveService archiveService,
            ITextTableService textTableService,
            IGameTableService gameTableService,
            ISpellModelService spellModelService)
        {
            _archiveService    = archiveService;
            _textTableService  = textTableService;
            _gameTableService  = gameTableService;
            _spellModelService = spellModelService;
        }

        public async Task Initialise(ProgressDialogController controller)
        {
            await _archiveService.Initialise();
            await _textTableService.Initialise(controller);
            await _gameTableService.Initialise(controller);
            await _spellModelService.Initialise(controller);
        }
    }
}
