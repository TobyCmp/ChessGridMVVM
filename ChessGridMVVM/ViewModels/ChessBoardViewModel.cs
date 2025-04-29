using ChessGridMVVM.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System;
using System.Windows;
using System.Collections;
using System.IO;
using System.Media;

public class ChessBoardViewModel : INotifyPropertyChanged
{
    private Game _game;
    private const int Size = 8;  // Size of the chessboard
    private ObservableCollection<ObservableCollection<Square>> _board; // array of rows, with an array of columns foreach
    private Square _selectedSquare;  // Track the selected square
    private readonly string _primaryColor = "#769656";
    private readonly string _secondaryColor = "#eeeed2";
    private readonly string _highlightColor = "Red";
    private readonly string _possiblemoveColor = "Cyan";
    private Square _whiteKing; // Square containing white king
    private Square _blackKing; // Square containing black king
    private Square threat; // Find piece that is causing check aka 'threatening' the king
    private bool _showValidMoves; // Toggleable button to display valid moves with highlighted squares

    private bool _isCurrentPlayerChecked;

    public bool IsCurrentPlayerChecked // Shows if current player is checked - used for move validation
    {
        get { return _isCurrentPlayerChecked; }
        set 
        {
            _isCurrentPlayerChecked = value;
            if (_isCurrentPlayerChecked == true)
            {
                if (Game.CurrentPlayer.Color == "Black")
                {
                    BlackKing.Color = "Brown";
                }

                if (Game.CurrentPlayer.Color == "White")
                {
                    WhiteKing.Color = "Brown";
                }
            }
            else
            {
                WhiteKing.RevertColor();
                BlackKing.RevertColor();
            }
        }
    }


    public Square WhiteKing
    {
        get => _whiteKing;
        set
        {
            _whiteKing = value;
        }
    }

    public Square BlackKing
    {
        get => _blackKing;
        set
        {
            _blackKing = value;
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

    public Square SelectedSquare // Selecting square handling, highlights possible moves if squares if _showValidMoves is true
    {
        get => _selectedSquare;
        set
        { 
            if(_showValidMoves == true)
            {
                if (_selectedSquare != null)
                {
                    removeValidMoves(_selectedSquare);
                }
                if (value != null)
                {
                    _selectedSquare = value;
                    drawValidMoves(_selectedSquare);
                }
                _selectedSquare = value;
                OnPropertyChanged();
            }
            if (value != null )
            {
                _selectedSquare = value;
            }
            _selectedSquare = value;
            OnPropertyChanged();
        }
    }

    public ChessBoardViewModel(Game game, bool showValidMoves, string filename, bool altColour)
    {
        Game = game; // accepts game instance
        if (altColour)
        {
                _primaryColor = "#E3BEA5";
                _secondaryColor = "#F5F5DC";
        }
        InitializeBoard(); // creates board
        IntializePieces(filename); // adds pieces to board
        IsCurrentPlayerChecked = false;
        _showValidMoves = showValidMoves;
    }

    private void InitializeBoard() // Sets up the chessboard as an 8x8 grid, alternating colors for squares.
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

    private void IntializePieces(string filename) //  Places the pieces on the board in their predefined positions
    {
        string filepath = "C:\\Users\\Toby\\Source\\Repos\\ChessGrid\\ChessGridMVVM\\" + filename + ".txt";
        using (StreamReader readtext = new StreamReader(filepath))
        {
            bool done = false;
            string readText;
            string[] tokens;
            int x;
            int y;
            string colour;
            string piece;

            while (done == false)
            {
                readText = readtext.ReadLine();
                if (readText == "-")
                {
                    done = true;
                }
                else
                {
                    tokens = readText.Split(',');
                    x = Convert.ToInt32(tokens[0]);
                    y = Convert.ToInt32(tokens[1]);
                    colour = tokens[2];
                    piece = tokens[3];
                    if (piece == "Rook")
                    {
                        Board[x][y].Piece = new Rook(colour);
                    }
                    if (piece == "Pawn")
                    {
                        Board[x][y].Piece = new Pawn(colour);
                    }
                    if (piece == "Bishop")
                    {
                        Board[x][y].Piece = new Bishop(colour);
                    }
                    if (piece == "Knight")
                    {
                        Board[x][y].Piece = new Knight(colour);
                    }
                    if (piece == "Queen")
                    {
                        Board[x][y].Piece = new Queen(colour);
                    }
                    if (piece == "King")
                    {
                        if (colour == "White")
                        {
                            WhiteKing = Board[x][y];
                        }
                        else
                        {
                            BlackKing = Board[x][y];
                        }
                        Board[x][y].Piece = new King(colour);
                    }
                }

            }
        }
    }

    // Handles the selection of a square and manages the logic for selecting pieces and moving them.
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
                string color = s.Piece.PieceColor;
                Piece p = Board[7 - row][col].Piece;

                if (SelectedSquare.Piece is King)
                {
                    if (isThreatened(Board[7 - row][col], color) == false)
                    {
                        SelectedSquare = null;
                        Move(s.Row, s.Column, row, col);
                    }
                }

                else
                {
                    SelectedSquare = null;
                    Move(s.Row, s.Column, row, col);
                }
            }
        }
    }

    // Determines if a move from one square to another is valid according to chess rules.
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

        if(validmove == false)
        {
            return false;
        }

        if (IsCurrentPlayerChecked)
        {
            if (start.Piece is not King)
            {
                List<Square> blockSquares = blockingSquares();
                if (blockSquares.Contains(end)!= true)
                {
                    return false;
                }
            }
        }

        if (start.Piece.Name == "Knight")
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



        return validmove;
    }




    //  Returns a list of valid moves for a selected piece.
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

        if(selectedSquare.Piece is King && possMoves.Count>0)
        {
            List<Square> templist = new List<Square>();
            string color = selectedSquare.Piece.PieceColor;
            selectedSquare.Piece = null;
            for (int i = 0; i < possMoves.Count; i++)
            {
                if (isThreatened(possMoves[i], color) == false)
                {
                    templist.Add(possMoves[i]);
                }
            }
            selectedSquare.Piece = new King(color);

            return templist;
        }

        return possMoves;
    }

    // Highlight valid moves on the board visually.
    public void drawValidMoves(Square s)
    {
        List<Square> l = getValidMoves(s);
        foreach (Square square in l)
        {
            if(square.Piece != null)
            {
                square.SetColour("Yellow");

            }
            else
            {
                square.SetColour(_possiblemoveColor);

            }
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

    SoundPlayer move_sound = new SoundPlayer(@"C:\Users\Toby\source\repos\ChessGridMVVM\chessmvm\ChessGridMVVM\move-self.wav");
    SoundPlayer capture_sound = new SoundPlayer(@"C:\Users\Toby\source\repos\ChessGridMVVM\chessmvm\ChessGridMVVM\capture.wav");



    // Executes the movement of a piece on the board and handles capturing pieces and promotions.
    private void Move(int startRow, int startCol, int endRow, int endCol)
    {
        move_sound.Play();
        Game.moves = Game.moves + ConvertToNotation(startRow, startCol, endRow, endCol) + ",";
        var startSquare = Board[7- startRow][startCol];
        var endSquare = Board[7- endRow][endCol];
       
        if(endSquare.Piece != null)
        {
            Capture(endSquare.Piece);
            capture_sound.Play();
        }

        if (startSquare.Piece is King) // update king pos if its a king
        {
            startSquare.Piece.ValidCastle = false;
            if(startSquare.Piece.PieceColor == "White")
            {
                WhiteKing = endSquare;
            }
            else
            {
                BlackKing = endSquare;
            }
        }

        if(startSquare.Piece is Rook)
        {
            startSquare.Piece.ValidCastle = false;
        }

        endSquare.Piece = startSquare.Piece;
        startSquare.Piece = null;
        startSquare.RevertColor();

        checkPromotion(endSquare); // checks for pawn promotion

        Game.endTurn();
    }

    // Checks if a pawn has reached the opposite end of the board for promotion.
    public void checkPromotion(Square endsquare)
    {
        if(endsquare.Row == 7 && endsquare.Piece.Name == "Pawn" || endsquare.Row == 0 && endsquare.Piece.Name == "Pawn")
        {
            endsquare.Piece = new Queen(endsquare.Piece.PieceColor);
        }
    }

    // Checks if the king of the current player has any valid moves.
    public bool KingHasMoves(string color) // Checks if the king of the color provided has a valid move
    {
        List<Square> validmoves = null;

        if(color == "White")
        {
            validmoves = getValidMoves(WhiteKing);
        }
        if (color == "Black")
        {
            validmoves = getValidMoves(BlackKing);
        }

        if(validmoves.Count == 0)
        {
            return false;
        }
        return true;
    }


    // Checks if a square is under threat from an opponent's piece. Used to prevent king moving into check.
    public bool isThreatened(Square checkSquare, string color)
    {

        Square s;
        Piece p;
        p = checkSquare.Piece;
        checkSquare.Piece = null;
        for(int i = 0; i < 8; i++)
        {
            for(int j = 0; j< 8; j++)
            {
                s = Board[7 - j][i];
                if(s.Piece != null && s.Piece.PieceColor != color)
                {
                    if (s.Piece is Pawn)
                    {
                        if (s.Piece.PieceColor == "White" && checkSquare.Row == s.Row + 1 && (checkSquare.Column == s.Column + 1 || checkSquare.Column == s.Column - 1))
                        {
                            checkSquare.Piece = p;
                            return true;
                        }

                        if(s.Piece.PieceColor == "Black" &&checkSquare.Row == s.Row - 1 && (checkSquare.Column == s.Column + 1 || checkSquare.Column == s.Column - 1))
                        {
                            checkSquare.Piece = p;
                            return true;
                        }

                    }
                    else if (isValidMove(s, checkSquare))
                    {
                        checkSquare.Piece = p;
                        return true;
                    }
                }
            }
        }
        checkSquare.Piece = p;
        return false;
    }

    // Finds the piece causing the check on king, aka 'threatening' the king.
    public Square findThreatPiece(Square checkSquare, string color)
    {

        Square s;
        Piece p;
        p = checkSquare.Piece;
        checkSquare.Piece = null;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                s = Board[7 - j][i];
                if (s.Piece != null && s.Piece.PieceColor != color)
                {
                    if (s.Piece is Pawn)
                    {
                        if (s.Piece.PieceColor == "White" && checkSquare.Row == s.Row + 1 && (checkSquare.Column == s.Column + 1 || checkSquare.Column == s.Column - 1))
                        {
                            checkSquare.Piece = p;
                            return s;
                        }

                        if (s.Piece.PieceColor == "Black" && checkSquare.Row == s.Row - 1 && (checkSquare.Column == s.Column + 1 || checkSquare.Column == s.Column - 1))
                        {
                            checkSquare.Piece = p;
                            return s;
                        }

                    }
                    else if (isValidMove(s, checkSquare))
                    {
                        checkSquare.Piece = p;
                        return s;
                    }
                }
            }
        }
        checkSquare.Piece = p;
        return null;
    }

    SoundPlayer notify_sound = new SoundPlayer(@"C:\Users\Toby\source\repos\ChessGridMVVM\chessmvm\ChessGridMVVM\notify.wav");
    // Check if king is in check
    private bool kinginCheck(string color)
    {
        if (color == "White")
        {
            if (isThreatened(WhiteKing, WhiteKing.Piece.PieceColor) == true) // king is in check
            {
                threat = findThreatPiece(WhiteKing, WhiteKing.Piece.PieceColor);
                notify_sound.Play();
                return true;
            }
        }
        
        if (color == "Black")
        {
            if (isThreatened(BlackKing, BlackKing.Piece.PieceColor) == true) // king is in check
            {
                threat = findThreatPiece(BlackKing, BlackKing.Piece.PieceColor);
                notify_sound.Play();
                return true;
            }

        }

        
        threat = null;
        return false;
    }

    // Checks if any squares can move into the line of check, thus blocking the check. Hence 'BlockingSquares'.
    private List<Square> blockingSquares()
    {
        List<Square> blockSquares = new List<Square>();
        Square start = null;
        Square end = null;

        if (Game.CurrentPlayer.Color == "White")
        {
            end = WhiteKing;
        }
        else if (Game.CurrentPlayer.Color == "Black")
        {
            end = BlackKing;
        }

        start = threat;
        blockSquares.Add(threat);

        int dx = end.Column - start.Column;
        int dy = end.Row - start.Row;
        int yStep = dy == 0 ? 0 : dy / Math.Abs(dy);
        int xStep = dx == 0 ? 0 : dx / Math.Abs(dx);
        int currentRow = start.Row + yStep;
        int currentColumn = start.Column + xStep;

        if (Math.Abs(dx) != Math.Abs(dy) && dx != 0 && dy != 0) // Check that move is either straight or diagonal 
        {
            return blockSquares;
        }

        while (currentRow != end.Row || currentColumn != end.Column && start.Piece.Name != "King") // Check if there exists a piece in any square between start and end 
        {
            blockSquares.Add(Board[7 - currentRow][currentColumn]);
            currentRow = currentRow + yStep;
            currentColumn = currentColumn + xStep;
        }

        foreach(Square s in blockSquares)
        {
            s.Color = "Green";
        }
        return blockSquares;
    }

    // Update check for new player. Ran at end of each turn.
    public void updateKingVariable(string color)
    {
        IsCurrentPlayerChecked = kinginCheck(color);   
    }

    // Checks if game is ended or can continue. Ran at end of each move.
    public string getCurrentState(string color)
    {
        if(KingHasMoves(color) == false) // king has no moves
        {
            if (color == "White")
            {
                if (kinginCheck(color)) // king is in check
                {
                    WhiteKing.Color = "Purple";
                    return "Checkmate";
                }

            }

            if (color == "Black")
            {
                if (kinginCheck(color)) // king is in check
                {
                    BlackKing.Color = "Purple";
                    return "Checkmate";
                }

            }
            return "Stalemate";
        }

        return "Valid";
        
    }

    private string ConvertToNotation(int startrow, int startcol, int endrow, int endcol)
    {
        string start;
        string end;
        string[] columns = { "a", "b", "c", "d", "e", "f", "g", "h" };
        start = columns[startcol] + Convert.ToString(startrow+1);
        end = columns[endcol] + Convert.ToString(endrow+1);
        return start + end;
    }
    // Stores captured piece in captured list
    private void Capture(Piece p)
    {

    }
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
