using System.Windows;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NexusForever.SpellWorks.Configuration;
using NexusForever.SpellWorks.Models;
using NexusForever.SpellWorks.Services;
using NexusForever.SpellWorks.ViewModels;
using NexusForever.SpellWorks.Views.Windows;

namespace NexusForever.SpellWorks
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IConfiguration _configuration;
        private IServiceProvider _serviceProvider;

        public App()
        {
            var configuration = new ConfigurationBuilder();
            ConfigureConfiguration(configuration);
            _configuration = configuration.Build();

            var services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
        }

        private void ConfigureConfiguration(IConfigurationBuilder configuration)
        {
            configuration.AddJsonFile("Configuration.json");
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.Configure<SpelllWorksConfiguration>(_configuration);

            services.AddTransient<MainWindow>();
            services.AddTransient<MainWindowViewModel>();
            services.AddTransient<SpellInfoViewModel>();
            services.AddTransient<SpellInfoSpellTabViewModel>();
            services.AddTransient<SpellInfoEffectsTabViewModel>();
            services.AddTransient<SpellsTabViewModel>();
            services.AddTransient<ProcsTabViewModel>();
            services.AddTransient<SettingsTabViewModel>();

            services.AddSingleton<IDialogCoordinator, DialogCoordinator>();

            services.AddSingleton<IResourceService, ResourceService>();
            services.AddSingleton<IArchiveService, ArchiveService>();
            services.AddSingleton<ITextTableService, TextTableService>();
            services.AddSingleton<IGameTableService, GameTableService>();

            services.AddSingleton<ISpellModelFilterService, SpellModelFilterService>();
            services.AddSingleton<ISpellModelService, SpellModelService>();
            services.AddTransient<ISpellModel, SpellModel>();
            services.AddTransient<ISpellBaseModel, SpellBaseModel>();
            services.AddTransient<ISpellEffectModel, SpellEffectModel>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            MainWindow mainWindow = _serviceProvider.GetService<MainWindow>();
            mainWindow.DataContext = _serviceProvider.GetService<MainWindowViewModel>();
            mainWindow.Show();

            base.OnStartup(e);
        }
    }

}
