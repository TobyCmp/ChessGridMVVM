﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGridMVVM.Models
{
    public class Queen : Piece
    {
        public override string PieceSymbol => PieceColor == "White" ? "\u2656" : "\u265C";

        public override int Value => _value = 5;


        public Queen(string pieceColor) : base(pieceColor)
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
            int dy = endRow = startRow;

            if (dx == dy)
            {
                return true;
            }

            return false;
        }
    }
}
