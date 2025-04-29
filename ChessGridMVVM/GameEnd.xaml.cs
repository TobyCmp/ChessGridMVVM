using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ChessGridMVVM
{
    /// <summary>
    /// Interaction logic for GameEnd.xaml
    /// </summary>
    public partial class GameEnd : Window
    {
        private DatabaseHelper _databaseHelper;
        string _moves;

        public GameEnd(User white, User black, string result, string moves)
        {
            InitializeComponent();
            _databaseHelper = new DatabaseHelper();
            if(result == "1-1")
            {
                resultBox.Text = string.Format("Stalemate! - Game ends in a draw.");
            }
            if (result == "0-1")
            {
                resultBox.Text = string.Format("Checkmate! - Black wins");
            }
            if (result == "1-0")
            {
                resultBox.Text = string.Format("Checkmate! - White wins");
            }
            _moves = moves;
            SaveGame(white.Id, black.Id, result);
        }

        private void SaveGame(int whiteID, int blackID, string result)
        {
            _databaseHelper.AddGame(whiteID, blackID, result);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Entry entry = new Entry();
            entry.Show();
            Close();
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            Moves.Text = _moves;
        }
    }
}
