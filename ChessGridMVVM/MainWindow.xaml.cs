using System.Windows;
using System.Windows.Input;
using ChessGridMVVM.Models;
using ChessGridMVVM.ViewModels;

namespace ChessGridMVVM
{
    public partial class MainWindow : Window
    {
        private ChessBoardViewModel ViewModel => DataContext as ChessBoardViewModel;
        Game game;

        public MainWindow(User user1, User user2, bool showValidMoves, string filename)
        {

            //InitializeComponent();
            //DataContext = new ChessBoardViewModel();

            InitializeComponent();
            game = new Game(user1, user2, showValidMoves, filename); // create an instance of the game
            DataContext = game.ChessBoardViewModel;
            this.Focusable = true;
            this.Focus();
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

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.U)
            {
                game.undoTurn(); // Call your existing undo method
            }
        }

    }
}
