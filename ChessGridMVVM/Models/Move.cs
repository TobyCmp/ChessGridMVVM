using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGridMVVM.Models
{
    public class Move
    {
        private Square From { get;}
        private Square To { get;}
        private bool isCapture { get; }
        private bool isPromotion { get; }

        public Piece Piece { get; }

        public Move(Square from, Square to)
        {
            From = from;
            To = to;
            Piece = from.Piece;
        }


    }
}
