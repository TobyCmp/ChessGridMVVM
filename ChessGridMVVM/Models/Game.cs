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


        private Stack<ChessBoardViewModel> gameMoves;

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
                        endGame();
                    }
                    if(value == "Stalemate")
                    {
                        endGame();
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


        public Game(User user1, User user2, bool showValidMoves)
        {
            ChessBoardViewModel = new ChessBoardViewModel(this, showValidMoves);
            WhitePlayer = new Player("White");
            BlackPlayer = new Player("Black");
            CurrentPlayer = WhitePlayer;
            OpposingPlayer = BlackPlayer;
            GameState = "Valid";
            gameMoves = new Stack<ChessBoardViewModel>();
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
            gameMoves.Push(ChessBoardViewModel);
        }

        public void updateGameState()
        {
            string currentState = ChessBoardViewModel.getCurrentState(OpposingPlayer.Color);
            GameState = currentState;
        }

        public void undoTurn()
        {
            var b = gameMoves.Pop();
            ChessBoardViewModel = b;
            OnPropertyChanged();
        }

        public void nextTurn()
        {
            OpposingPlayer = CurrentPlayer;
            CurrentPlayer = CurrentPlayer == WhitePlayer ? BlackPlayer : WhitePlayer;
            ChessBoardViewModel.updateKingVariable(CurrentPlayer.Color);
            addSnapshot();
        }

        public void endGame()
        {
            GameEnd ge = new GameEnd("s", _user1, _user2);
            ge.Show();
            
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
