using System.Windows.Controls;
using WpfShop.Modules.MainAppModule.ViewModels;
using WpfShop.Core.Services;
using System.Net.Http;

namespace WpfShop.Modules.MainAppModule.Views
{
    public partial class OrdersView : UserControl
    {
        public OrdersView()
        {
            InitializeComponent();
            
            // Tạo ViewModel trực tiếp
            var httpClient = new HttpClient();
            var apiService = new ApiService(httpClient);
            DataContext = new OrdersViewModel(apiService);
        }
    }
}