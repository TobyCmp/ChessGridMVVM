using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ChessGridMVVM.ViewModels
{
    public class EntryViewModel
    {
        private DatabaseHelper _databaseHelper;

        public ObservableCollection<User> Players { get; set; }

        public EntryViewModel()
        {
            _databaseHelper = new DatabaseHelper();
            Players = new ObservableCollection<User>(_databaseHelper.getPlayers());
        }

        public void addPlayer(string name, string password)
        {
            _databaseHelper.addPlayer(name, password);
        }

        public User login(string username, string password)
        {
            return _databaseHelper.fetchUser(username, password);
        }

        public int getWins(int playerID)
        {
            return _databaseHelper.fetchPlayerWins(playerID);
        }
        
        public string headToHead(int player1_id, int player2_id)
        {

            int player1_Wwins = _databaseHelper.recordedGames(player1_id, player2_id, "1-0");
            int player1_Bwins = _databaseHelper.recordedGames(player2_id, player1_id, "0-1");
            int player1_wins = player1_Bwins + player1_Wwins;

            int player2_Bwins = _databaseHelper.recordedGames(player1_id, player2_id, "0-1");
            int player2_Wwins = _databaseHelper.recordedGames(player2_id, player1_id, "1-0");
            int player2_wins = player2_Bwins + player2_Wwins;

            int draw = _databaseHelper.recordedGames(player2_id, player1_id, "1-1") + _databaseHelper.recordedGames(player1_id, player2_id, "1-1");

            string result = Convert.ToString(player1_wins) + "-" + Convert.ToString(player2_wins) + "-" + Convert.ToString(draw);

            return result;
        }
    }
}
