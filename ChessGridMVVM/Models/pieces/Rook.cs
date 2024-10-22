using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGridMVVM.Models
{
    public class Rook : Piece
    {
        public override string PieceSymbol => PieceColor == "White" ? "\u2656" : "\u265C";

        public override int Value => _value = 5;


        public Rook(string pieceColor) : base(pieceColor)
        {
            
        }

        public override bool isValidMove(Square startingSquare, Square endSquare)
        {
            var startRow = startingSquare.Row;
            var startCol = startingSquare.Column;
            var endRow = endSquare.Row;
            var endCol = endSquare.Column;

            if (endRow < 0 || endRow > 7 || endCol < 0 || endCol > 7 || startingSquare == endSquare)
                return false;

            

            if(startCol == endCol || startRow == endRow)
            {
                return true;
            }

            return false;
        }
        }
    }
