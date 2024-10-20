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
        public string _pieceColor { get; set; }


        public Piece(string pieceColor)
        {
            _pieceColor = pieceColor;
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
