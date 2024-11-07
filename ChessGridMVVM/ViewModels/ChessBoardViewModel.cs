﻿using ChessGridMVVM.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System;
using System.Windows;

public class ChessBoardViewModel : INotifyPropertyChanged
{
    private Game _game;
    private const int Size = 8;  // Size of the chessboard
    private ObservableCollection<ObservableCollection<Square>> _board;
    private Square _selectedSquare;  // Track the selected square
    private readonly string _primaryColor = "#769656";
    private readonly string _secondaryColor = "#eeeed2";
    private readonly string _highlightColor = "Red";
    private readonly string _possiblemoveColor = "Cyan";

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
            if (_selectedSquare != null)
            {
                removeValidMoves(_selectedSquare);
            }
            if (value != null )
            {
                _selectedSquare = value;
                drawValidMoves(_selectedSquare);
            }
            _selectedSquare = value;
            OnPropertyChanged();
        }
    }

    public ChessBoardViewModel(Game game)
    {
        Game = game; // accepts game instance
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
        Board[7][2].Piece = new Bishop("White");
        Board[7][3].Piece = new Queen("White");
        Board[5][4].Piece = new King("White");
        Board[4][5].Piece = new Knight("White");

    }

    // Select a square
    public void SelectSquare(int row, int col)
    {
        if (row >= 0 && row < Size && col >= 0 && col < Size)
        {
            if(SelectedSquare == null && Board[7-row][col].Piece != null && Game.CurrentPlayer.Color == Board[7 - row][col].Piece.PieceColor) // Select first piece
            {
                SelectedSquare = Board[7 - row][col];
                SelectedSquare.SetColour(_highlightColor);
            }
            else if(SelectedSquare != null && Board[7 - row][col].Piece != null && Board[7 - row][col].Piece.PieceColor == SelectedSquare.Piece.PieceColor) // If second selected piece is one of own, update selected piece
            {
                SelectedSquare.RevertColor();
                SelectedSquare = Board[7 - row][col];
                SelectedSquare.SetColour(_highlightColor);

            }
            else if(SelectedSquare != null && SelectedSquare.Piece != null && isValidMove(SelectedSquare, Board[7 - row][col]) == true) // 
            {
                Square s = SelectedSquare;
                SelectedSquare = null;
                Move(s.Row, s.Column, row, col);
            }
        }
    }

    // Check if move from one cell to another is valid
    public bool isValidMove(Square start, Square end)
    {
        if (start.Piece == null)
        {
            return false;
        }
        int dx = end.Column - start.Column;
        int dy = end.Row - start.Row;
        int yStep = dy == 0 ? 0 : dy / Math.Abs(dy);
        int xStep = dx == 0 ? 0 : dx / Math.Abs(dx);
        int currentRow = start.Row + yStep;
        int currentColumn = start.Column + xStep;
        bool validmove = false;
        validmove = start.Piece.isValidMove(start, end);

        if(start.Piece.Name == "Knight")
        {
            return validmove;
        }

        if (Math.Abs(dx) != Math.Abs(dy) && dx != 0 && dy != 0) // Check that move is either straight or diagonal 
        {
            return false;
        }


        while (currentRow != end.Row || currentColumn != end.Column) // Check if there exists a piece in any square between start and end 
        {
            if (Board[7 - currentRow][currentColumn].Piece != null)
            {
                return false;
            }
            currentRow = currentRow + yStep;
            currentColumn = currentColumn + xStep;
        }



        if (start.Piece.Name == "King")
        {
            List<Square> threats = new List<Square>();
            threats = getThreats(end, start.Piece.PieceColor);
            if (threats.Count == 0)
            {
                return validmove;
            }

            return false;
        }

        return validmove;
    }


    // Highlight selected cell and highlight all valid moves from that cell
    public List<Square> getValidMoves(Square selectedSquare) 
    {
        List<Square> possMoves = new List<Square>();
        for(int col = 0; col < Size; col++) // For each xy coordinate, check if the piece has a valid move
        {
            for(int row = 0; row< Size; row++)
            {
                if(isValidMove(selectedSquare, Board[7- row][col]) == true)
                {
                    possMoves.Add(Board[7 - row][col]);
                }
            }
        }
        return possMoves;
    }

    public void drawValidMoves(Square s)
    {
        List<Square> l = getValidMoves(s);
        foreach (Square square in l)
        {
            square.SetColour(_possiblemoveColor);
        }
    }

    // Remove the highlighting of possible move cells
    public void removeValidMoves(Square s)
    {
        List<Square> l = getValidMoves(s);
        foreach (Square square in l)
        {
            square.RevertColor();
        }
    }

    // Move piece from one cell to another
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

        checkPromotion(endSquare);
        checkCheckmate();
        Game.nextTurn();
    }

    public void checkPromotion(Square endsquare)
    {
        if(endsquare.Row == 7 && endsquare.Piece.Name == "Pawn" || endsquare.Row == 0 && endsquare.Piece.Name == "Pawn")
        {
            endsquare.Piece = new Queen(endsquare.Piece.PieceColor);
        }
    }

    public void checkCheckmate()
    {
        Square s;
        List<Square> validmoves;
        List<Square> threats;
        for (int i = 0; i < Size; i++)
        {
            for (int j = 0; j < Size; j++)
            {
                s = Board[7 - j][i];
                if (s.Piece != null && s.Piece.Name == "King")
                {
                    if(s.Piece.PieceColor == "White")
                    {
                        validmoves = getValidMoves(s);
                        threats = getThreats(s, "White");
                        
                        if(validmoves.Count == 0 && threats.Count > 0)
                        {
                            Game.endGame();
                        }
                    }

                    if (s.Piece != null && s.Piece.PieceColor == "Black")
                    {
                        validmoves = getValidMoves(s);
                        threats = getThreats(s, "Black");
                        if (validmoves.Count == 0 && threats.Count > 0)
                        {
                            Game.endGame();
                        }

                    }
                }
            }
        }
    }

    private List<Square> getThreats(Square checkSquare, string color)
    {
        List<Square> threats = new List<Square>();
        Square s;
        for(int i = 0; i<Size; i++)
        {
            for(int j = 0; j < Size; j++)
            {
                s = Board[7 - j][i];
                if (s.Piece != null && s.Piece.PieceColor != color)
                {
                    if(s.Piece.isValidMove(s, checkSquare) == true && s.Piece.Name != "Pawn")
                    {

                        threats.Add(s);
                    }

                    if(s.Piece.Name == "Pawn") // check for pawn capture
                    {
                        if (s.Piece.PieceColor == "White" && checkSquare.Row == s.Row + 1 && (checkSquare.Column == s.Column + 1 || checkSquare.Column == s.Column - 1))
                        {
                            threats.Add(s);
                        }
                        else if (s.Piece.PieceColor == "Black" && checkSquare.Row == s.Row - 1 && (checkSquare.Column == s.Column + 1 || checkSquare.Column == s.Column - 1))
                        {
                            threats.Add(s);
                        }
                    }

                }

            }
        }

        return threats;
    }

    private void Capture(Piece p)
    {

    }
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
