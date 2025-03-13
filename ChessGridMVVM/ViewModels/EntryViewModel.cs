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
            Players = new ObservableCollection<User>(_databaseHelper.GetPlayers());
        }

        public void addPlayer(string name, string password)
        {
            _databaseHelper.AddPlayer(name, password);
        }

        public bool login(string username, string password)
        {
            return _databaseHelper.validPlayer(username, password);
            

        }
    }
}
