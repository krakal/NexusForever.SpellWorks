using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MahApps.Metro.Controls.Dialogs;
using NexusForever.SpellWorks.Services;

namespace NexusForever.SpellWorks.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        public string Title => "NexusForever SpellWorks";

        public ObservableCollection<BaseTabItem> Tabs { get; } = [];

        [ObservableProperty]
        private int _selectedTabIndex;



        public ICommand OnLoadCommand => _onLoadCommand ??= new AsyncRelayCommand(OnLoad);
        private ICommand _onLoadCommand;

        #region Dependency Injection

        private readonly IDialogCoordinator _dialogCoordinator;
        private readonly IResourceService _resourceService;

        public MainWindowViewModel(
            IDialogCoordinator dialogCoordinator,
            IResourceService resourceService,
            SpellsTabViewModel spellsTabViewModel,
            ProcsTabViewModel procsTabViewModel,
            SettingsTabViewModel settingsTabViewModel)
        {
            _dialogCoordinator = dialogCoordinator;
            _resourceService   = resourceService;

            Tabs.Add(spellsTabViewModel);
            Tabs.Add(procsTabViewModel);
            Tabs.Add(settingsTabViewModel);
        }

        #endregion

        public MainWindowViewModel()
        {
        }

        private async Task OnLoad()
        {
            ProgressDialogController controller = await _dialogCoordinator.ShowProgressAsync(this, "Loading Resources...", "");

            try
            {
                await _resourceService.Initialise(controller);
            }
            catch (Exception e)
            {
                await _dialogCoordinator.ShowMessageAsync(this, "Error", e.Message);
            }
            finally
            {
                await controller.CloseAsync();
            }
        }
    }
}
