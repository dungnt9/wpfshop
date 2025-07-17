using System.Windows;

namespace WpfShop.Views
{
    public partial class ConfirmDialog : Window
    {
        public class ConfirmDialogViewModel
        {
            public string Message { get; set; } = "";
        }

        public ConfirmDialog(string message)
        {
            InitializeComponent();
            
            var viewModel = new ConfirmDialogViewModel
            {
                Message = message
            };
            
            DataContext = viewModel;
        }

        private void Yes_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void No_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}