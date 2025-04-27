using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using ChessGridMVVM.Models;

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

        public void AddGame(int _whiteID, int _blackID, string _result)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string insertQuery = "INSERT INTO Games (White_PlayerID, Black_PlayerID, Result) VALUES (@_white_playerID,@_black_playerID, @_result)";
                using (var cmd = new SQLiteCommand(insertQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@_white_playerID", _whiteID);
                    cmd.Parameters.AddWithValue("@_black_playerID", _blackID);
                    cmd.Parameters.AddWithValue("@_result", _result);


                    cmd.ExecuteNonQuery();
                }

            }
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

        public int recordedGames(int whiteID, int blackID, string result)
        {
            int count = 0;
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string selectQuery = "SELECT * FROM Games WHERE White_PlayerID = " + Convert.ToString(whiteID) + " AND Black_PlayerID = " + Convert.ToString(blackID) + " AND Result = '" + result + "'";
                using (var cmd = new SQLiteCommand(selectQuery, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        public int fetchPlayerWins(int playerID)
        {
            int whiteWin = 0;
            int blackWin = 0;
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string selectQuery = "SELECT * FROM Games WHERE White_PlayerId = " + Convert.ToString(playerID) + " AND Result = '1-0'";
                using (var cmd = new SQLiteCommand(selectQuery, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        whiteWin++;
                    }
                }
                selectQuery = "SELECT * FROM Games WHERE Black_PlayerId = " + Convert.ToString(playerID) + " AND Result = '0-1'";
                using (var cmd = new SQLiteCommand(selectQuery, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        blackWin++;
                    }
                }
            }

            return whiteWin + blackWin;
        }
      

        public User fetchUser(string _username, string _password)
        {
            User user = null;
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string selectQuery = "SELECT * FROM Players WHERE Username = '" + _username + "' AND Password = '" + _password + "'";
                using (var cmd = new SQLiteCommand(selectQuery, conn))
                {
                    var dr = cmd.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        user = new User(dr.GetInt32(0), dr.GetString(1));
                    }
                }
            }

            return user;
        }
    }
}
