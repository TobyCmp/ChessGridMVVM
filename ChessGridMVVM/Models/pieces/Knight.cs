using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGridMVVM.Models
{
    public class Knight : Piece
    {
        public override string Name => "Knight";

        public override string PieceSymbol => PieceColor == "White" ? "\u2658" : "\u265E";

        public override int Value => _value = 5;


        public Knight(string pieceColor) : base(pieceColor)
        {
            
        }

        public override bool isValidMove(Square startSquare, Square endSquare)
        {
            var startRow = startSquare.Row;
            var startCol = startSquare.Column;
            var endRow = endSquare.Row;
            var endCol = endSquare.Column;

            if (endRow < 0 || endRow > 7 || endCol < 0 || endCol > 7 || startSquare == endSquare || endSquare.Piece != null && startSquare.Piece.PieceColor == endSquare.Piece.PieceColor)
                return false;

            int dx = endCol - startCol;
            int dy = endRow - startRow;

            if (dx == 1 && dy == 2 || dx == 1 && dy == -2 ||dx == 2 && dy == 1 || dx == 2 && dy == -1 || dx == -1 && dy == 2 || dx == -1 && dy == -2 || dx == -2 && dy == 1 || dx == -2 && dy == -1)
            {
                return true;
            }

            return false;
        }
    }
    }
