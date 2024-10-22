using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGridMVVM.Models
{
    public class King : Piece
    {
        public override string PieceSymbol => PieceColor == "White" ? "\u2656" : "\u265C";

        public override int Value => _value = 5;


        public King(string pieceColor) : base(pieceColor)
        {
            
        }

        public override bool isValidMove(Square startingSquare, Square endSquare)
        {
            return false;
        }
        }
    }
