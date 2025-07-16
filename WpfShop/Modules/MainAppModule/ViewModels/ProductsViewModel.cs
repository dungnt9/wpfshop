using System.Collections.ObjectModel;
using System.Windows.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WpfShop.Core.Models;
using WpfShop.Core.Services;
using WpfShop.Core;

namespace WpfShop.Modules.MainAppModule.ViewModels
{
    public class ProductsViewModel : INotifyPropertyChanged
    {
        private readonly IApiService _apiService;
        private ObservableCollection<Product> _products = new();
        private Product? _selectedProduct;
        private bool _isLoading;

        // Constructor chỉ có 1 parameter
        public ProductsViewModel(IApiService apiService)
        {
            _apiService = apiService;
            Products = new ObservableCollection<Product>();
            
            // Initialize commands
            LoadProductsCommand = new RelayCommand(async () => await LoadProductsAsync());
            AddProductCommand = new RelayCommand(AddProduct);
            EditProductCommand = new RelayCommand<Product>(EditProduct);
            DeleteProductCommand = new RelayCommand<Product>(DeleteProduct);
            
            // Load initial data
            _ = LoadProductsAsync();
        }

        public ObservableCollection<Product> Products
        {
            get => _products;
            set
            {
                _products = value;
                OnPropertyChanged();
            }
        }

        public Product? SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                OnPropertyChanged();
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoadProductsCommand { get; }
        public ICommand AddProductCommand { get; }
        public ICommand EditProductCommand { get; }
        public ICommand DeleteProductCommand { get; }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async Task LoadProductsAsync()
        {
            try
            {
                IsLoading = true;
                var products = await _apiService.GetProductsAsync();
                
                Products.Clear();
                foreach (var product in products)
                {
                    Products.Add(product);
                }
            }
            catch (Exception ex)
            {
                // Simple error handling - no dialog service
                Console.WriteLine($"Error loading products: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void AddProduct()
        {
            // Simple implementation - add sample product
            var newProduct = new Product
            {
                Name = "New Product",
                Brand = "Sample Brand",
                Price = 100,
                Description = "Sample Description",
                Stock = 10
            };
            Products.Add(newProduct);
        }

        private void EditProduct(Product? product)
        {
            if (product == null) return;
            // Simple implementation - in real app would show edit dialog
            product.Name = product.Name + " (Edited)";
        }

        private void DeleteProduct(Product? product)
        {
            if (product != null)
            {
                Products.Remove(product);
            }
        }
    }
}