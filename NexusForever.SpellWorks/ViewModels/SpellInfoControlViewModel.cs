using System.Windows.Controls;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NexusForever.SpellWorks.Models;

namespace NexusForever.SpellWorks.ViewModels
{
    public partial class SpellInfoControlViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _name;

        [ObservableProperty]
        private ISpellModel _selectedSpell;

        partial void OnSelectedSpellChanged(ISpellModel value)
        {
            Name = $"{value.Id} - {value.Description}";
        }

        public ICommand OnAutoGeneratingColoumnsCommand => _onAutoGeneratingColoumnsCommand ??= new RelayCommand<DataGridAutoGeneratingColumnEventArgs>(OnAutoGeneratingColoumns);
        private ICommand _onAutoGeneratingColoumnsCommand;

        private void OnAutoGeneratingColoumns(DataGridAutoGeneratingColumnEventArgs e)
        {
            // Do something
        }
    }
}
