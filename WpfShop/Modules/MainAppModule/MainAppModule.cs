using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using WpfShop.Modules.MainAppModule.Views;
using WpfShop.Modules.MainAppModule.ViewModels;

namespace WpfShop.Modules.MainAppModule
{
    public class MainAppModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            
            regionManager.RegisterViewWithRegion("ProductsRegion", typeof(ProductsView));
            regionManager.RegisterViewWithRegion("OrdersRegion", typeof(OrdersView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<ProductsView>();
            containerRegistry.Register<ProductsViewModel>();
            containerRegistry.Register<OrdersView>();
            containerRegistry.Register<OrdersViewModel>();
        }
    }
}