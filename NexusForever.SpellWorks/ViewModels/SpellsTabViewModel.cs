using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NexusForever.SpellWorks.Models;
using NexusForever.SpellWorks.Services;

namespace NexusForever.SpellWorks.ViewModels
{
    public partial class SpellsTabViewModel : BaseTabItem
    {
        public override string Header => "Spells";

        public SpellInfoControlViewModel SpellInfoControlLeft { get; }
        public SpellInfoControlViewModel SpellInfoControlRight { get; }

        public ObservableCollection<ISpellModel> Spells { get; } = [];

        [ObservableProperty]
        private ISpellModel _selectedSpell;

        partial void OnSelectedSpellChanged(ISpellModel value)
        {
            SpellInfoControlLeft.SelectedSpell = value;
        }

        public ICommand OnLoadCommand => _onLoadCommand ??= new AsyncRelayCommand(Initialise);
        private ICommand _onLoadCommand;



        [ObservableProperty]
        private string _searchDescription;

        partial void OnSearchDescriptionChanged(string value)
        {
            Spells.Clear();

            var filter = new SpellModelFilter
            {
                Description = value
            };

            foreach (var item in _spellModelFilterService.Filter([ filter ], _spellModelService.SpellModels))
            {
                Spells.Add(item);
            }
        }

        #region Dependency Injection

        private readonly ISpellModelService _spellModelService;
        private readonly ISpellModelFilterService _spellModelFilterService;

        public SpellsTabViewModel(
            ISpellModelService spellModelService,
            ISpellModelFilterService spellModelFilterService,
            SpellInfoControlViewModel mainSpellControl)
        {
            _spellModelService       = spellModelService;
            _spellModelFilterService = spellModelFilterService;

            SpellInfoControlLeft = mainSpellControl;
        }

        #endregion

        public SpellsTabViewModel()
        {
        }

        public Task Initialise()
        {
            foreach (var item in _spellModelService.SpellModels)
            {
                Spells.Add(item);
            }

            return Task.CompletedTask;
        }


    }
}
