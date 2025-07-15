using System.Collections.ObjectModel;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
using WpfShop.Core.Models;
using WpfShop.Core.Services;

namespace WpfShop.Modules.MainAppModule.ViewModels
{
    public class OrdersViewModel : BindableBase
    {
        private readonly IApiService _apiService;
        
        private ObservableCollection<Order> _orders;
        private Order _selectedOrder;
        private bool _isLoading;

        public OrdersViewModel(IApiService apiService)
        {
            _apiService = apiService;
            Orders = new ObservableCollection<Order>();
            
            LoadOrdersCommand = new DelegateCommand(async () => await LoadOrdersAsync());
            AddOrderCommand = new DelegateCommand(AddOrder);
            EditOrderCommand = new DelegateCommand<Order>(EditOrder);
            DeleteOrderCommand = new DelegateCommand<Order>(DeleteOrder);
            
            _ = LoadOrdersAsync();
        }

        public ObservableCollection<Order> Orders
        {
            get => _orders;
            set => SetProperty(ref _orders, value);
        }

        public Order SelectedOrder
        {
            get => _selectedOrder;
            set => SetProperty(ref _selectedOrder, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public ICommand LoadOrdersCommand { get; }
        public ICommand AddOrderCommand { get; }
        public ICommand EditOrderCommand { get; }
        public ICommand DeleteOrderCommand { get; }

        private async Task LoadOrdersAsync()
        {
            IsLoading = true;
            var orders = await _apiService.GetOrdersAsync();
            
            Orders.Clear();
            foreach (var order in orders)
            {
                Orders.Add(order);
            }
            IsLoading = false;
        }

        private void AddOrder()
        {
            // Simple implementation - in real app would show dialog
            var newOrder = new Order
            {
                CustomerName = "New Customer",
                CustomerEmail = "customer@email.com",
                ProductId = 1,
                Quantity = 1,
                OrderDate = DateTime.Now
            };
            Orders.Add(newOrder);
        }

        private void EditOrder(Order order)
        {
            if (order == null) return;
            // Simple implementation - in real app would show edit dialog
        }

        private void DeleteOrder(Order order)
        {
            if (order != null)
            {
                Orders.Remove(order);
            }
        }
    }
}