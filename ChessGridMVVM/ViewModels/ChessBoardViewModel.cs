using ChessGridMVVM.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

public class ChessBoardViewModel : INotifyPropertyChanged
{
    private const int Size = 8;  // Size of the chessboard
    private ObservableCollection<ObservableCollection<Square>> _board;
    private Square _selectedSquare;  // Track the selected square
    private Square _lastMovedSquare; // Track the last moved square
    private string _currentPlayer;  // Track the current player

    public ObservableCollection<ObservableCollection<Square>> Board
    {
        get => _board;
        set
        {
            if (_board != value)
            {
                _board = value;
                OnPropertyChanged();
            }
        }
    }

    //public string CurrentPlayer
    //{
    //    get => _currentPlayer;
    //    private set
    //    {
    //        if (_currentPlayer != value)
    //        {
    //            _currentPlayer = value;
    //            OnPropertyChanged();
    //        }
    //    }
    //}

    public ChessBoardViewModel()
    {
        InitializeBoard();
        IntializePieces();
        //CurrentPlayer = "White"; // White starts the game
    }

    private void InitializeBoard()
    {
        Board = new ObservableCollection<ObservableCollection<Square>>();

        for (int j = 0; j < Size; j++)
        {
            var row = new ObservableCollection<Square>();

            for (int i = 0; i < Size; i++)
            {
                var color = (i + j) % 2 == 0 ? "White" : "Black";
                row.Add(new Square(color, j, i));
            }
            Board.Add(row);
        }
    }

    private void IntializePieces()
    {
        for(int j = 0; j <= Size; j++)
        {
            for(int i = 0; i < Size; i++)
            {
                if(j == 1)
                {
                    Board[j][i].Piece = new Pawn("White");
                }

                if (j == 6)
                {
                    Board[j][i].Piece = new Pawn("Black");
                }
            }
        }
    }
    //private void InitializeBoard()
    //{
    //    Board = new ObservableCollection<ObservableCollection<Square>>();

    //    var whitePawnSymbol = "\u2659";  // Unicode for white pawn
    //    var blackPawnSymbol = "\u265F";  // Unicode for black pawn

    //    for (int i = 0; i < Size; i++)
    //    {
    //        var row = new ObservableCollection<Square>();

    //        for (int j = 0; j < Size; j++)
    //        {
    //            var color = (i + j) % 2 == 0 ? "White" : "Black";
    //            string pieceSymbol = null;

    //            if (i == 1)
    //            {
    //                pieceSymbol = whitePawnSymbol;
    //            }
    //            else if (i == 6)
    //            {
    //                pieceSymbol = blackPawnSymbol;
    //            }

    //            row.Add(new Square(color, pieceSymbol, i, j));
    //        }

    //        Board.Add(row);
    //    }
    //}


    //public void SelectSquare(int row, int col)
    //{
    //    if (row >= 0 && row < Size && col >= 0 && col < Size)
    //    {
    //        var selectedSquare = Board[row][col];

    //        if (_selectedSquare != null)
    //        {
    //            var currentRow = _selectedSquare.Row;
    //            var currentCol = _selectedSquare.Column;

    //            if (IsValidMove(currentRow, currentCol, row, col) &&
    //                (selectedSquare.PieceSymbol == null || selectedSquare.PieceSymbol != GetCurrentPlayerPawnSymbol()))
    //            {
    //                MovePawn(currentRow, currentCol, row, col);
    //                _selectedSquare = null;
    //                ToggleTurn(); // Switch turns after a valid move
    //            }
    //            else
    //            {
    //                _selectedSquare = Board[row][col];
    //            }
    //        }
    //        else
    //        {
    //            // Select square only if it contains a piece belonging to the current player
    //            if (selectedSquare.PieceSymbol != null && selectedSquare.PieceSymbol == GetCurrentPlayerPawnSymbol())
    //            {
    //                _selectedSquare = selectedSquare;
    //            }
    //        }
    //    }
    //}


    //private string GetCurrentPlayerPawnSymbol()
    //{
    //    return CurrentPlayer == "White" ? "\u2659" : "\u265F";
    //}


    //private bool IsValidMove(int startRow, int startCol, int endRow, int endCol)
    //{
    //    var startPieceSymbol = Board[startRow][startCol].PieceSymbol;
    //    var endPieceSymbol = Board[endRow][endCol].PieceSymbol;

    //    if (endRow < 0 || endRow >= Size || endCol < 0 || endCol >= Size)
    //        return false;

    //    // White pawn movement
    //    if (startPieceSymbol == "\u2659")
    //    {
    //        if (endRow == startRow + 1 && startCol == endCol && endPieceSymbol == null)
    //        {
    //            return true;
    //        }
    //        else if (endRow == startRow + 1 && (endCol == startCol + 1 || endCol == startCol - 1) && endPieceSymbol != null)
    //        {
    //            return true;
    //        }
    //    }
    //    // Black pawn movement
    //    else if (startPieceSymbol == "\u265F")
    //    {
    //        if (endRow == startRow - 1 && startCol == endCol && endPieceSymbol == null)
    //        {
    //            return true;
    //        }
    //        else if (endRow == startRow - 1 && (endCol == startCol + 1 || endCol == startCol - 1) && endPieceSymbol != null)
    //        {
    //            return true;
    //        }
    //    }

    //    return false;
    //}

    //private void MovePawn(int startRow, int startCol, int endRow, int endCol)
    //{
    //    var startSquare = Board[startRow][startCol];
    //    var endSquare = Board[endRow][endCol];

    //    if (endSquare.PieceSymbol == null || (endSquare.PieceSymbol != null && endSquare.PieceSymbol != startSquare.PieceSymbol))
    //    {
    //        endSquare.PieceSymbol = startSquare.PieceSymbol;
    //        startSquare.PieceSymbol = null;

    //        if (_lastMovedSquare != null)
    //        {
    //            _lastMovedSquare.RevertColor();
    //        }

    //        endSquare.SetColour("green");
    //        _lastMovedSquare = endSquare;
    //    }
    //}

    //public void NextTurn()
    //{
    //    if (_lastMovedSquare != null)
    //    {
    //        _lastMovedSquare.RevertColor();
    //        _lastMovedSquare = null;
    //    }
    //}

    //private void ToggleTurn()
    //{
    //    // Switch the current player
    //    CurrentPlayer = CurrentPlayer == "White" ? "Black" : "White";
    //}

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
