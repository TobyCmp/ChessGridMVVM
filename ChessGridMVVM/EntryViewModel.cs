using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGridMVVM
{
    public class EntryViewModel
    {
        private DatabaseHelper _databaseHelper;

        public ObservableCollection<User> Users { get; set; }

        public EntryViewModel()
        {
            _databaseHelper = new DatabaseHelper();
            // Users = new ObservableCollection<User>(_databaseHelper.GetUsers());
        }
    } 
}
