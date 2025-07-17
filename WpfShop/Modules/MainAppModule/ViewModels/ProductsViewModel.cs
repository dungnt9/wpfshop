using System.Collections.ObjectModel;
using System.Windows.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WpfShop.Core.Models;
using WpfShop.Core.Services;
using WpfShop.Core;
using WpfShop.Views;

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
            var dialog = new ProductDialog("Add New Product");
            if (dialog.ShowDialog() == true && dialog.Result != null)
            {
                Products.Add(dialog.Result);
            }
        }

        private void EditProduct(Product? product)
        {
            if (product == null) return;
            
            var clonedProduct = new Product
            {
                Id = product.Id,
                Name = product.Name,
                Brand = product.Brand,
                Price = product.Price,
                Description = product.Description,
                Stock = product.Stock
            };
            
            var dialog = new ProductDialog("Edit Product", clonedProduct);
            if (dialog.ShowDialog() == true && dialog.Result != null)
            {
                product.Name = dialog.Result.Name;
                product.Brand = dialog.Result.Brand;
                product.Price = dialog.Result.Price;
                product.Description = dialog.Result.Description;
                product.Stock = dialog.Result.Stock;
            }
        }

        private void DeleteProduct(Product? product)
        {
            if (product == null) return;
            
            var dialog = new ConfirmDialog($"Are you sure you want to delete '{product.Name}'?");
            if (dialog.ShowDialog() == true)
            {
                Products.Remove(product);
            }
        }
    }
}