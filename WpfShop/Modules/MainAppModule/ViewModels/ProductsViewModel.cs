using System.Collections.ObjectModel;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using WpfShop.Core.Models;
using WpfShop.Core.Services;

namespace WpfShop.Modules.MainAppModule.ViewModels
{
    public class ProductsViewModel : BindableBase
    {
        private readonly IApiService _apiService;
        private readonly IDialogService _dialogService;
        
        private ObservableCollection<Product> _products;
        private Product _selectedProduct;
        private bool _isLoading;

        public ProductsViewModel(IApiService apiService, IDialogService dialogService)
        {
            _apiService = apiService;
            _dialogService = dialogService;
            
            Products = new ObservableCollection<Product>();
            
            // Initialize commands
            LoadProductsCommand = new DelegateCommand(async () => await LoadProductsAsync());
            AddProductCommand = new DelegateCommand(AddProduct);
            EditProductCommand = new DelegateCommand<Product>(EditProduct);
            DeleteProductCommand = new DelegateCommand<Product>(DeleteProduct);
            
            // Load initial data
            _ = LoadProductsAsync();
        }

        public ObservableCollection<Product> Products
        {
            get => _products;
            set => SetProperty(ref _products, value);
        }

        public Product SelectedProduct
        {
            get => _selectedProduct;
            set => SetProperty(ref _selectedProduct, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public ICommand LoadProductsCommand { get; }
        public ICommand AddProductCommand { get; }
        public ICommand EditProductCommand { get; }
        public ICommand DeleteProductCommand { get; }

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
                // Show error dialog
                ShowErrorDialog($"Error loading products: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void AddProduct()
        {
            var parameters = new DialogParameters();
            parameters.Add("Title", "Add New Product");
            parameters.Add("Product", new Product());
            
            _dialogService.ShowDialog("ProductDialog", parameters, async result =>
            {
                if (result.Result == ButtonResult.OK)
                {
                    var product = result.Parameters.GetValue<Product>("Product");
                    await SaveProductAsync(product);
                }
            });
        }

        private void EditProduct(Product product)
        {
            if (product == null) return;
            
            var parameters = new DialogParameters();
            parameters.Add("Title", "Edit Product");
            parameters.Add("Product", CloneProduct(product));
            
            _dialogService.ShowDialog("ProductDialog", parameters, async result =>
            {
                if (result.Result == ButtonResult.OK)
                {
                    var editedProduct = result.Parameters.GetValue<Product>("Product");
                    await SaveProductAsync(editedProduct);
                }
            });
        }

        private async void DeleteProduct(Product product)
        {
            if (product == null) return;
            
            var parameters = new DialogParameters();
            parameters.Add("Title", "Confirm Delete");
            parameters.Add("Message", $"Are you sure you want to delete '{product.Name}'?");
            
            _dialogService.ShowDialog("ConfirmDialog", parameters, result =>
            {
                if (result.Result == ButtonResult.OK)
                {
                    // In a real app, you would call delete API
                    Products.Remove(product);
                }
            });
        }

        private async Task SaveProductAsync(Product product)
        {
            try
            {
                IsLoading = true;
                
                if (product.Id == 0)
                {
                    // Create new product
                    var newProduct = await _apiService.CreateProductAsync(product);
                    Products.Add(newProduct);
                }
                else
                {
                    // Update existing product (would need PUT endpoint)
                    var existingProduct = Products.FirstOrDefault(p => p.Id == product.Id);
                    if (existingProduct != null)
                    {
                        existingProduct.Name = product.Name;
                        existingProduct.Brand = product.Brand;
                        existingProduct.Price = product.Price;
                        existingProduct.Description = product.Description;
                        existingProduct.Stock = product.Stock;
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorDialog($"Error saving product: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private Product CloneProduct(Product product)
        {
            return new Product
            {
                Id = product.Id,
                Name = product.Name,
                Brand = product.Brand,
                Price = product.Price,
                Description = product.Description,
                Stock = product.Stock
            };
        }

        private void ShowErrorDialog(string message)
        {
            var parameters = new DialogParameters();
            parameters.Add("Title", "Error");
            parameters.Add("Message", message);
            
            _dialogService.ShowDialog("MessageDialog", parameters, null);
        }
    }
}