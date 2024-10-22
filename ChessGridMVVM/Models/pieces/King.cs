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


        public King(string pieceColor) : base(pieceColor)
        {
            
        }

        public override bool isValidMove(Square startSquare, Square endSquare)
        {
            
            var startRow = startSquare.Row;
            var startCol = startSquare.Column;
            var endRow = endSquare.Row;
            var endCol = endSquare.Column;
            int dx = endCol - startCol;
            int dy = endRow - startRow;

            if (endRow < 0 || endRow > 7 || endCol < 0 || endCol > 7 || startSquare == endSquare || endSquare.Piece != null && startSquare.Piece.PieceColor == endSquare.Piece.PieceColor)
                return false;

            if(Math.Abs(dx) < 2 && Math.Abs(dy) < 2)
            {
                return true;
            }

            return false;
        }
        }
    }
