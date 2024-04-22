using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NexusForever.SpellWorks.Models;

namespace NexusForever.SpellWorks.ViewModels
{
    public partial class SpellInfoViewModel : ObservableObject
    {
        public ObservableCollection<BaseTabItem> Tabs { get; } = [];

        private SpellInfoSpellTabViewModel _spellInfoSpellTabViewModel;
        private SpellInfoEffectsTabViewModel _spellInfoEffectsTabViewModel;

        [ObservableProperty]
        private string _title;

        [ObservableProperty]
        private ISpellModel _selectedSpell;

        partial void OnSelectedSpellChanged(ISpellModel value)
        {
            _spellInfoSpellTabViewModel.SelectedSpell = value;
            _spellInfoEffectsTabViewModel.SelectedSpell = value;

            Title = $"{value?.Id} - {value?.Description}";
        }

        [ObservableProperty]
        private bool _isLocked;

        public ICommand BackButtonCommand => _backButtonCommand ??= new RelayCommand(OnBackButtonCommand);
        private ICommand _backButtonCommand;

        private void OnBackButtonCommand()
        {
            SelectedSpell = _history.Pop();
        }

        [ObservableProperty]
        private bool _backButtonIsEnabled;

        private Stack<ISpellModel> _history = [];



        #region Depedency Injection

        public SpellInfoViewModel(
            SpellInfoSpellTabViewModel spellInfoSpellTabViewModel,
            SpellInfoEffectsTabViewModel spellInfoEffectsTabViewModel)
        {
            _spellInfoSpellTabViewModel   = spellInfoSpellTabViewModel;
            _spellInfoEffectsTabViewModel = spellInfoEffectsTabViewModel;

            Tabs.Add(_spellInfoSpellTabViewModel);
            Tabs.Add(_spellInfoEffectsTabViewModel);
        }

        #endregion

        public SpellInfoViewModel()
        {
        }

        
    }
}
