using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace ChessGridMVVM.Models
{
    public abstract class Piece : INotifyPropertyChanged
    {
        protected int _value; 
        protected string _pieceSymbol;
        private string _pieceColor;
        public abstract string Name { get; }

        public string PieceColor
        {
            get { return _pieceColor; }
            set { _pieceColor = value; }
        }



        public Piece(string pieceColor)
        {
            PieceColor = pieceColor;
        }

        public abstract string PieceSymbol // allows each piece to set its own pieceSymbol
        {
            get;
        }

        public abstract int Value
        {
            get;
        }

        public abstract bool isValidMove(Square startingSquare, Square endSquare);





        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
