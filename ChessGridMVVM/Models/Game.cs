using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGridMVVM.Models
{
    public class Game
    {
        public ChessBoardViewModel ChessBoardViewModel { get; set; }
        public Player WhitePlayer { get; set; }

        public Player BlackPlayer { get; set; }

        private Player _currentPlayer;
        private Player _opposingPlayer;


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


        public Game()
        {
            ChessBoardViewModel = new ChessBoardViewModel(this);
            WhitePlayer = new Player("White");
            BlackPlayer = new Player("Black");
            CurrentPlayer = WhitePlayer;
            OpposingPlayer = BlackPlayer;
            GameState = "Valid";
        }

        public void endTurn()
        {
            updateGameState();
            if(GameState == "Valid")
            {
                nextTurn();
            }
        }

        public void updateGameState()
        {
            string currentState = ChessBoardViewModel.getCurrentState(OpposingPlayer.Color);
            GameState = currentState;
        }

        public void nextTurn()
        {
            OpposingPlayer = CurrentPlayer;
            CurrentPlayer = CurrentPlayer == WhitePlayer ? BlackPlayer : WhitePlayer;
            ChessBoardViewModel.updateKingVariable(CurrentPlayer.Color);

        }

        public void endGame()
        {
            Entry e = new Entry();
            e.Show();
        }
    }
}
