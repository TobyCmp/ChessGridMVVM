using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ChessGridMVVM.Models
{
    public class Game : INotifyPropertyChanged
    {
        public ChessBoardViewModel ChessBoardViewModel { get; set; }
        public Player WhitePlayer { get; set; }

        public Player BlackPlayer { get; set; }

        private Player _currentPlayer;
        private Player _opposingPlayer;
        private User _user1;
        private User _user2;

        public string moves;

        private Stack<ObservableCollection<ObservableCollection<Square>>> gameMoves;

        private string _gameState;
        public string GameState
        {
            get { return _gameState; }
            set
            {
                if(value != "Valid")
                {
                    if(value == "Checkmate")
                    {
                        endGame("Checkmate");
                    }
                    if(value == "Stalemate")
                    {
                        endGame("Stalemate");
                    }
                }
                else
                {
                    _gameState = "Valid";
                }
            }
        }
        public Player CurrentPlayer
        {
            get { return _currentPlayer; }
            set
            {
                _currentPlayer = value;
            }
        }

        public Player OpposingPlayer
        {
            get { return _opposingPlayer; }
            set
            {
                _opposingPlayer = value;
            }
        }


        public Game(User user1, User user2, bool showValidMoves, string filename, bool altColour)
        {
            ChessBoardViewModel = new ChessBoardViewModel(this, showValidMoves, filename, altColour);
            WhitePlayer = new Player("White");
            BlackPlayer = new Player("Black");
            CurrentPlayer = WhitePlayer;
            OpposingPlayer = BlackPlayer;
            GameState = "Valid";
            gameMoves = new Stack<ObservableCollection<ObservableCollection<Square>>>();
            addSnapshot();
            _user1 = user1;
            _user2 = user2;
        }

        public void endTurn()
        {
            updateGameState();
            if(GameState == "Valid")
            {
                nextTurn();
            }
        }

        // Stores current board file in a stack. Used for undo-move.
        private void addSnapshot()
        {
            gameMoves.Push(CloneBoard(ChessBoardViewModel.Board));
        }

        public void updateGameState()
        {
            string currentState = ChessBoardViewModel.getCurrentState(OpposingPlayer.Color);
            GameState = currentState;
        }

        public void undoTurn()
        {
            if (gameMoves.Count > 1)
            {
                gameMoves.Pop(); // Remove current state
                ChessBoardViewModel.Board = CloneBoard(gameMoves.Peek()); // Restore previous state

                // Swap players back
                CurrentPlayer = CurrentPlayer == WhitePlayer ? BlackPlayer : WhitePlayer;
                OpposingPlayer = CurrentPlayer == WhitePlayer ? BlackPlayer : WhitePlayer;

                ChessBoardViewModel.updateKingVariable(CurrentPlayer.Color);
            }
        }

        public void nextTurn()
        {
            OpposingPlayer = CurrentPlayer;
            CurrentPlayer = CurrentPlayer == WhitePlayer ? BlackPlayer : WhitePlayer;
            ChessBoardViewModel.updateKingVariable(CurrentPlayer.Color);
            addSnapshot();
        }

        private ObservableCollection<ObservableCollection<Square>> CloneBoard(ObservableCollection<ObservableCollection<Square>> board)
        {
            var newBoard = new ObservableCollection<ObservableCollection<Square>>();
            foreach (var row in board)
            {
                var newRow = new ObservableCollection<Square>();
                foreach (var square in row)
                {
                    // Create a new Square with the same color, row, and column
                    var newSquare = new Square(square.Color, square.Row, square.Column);

                    // Copy the piece reference if needed
                    newSquare.Piece = square.Piece; // Assuming Piece is a reference or needs copying separately

                    newRow.Add(newSquare);
                }
                newBoard.Add(newRow);
            }
            return newBoard;
        }

        public void endGame(string outcome)
        {
            if(outcome == "Stalemate")
            {
                GameEnd ge = new GameEnd(_user1, _user2, "1-1", moves);
                ge.Show();
            }
            else if(_currentPlayer == WhitePlayer)
            {

                GameEnd ge = new GameEnd(_user1, _user2, "1-0", moves);
                ge.Show();

            }
            else
            {
                GameEnd ge = new GameEnd(_user1, _user2, "0-1", moves);
                ge.Show();

            }
            
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
