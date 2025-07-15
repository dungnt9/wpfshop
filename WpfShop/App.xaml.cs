using System.Windows;
using Prism.Ioc;
using Prism.Modularity;
using Prism.DryIoc;
using WpfShop.Core.Services;
using WpfShop.Modules.MainAppModule;
using System.Net.Http;

namespace WpfShop
{
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register HttpClient
            containerRegistry.RegisterSingleton<HttpClient>();
            
            // Register Services
            containerRegistry.RegisterSingleton<IApiService, ApiService>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<MainAppModule>();
        }
    }
}