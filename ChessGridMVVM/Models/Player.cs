using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGridMVVM.Models
{
    public class Player
    {
        private string _color;

        public string Color
        {
            get { return _color; }
            set { _color = value; }
        }

        public Player(string color)
        {
            _color = color;
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
