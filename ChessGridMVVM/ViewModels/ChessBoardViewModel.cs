using ChessGridMVVM.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System;

public class ChessBoardViewModel : INotifyPropertyChanged
{
    private Game _game;
    private const int Size = 8;  // Size of the chessboard
    private ObservableCollection<ObservableCollection<Square>> _board;
    private Square _selectedSquare;  // Track the selected square
    private Square _lastMovedSquare; // Track the last moved square
    private string _currentPlayer;  // Track the current player
    private string _primaryColor = "#769656";
    private string _secondaryColor = "#eeeed2";
    private string _highlightColor = "Red";
    private string _posssiblemoveColor = "Cyan";
    private List<Square> _possibleMoves;


    public List<Square> PossibleMoves
    {
        get => _possibleMoves;

        set
        {
            _possibleMoves = value;
        }

    }

    public Game Game
    {
        get => _game;
        set
        {
            if (_game != value)
            {
                _game = value;
            }
        }
    }
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

    public Square SelectedSquare
    {
        get => _selectedSquare;
        set
        {
            if(_selectedSquare != null)
            {
                removeValidMoves();

            }
            if(value != null)
            {
                _selectedSquare = value;
                DrawValidMoves(_selectedSquare);
            }
            _selectedSquare = value;
            OnPropertyChanged();
        }
    }

    public ChessBoardViewModel()
    {
        Game = new Game();
        InitializeBoard();
        IntializePieces();
    }

    private void InitializeBoard()
    {
        Board = new ObservableCollection<ObservableCollection<Square>>();

        for (int j = 7; j >= 0; j--)
        {
            var row = new ObservableCollection<Square>();

            for (int i = 0; i < Size; i++)
            {
                var color = (i + j) % 2 == 1 ? _primaryColor : _secondaryColor;
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
                    Board[7-j][i].Piece = new Pawn("White");
                }

                if (j == 6)
                {
                    Board[7-j][i].Piece = new Pawn("Black");
                }
            }
        }
        Board[0][0].Piece = new Rook("Black");
        Board[0][7].Piece = new Rook("Black");
        Board[7][0].Piece = new Rook("White");
        Board[7][7].Piece = new Rook("White");

    }


    public void SelectSquare(int row, int col)
    {
        if (row >= 0 && row < Size && col >= 0 && col < Size)
        {
            if(SelectedSquare == null && Board[7-row][col].Piece != null && Game.CurrentPlayer.Color == Board[7 - row][col].Piece.PieceColor) // Select first piece
            {
                SelectedSquare = Board[7 - row][col];
            }
            else if(SelectedSquare != null && Board[7 - row][col].Piece != null && Board[7 - row][col].Piece.PieceColor == SelectedSquare.Piece.PieceColor) // If second selected piece is one of own, update selected piece
            {
                SelectedSquare.RevertColor();
                SelectedSquare = Board[7 - row][col];
            }
            else if(SelectedSquare != null && SelectedSquare.Piece != null && isValidMove(SelectedSquare, Board[7 - row][col]) == true) // 
            {
                Move(SelectedSquare.Row, SelectedSquare.Column, row, col);
                SelectedSquare = null;
            }
        }
    }

    public bool isValidMove(Square start, Square end)
    {
        int dx = end.Column - start.Column;
        int dy = end.Row - start.Row;

        if(dx != dy && dx != 0 && dy != 0)
        {
            return false;
        }

        int yStep = dy == 0 ? 0 : dy / Math.Abs(dy);
        int xStep = dx == 0 ? 0 : dx / Math.Abs(dx);

        int currentRow = start.Row + yStep;
        int currentColumn = start.Column + xStep;
        while (currentRow != end.Row || currentColumn != end.Column)
        {
            if (Board[7 - currentRow][currentColumn].Piece != null)
            {
                return false;
            }
            currentRow = currentRow + yStep;
            currentColumn = currentColumn + xStep;
        }
        return start.Piece.isValidMove(start, end);
    }

    public void DrawValidMoves(Square selectedSquare)
    {
        PossibleMoves = new List<Square>();
        selectedSquare.SetColour(_highlightColor);
        for(int col = 0; col < Size; col++)
        {
            for(int row = 0; row< Size; row++)
            {
                if(isValidMove(selectedSquare, Board[7- row][col]) == true)
                {
                    Board[7 - row][col].SetColour(_posssiblemoveColor);
                    PossibleMoves.Add(Board[7 - row][col]);
                }
            }
        }
    }

    public void removeValidMoves()
    {
        foreach(Square s in PossibleMoves)
        {
            s.RevertColor();
        }
    }

    private void Move(int startRow, int startCol, int endRow, int endCol)
    {
        var startSquare = Board[7- startRow][startCol];
        var endSquare = Board[7- endRow][endCol];
        
        if(endSquare.Piece != null)
        {
            Capture(endSquare.Piece);
        }

        endSquare.Piece = startSquare.Piece;
        startSquare.Piece = null;

        startSquare.RevertColor();
        Game.nextTurn();
    }

    private void Capture(Piece captured)
    {
        
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
