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

        private string path = "C:\\Users\\K31644\\Source\\Repos\\ChessGridMVVM\\ChessGridMVVM\\Models\\gamestates";

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

        public void SaveBoardState(string filePath)
        {
            using StreamWriter writer = new StreamWriter(filePath);
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    writer.Write(ChessBoardViewModel.Board[i][j].Piece);
                    if (j < 7) writer.Write(","); // Separate with commas
                }
                writer.WriteLine();
            }
        }

        //public void LoadBoardState(string filePath)
        //{
        //    using (StreamReader reader = new StreamReader(filePath))
        //    {
        //        for (int i = 0; i < 8; i++)
        //        {
        //            string line = reader.ReadLine();
        //            if (line != null)
        //            {
        //                string[] pieces = line.Split(',');
        //                for (int j = 0; j < 8; j++)
        //                {
        //                    board[i, j] = pieces[j];
        //                }
        //            }
        //        }
        //    }
        //}


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
            SaveBoardState(path);
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

        }

        public void endGame()
        {
            System.Environment.Exit(1);
        }
    }
}
