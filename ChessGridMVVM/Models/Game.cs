using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGridMVVM.Models
{
    public class Game
    {
        public ChessBoardViewModel ChessBoardViewModel { get; set;}
        public Player WhitePlayer { get; set; }

        public Player BlackPlayer { get; set; }

        private Player _currentPlayer;

        public Player CurrentPlayer
        {
            get { return _currentPlayer; }
            set 
            {
                _currentPlayer = value;
            }
        }

        public Game()
        {
            ChessBoardViewModel = new ChessBoardViewModel(this);
            WhitePlayer = new Player("White");
            BlackPlayer = new Player("Black");
            CurrentPlayer = WhitePlayer;
        }


        public void nextTurn()
        {
           CurrentPlayer = CurrentPlayer == WhitePlayer ? BlackPlayer : WhitePlayer;

        }
    }
}
