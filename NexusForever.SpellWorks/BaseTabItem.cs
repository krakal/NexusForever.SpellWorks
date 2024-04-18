using CommunityToolkit.Mvvm.ComponentModel;

namespace NexusForever.SpellWorks
{
    public abstract partial class BaseTabItem : ObservableObject
    {
        public abstract string Header { get; }

        [ObservableProperty]
        private bool _isSelected;
    }
}
