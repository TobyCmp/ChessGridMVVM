using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGridMVVM.Models
{
    public class Player
    {
        private List<Square> _pieces;
        private string _color;


        public List<Square> Pieces
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
            _pieces = new List<Square>();
        }

        private void getPieces()
        {

        }

        public string OppositeColor()
        {
            var color = _color == "White" ? "Black" : "White";
            return color;
        }
    }
}
