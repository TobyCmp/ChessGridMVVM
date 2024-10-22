using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGridMVVM.Models
{
    public class Player
    {
        private List<Piece> _pieces;
        private string _color;


        public List<Piece> Pieces
        {
            get { return _pieces; }
            set { _pieces = value; }
        }

        public string Color
        {
            get { return _color; }
            set { _color = value; }
        }




        public Player(string color)
        {
            _color = color;
            _pieces = new List<Piece>();
        }

        public string OppositeColor()
        {
            var color = _color == "White" ? "Black" : "White";
            return color;
        }
    }
}
