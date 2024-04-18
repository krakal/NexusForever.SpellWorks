using MahApps.Metro.Controls.Dialogs;
using Microsoft.Extensions.DependencyInjection;
using NexusForever.SpellWorks.GameTable.Model;
using NexusForever.SpellWorks.Models;

namespace NexusForever.SpellWorks.Services
{
    public class SpellModelService : ISpellModelService
    {
        public Dictionary<uint, ISpellBaseModel> SpellBaseModels { get; } = [];
        public List<ISpellModel> SpellModels { get; } = [];
        public Dictionary<uint, List<ISpellEffectModel>> SpellEffectModels { get; } = [];

        #region Dependency Injection

        private readonly IGameTableService _gameTableService;
        private readonly IServiceProvider _serviceProvider;

        public SpellModelService(
            IGameTableService gameTableService,
            IServiceProvider serviceProvider)
        {
            _gameTableService = gameTableService;
            _serviceProvider = serviceProvider;
        }

        #endregion

        public Task Initialise(ProgressDialogController controller)
        {
            controller.SetIndeterminate();
            controller.SetMessage("Loading Spell Models...");

            InitialiseBaseSpells();
            InitialiseSpellEffectModels();
            InitialiseSpells();

            return Task.CompletedTask;
        }

        private void InitialiseBaseSpells()
        {
            foreach (Spell4BaseEntry item in _gameTableService.Spell4Base.Entries)
            {
                var model = _serviceProvider.GetService<ISpellBaseModel>();
                model.Initialise(item);
                SpellBaseModels.Add(model.Entry.Id, model);
            }
        }

        private void InitialiseSpells()
        {
            foreach (Spell4Entry item in _gameTableService.Spell4.Entries)
            {
                var model = _serviceProvider.GetService<ISpellModel>();
                model.Initialise(item);
                SpellModels.Add(model);
            }
        }

        private void InitialiseSpellEffectModels()
        {
            foreach (Spell4EffectsEntry item in _gameTableService.Spell4Effects.Entries)
            {
                var model = _serviceProvider.GetService<ISpellEffectModel>();
                model.Initialise(item);

                if (!SpellEffectModels.TryGetValue(model.Entry.SpellId, out List<ISpellEffectModel> list))
                {
                    list = new List<ISpellEffectModel>();
                    SpellEffectModels.Add(model.Entry.SpellId, list);
                }

                list.Add(model);
            }
        }
    }
}
