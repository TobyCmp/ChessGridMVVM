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

        public List<User> GetPlayers()
        {
            List<User> players = new List<User>();
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Players";
                using (var cmd = new SQLiteCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        players.Add(new User(reader.GetInt32(0), reader.GetString(1)));
                    }
                }
            }
            return players;
        }

        public void AddPlayer(string _username, string _password)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string insertQuery = "INSERT INTO Players (Username,Password) VALUES (@_username,@_password)";
                using (var cmd = new SQLiteCommand(insertQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@_username", _username);
                    cmd.Parameters.AddWithValue("@_password", _password);

                    cmd.ExecuteNonQuery();
                }

            }
        }
    }
}
