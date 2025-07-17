using System.Windows;
using WpfShop.Core.Models;

namespace WpfShop.Views
{
    public partial class ProductDialog : Window
    {
        public class ProductDialogViewModel
        {
            public string Title { get; set; } = "";
            public Product Product { get; set; } = new Product();
        }

        public Product? Result { get; private set; }

        public ProductDialog(string title, Product? product = null)
        {
            InitializeComponent();
            
            var viewModel = new ProductDialogViewModel
            {
                Title = title,
                Product = product ?? new Product()
            };
            
            DataContext = viewModel;
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is ProductDialogViewModel vm)
            {
                Result = vm.Product;
            }
            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}