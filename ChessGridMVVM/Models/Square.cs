using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace ChessGridMVVM.Models
{
    public class Square : INotifyPropertyChanged
    {
        private string _color;
        private Piece _piece;
        private string _originalColor;
        private readonly int _row;
        private readonly int _column;

        public int Row => _row;
        public int Column => _column;


        public Piece Piece
        {
            get { return _piece; }
            set 
            {
                _piece = value;
                OnPropertyChanged();
            }
        }

        public string Color
        {
            get => _color;
            set
            {
                if (_color != value)
                {
                    _color = value;
                    OnPropertyChanged();
                }
            }
        }


        public Square(string color, int row, int column)
        {
            _color = color;
            _originalColor = color;
            _row = row;
            _column = column;
        }

        public void SetColour(string colour)
        {
            Color = colour;
        }

        public void RevertColor()
        {
            Color = _originalColor;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
