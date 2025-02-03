using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

namespace ChessGridMVVM
{
    public class DatabaseHelper
    {
        private string connectionString;
        public DatabaseHelper()
        {
            connectionString = "Data Source=|DataDirectory|PlayerDB.db; Version=3;";

            if (!File.Exists(GetDatabasePath()))
            {
                SQLiteConnection.CreateFile(GetDatabasePath());
            }
        }

        private string GetDatabasePath()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PlayerDB.db");
        }

        public List<User> GetUsers()
        {
            List<User> users = new List<User>();
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Players";
                using (var cmd = new SQLiteCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(new User(reader.GetInt32(0), reader.GetString(1)));
                    }
                }
            }
            return users;
        }
    }
}
