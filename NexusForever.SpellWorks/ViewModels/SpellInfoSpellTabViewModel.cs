using CommunityToolkit.Mvvm.ComponentModel;
using NexusForever.SpellWorks.Models;
using NexusForever.SpellWorks.Views;
using System.Diagnostics;

namespace NexusForever.SpellWorks.ViewModels
{
    public partial class SpellInfoSpellTabViewModel : BaseTabItem
    {
        public override string Header => "Spell";

        [ObservableProperty]
        private ISpellModel _selectedSpell;

        [ObservableProperty]
        public UInt32 _level;
        partial void OnLevelChanged(UInt32 value)
        {
            Debug.WriteLine($"Level change to: {value}");

            if(SelectedSpell != null && SelectedSpell.Effects != null)
            {
                foreach (var effect in SelectedSpell.Effects)
                {
                    EffectiveLevel = effect.Entry.ParameterType00;
                }
            }
            
        }

        [ObservableProperty]
        public UInt32 _effectiveLevel;

        partial void OnEffectiveLevelChanged(UInt32 value)
        {
            Debug.WriteLine($"Effective Level change to: {value}");
        }


        [ObservableProperty]
        public UInt32 _assaultPower;
        partial void OnAssaultPowerChanged(UInt32 value)
        {
            Debug.WriteLine($"Assault power change to: {value}");
        }

        [ObservableProperty]
        public UInt32 _supportPower;
        partial void OnSupportPowerChanged(UInt32 value)
        {
            Debug.WriteLine($"Support power change to: {value}");
        }
    }
}
