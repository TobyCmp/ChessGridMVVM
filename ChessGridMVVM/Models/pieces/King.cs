using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGridMVVM.Models
{
    public class King : Piece
    {
        public override string Name => "King";

        public override string PieceSymbol => PieceColor == "White" ? "\u2654" : "\u265A";

        public override int Value => _value = 5;

        public bool validCastle = true;

        public King(string pieceColor) : base(pieceColor)
        {
            
        }

        public override bool isValidMove(Square startSquare, Square endSquare)
        {
            bool valid = false;
            var startRow = startSquare.Row;
            var startCol = startSquare.Column;
            var endRow = endSquare.Row;
            var endCol = endSquare.Column;
            int dx = endCol - startCol;
            int dy = endRow - startRow;

            if(Math.Abs(dx) < 2 && Math.Abs(dy) < 2)
            {
                valid = true;
            }

            if (endRow < 0 || endRow > 7 || endCol < 0 || endCol > 7 || startSquare == endSquare || endSquare.Piece != null && startSquare.Piece.PieceColor == endSquare.Piece.PieceColor && endSquare.Piece is not Rook)
            {
                valid = false;
            }

            //if (validCastle == true && endSquare.Piece.PieceColor == startSquare.Piece.PieceColor)
            //{
            //    if(endSquare.Piece is Rook)
            //    {
            //        if(endSquare.Piece.ValidCastle == true)
            //        {
            //            valid = true;
            //        }
            //    }
            //}


            return valid;
        }
        }
    }
