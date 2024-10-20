using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGridMVVM.Models
{
    public class Pawn : Piece
    {
        public override string PieceSymbol => _pieceColor == "White" ? "\u2659" : "\u265F";

        public Pawn(string pieceColor) : base(pieceColor)
        {
            
        }

    }
}
