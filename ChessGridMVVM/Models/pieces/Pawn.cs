﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGridMVVM.Models
{
    public class Pawn : Piece
    {
        public override string Name => "Pawn";

        public override string PieceSymbol => PieceColor == "White" ? "\u2659" : "\u265F";

        public override int Value => _value = 1;

        public bool enPassant;
        public Pawn(string pieceColor) : base(pieceColor)
        {
            enPassant = false;
        }

        public override bool isValidMove(Square startingSquare, Square endSquare)
        {
            var startRow = startingSquare.Row;
            var startCol = startingSquare.Column;
            var endRow = endSquare.Row;
            var endCol = endSquare.Column;

            if (endRow < 0 || endRow > 7 || endCol < 0 || endCol > 7 || startingSquare == endSquare)
                return false;
            
            // White pawn movement
            if (PieceColor == "White")
            {
                if (endRow == startRow + 1 && startCol == endCol && endSquare.Piece == null) // +1 Vertically
                {
                    return true;
                }

                if (startRow == 1 && endRow == startRow + 2 && startCol == endCol && endSquare.Piece == null) // +2 Vertically if on starting square
                {
                    return true;
                }

                else if (endRow == startRow + 1 && (endCol == startCol + 1 || endCol == startCol - 1) && endSquare.Piece != null && endSquare.Piece.PieceColor == "Black") // take piece
                {
                    return true;
                }
            }

            // Black pawn movement
            else if (PieceColor == "Black")
            {
                if (endRow == startRow - 1 && startCol == endCol && endSquare.Piece == null) // +1 Vertically
                {
                    return true;
                } 
                if (startRow == 6 && endRow == startRow - 2 && startCol == endCol && endSquare.Piece == null) // +2 Vertically if on starting square
                {
                    return true;
                }
                else if (endRow == startRow - 1 && (endCol == startCol + 1 || endCol == startCol - 1) && endSquare.Piece != null && endSquare.Piece.PieceColor == "White") // take piece
                {
                    return true;
                }
            }


            return false;
        }
        }
    }
