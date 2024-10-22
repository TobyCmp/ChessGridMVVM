using System.Windows;
using System.Windows.Input;
using ChessGridMVVM.Models;
using ChessGridMVVM.ViewModels;

namespace ChessGridMVVM
{
    public partial class MainWindow : Window
    {
        private ChessBoardViewModel ViewModel => DataContext as ChessBoardViewModel;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ChessBoardViewModel();
        }

        private void Square_Click(object sender, MouseButtonEventArgs e)
        {
            var border = sender as FrameworkElement;
            var dataContext = border.DataContext as Square;
            if (dataContext != null)
            {
                ViewModel.SelectSquare(dataContext.Row, dataContext.Column);
            }
        }
    }
}
