using System.Windows.Controls;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NexusForever.SpellWorks.Models;

namespace NexusForever.SpellWorks.ViewModels
{
    public partial class SpellInfoEffectsTabViewModel : BaseTabItem
    {
        public override string Header => "Effects";

        [ObservableProperty]
        private ISpellModel _selectedSpell;

        public ICommand Command => _onAutoGeneratingColumnsCommand ??= new RelayCommand<DataGridAutoGeneratingColumnEventArgs>(OnAutoGeneratingColoumns);
        private ICommand _onAutoGeneratingColumnsCommand;

        private void OnAutoGeneratingColoumns(DataGridAutoGeneratingColumnEventArgs e)
        {
            // Do something
        }
    }
}
