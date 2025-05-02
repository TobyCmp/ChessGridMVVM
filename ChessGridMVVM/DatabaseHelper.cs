using System;

using System.Collections.Generic;

using System.Data.SQLite;

using System.IO;

using System.Text;

using ChessGridMVVM.Models;



namespace ChessGridMVVM

{

    // Helper class for handling all database operations 

    public class DatabaseHelper

    {

        private string connectionString;



        // Constructor initializes the database and creates it if not exists 

        public DatabaseHelper()

        {

            connectionString = "Data Source=|DataDirectory|PlayerDB.db; Version=3;";



            if (!File.Exists(getDatabasePath()))

            {

                SQLiteConnection.CreateFile(getDatabasePath());

            }

        }



        // Returns the full database file path 

        private string getDatabasePath()

        {

            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PlayerDB.db");

        }



        // Recursively counts records in batches (example of recursion) 

        private int countRecordsRecursive(SQLiteDataReader reader, int count = 0)

        {

            if (!reader.Read())

            {

                return count;

            }

            return countRecordsRecursive(reader, count + 1);

        }



        // Fetches all players from the database 

        public List<User> getPlayers()

        {

            List<User> players = new List<User>();

            try

            {

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

            }

            catch (Exception ex)

            {

                Console.WriteLine($"Error fetching players: {ex.Message}");

            }

            return players;

        }



        // Adds a new game record to the database 

        public void addGame(int whiteId, int blackId, string result)

        {

            try

            {

                using (var conn = new SQLiteConnection(connectionString))

                {

                    conn.Open();

                    string insertQuery = "INSERT INTO Games (White_PlayerID, Black_PlayerID, Result) VALUES (@whiteId, @blackId, @result)";

                    using (var cmd = new SQLiteCommand(insertQuery, conn))

                    {

                        cmd.Parameters.AddWithValue("@whiteId", whiteId);

                        cmd.Parameters.AddWithValue("@blackId", blackId);

                        cmd.Parameters.AddWithValue("@result", result);



                        cmd.ExecuteNonQuery();

                    }

                }

            }

            catch (Exception ex)

            {

                Console.WriteLine($"Error adding game: {ex.Message}");

            }

        }



        // Adds a new player to the database 

        public void addPlayer(string username, string password)

        {

            try

            {

                using (var conn = new SQLiteConnection(connectionString))

                {

                    conn.Open();

                    string insertQuery = "INSERT INTO Players (Username, Password) VALUES (@username, @password)";

                    using (var cmd = new SQLiteCommand(insertQuery, conn))

                    {

                        cmd.Parameters.AddWithValue("@username", username);

                        cmd.Parameters.AddWithValue("@password", password);



                        cmd.ExecuteNonQuery();

                    }

                }

            }

            catch (Exception ex)

            {

                Console.WriteLine($"Error adding player: {ex.Message}");

            }

        }



        // Counts the number of recorded games with specific players and result 

        public int recordedGames(int whiteId, int blackId, string result)

        {

            int count = 0;

            try

            {

                using (var conn = new SQLiteConnection(connectionString))

                {

                    conn.Open();

                    string selectQuery = "SELECT * FROM Games WHERE White_PlayerID = @whiteId AND Black_PlayerID = @blackId AND Result = @result";

                    using (var cmd = new SQLiteCommand(selectQuery, conn))

                    {

                        cmd.Parameters.AddWithValue("@whiteId", whiteId);

                        cmd.Parameters.AddWithValue("@blackId", blackId);

                        cmd.Parameters.AddWithValue("@result", result);



                        using (var reader = cmd.ExecuteReader())

                        {

                            count = countRecordsRecursive(reader); // Recursive counting! 

                        }

                    }

                }

            }

            catch (Exception ex)

            {

                Console.WriteLine($"Error counting recorded games: {ex.Message}");

            }

            return count;

        }



        // Fetches the total number of wins for a player 

        public int fetchPlayerWins(int playerId)

        {

            int whiteWins = 0;

            int blackWins = 0;

            try

            {

                using (var conn = new SQLiteConnection(connectionString))

                {

                    conn.Open();

                    string selectWhiteWinsQuery = "SELECT * FROM Games WHERE White_PlayerId = @playerId AND Result = '1-0'";

                    using (var cmd = new SQLiteCommand(selectWhiteWinsQuery, conn))

                    {

                        cmd.Parameters.AddWithValue("@playerId", playerId);

                        using (var reader = cmd.ExecuteReader())

                        {

                            whiteWins = countRecordsRecursive(reader);

                        }

                    }



                    string selectBlackWinsQuery = "SELECT * FROM Games WHERE Black_PlayerId = @playerId AND Result = '0-1'";

                    using (var cmd = new SQLiteCommand(selectBlackWinsQuery, conn))

                    {

                        cmd.Parameters.AddWithValue("@playerId", playerId);

                        using (var reader = cmd.ExecuteReader())

                        {

                            blackWins = countRecordsRecursive(reader);

                        }

                    }

                }

            }

            catch (Exception ex)

            {

                Console.WriteLine($"Error fetching player wins: {ex.Message}");

            }



            return whiteWins + blackWins;

        }



        // Fetches a user based on username and password 

        public User fetchUser(string username, string password)

        {

            User user = null;

            try

            {

                using (var conn = new SQLiteConnection(connectionString))

                {

                    conn.Open();

                    string selectQuery = "SELECT * FROM Players WHERE Username = @username AND Password = @password";

                    using (var cmd = new SQLiteCommand(selectQuery, conn))

                    {

                        cmd.Parameters.AddWithValue("@username", username);

                        cmd.Parameters.AddWithValue("@password", password);



                        using (var reader = cmd.ExecuteReader())

                        {

                            if (reader.Read())

                            {

                                user = new User(reader.GetInt32(0), reader.GetString(1));

                            }

                        }

                    }

                }

            }

            catch (Exception ex)

            {

                Console.WriteLine($"Error fetching user: {ex.Message}");

            }

            return user;

        }



        // Fetches and sorts moves for a given game ID 

        public List<string> GetSortedMoves(int gameId)

        {

            List<string> moves = new List<string>();



            try

            {

                using (var conn = new SQLiteConnection(connectionString))

                {

                    conn.Open();

                    string selectQuery = "SELECT Move FROM Moves WHERE GameID = @gameId ORDER BY MoveNumber ASC";

                    using (var cmd = new SQLiteCommand(selectQuery, conn))

                    {

                        cmd.Parameters.AddWithValue("@gameId", gameId);



                        using (var reader = cmd.ExecuteReader())

                        {

                            while (reader.Read())

                            {

                                moves.Add(reader.GetString(0));

                            }

                        }

                    }

                }



                // Apply the recursive QuickSort algorithm to sort the moves list using median of three pivot 

                QuickSort(moves, 0, moves.Count - 1);

            }

            catch (Exception ex)

            {

                Console.WriteLine($"Error fetching sorted moves for game {gameId}: {ex.Message}");

            }



            return moves;

        }



        // Recursive QuickSort 

        private void QuickSort(List<string> moves, int low, int high)

        {

            if (low < high)

            {

                int pivotIndex = Partition(moves, low, high);

                QuickSort(moves, low, pivotIndex - 1); // Before pivot 

                QuickSort(moves, pivotIndex + 1, high); // After pivot 

            }

        }



        // Partitioning step for QuickSort 

        private int Partition(List<string> moves, int low, int high)

        {

            // Find the median of three (first, middle, last) 

            int middle = low + (high - low) / 2;



            // Get the median of the three elements 

            string pivot = MedianOfThree(moves, low, middle, high);



            // Swap the pivot with the last element (since the pivot needs to be at the end) 

            int pivotIndex = moves.IndexOf(pivot);

            Swap(moves, pivotIndex, high);



            // Now perform the partitioning step 

            string pivotValue = moves[high]; // Pivot is now at the end 

            int i = (low - 1); // Index of smaller element 



            for (int j = low; j < high; j++)

            {

                // If current element is less than or equal to pivot 

                if (String.Compare(moves[j], pivotValue) <= 0)

                {

                    i++;

                    // Swap moves[i] and moves[j] 

                    Swap(moves, i, j);

                }

            }



            // Swap moves[i + 1] and moves[high] (or pivot) 

            Swap(moves, i + 1, high);



            return i + 1;

        }



        // Median of three pivot selection 

        private string MedianOfThree(List<string> moves, int low, int middle, int high)

        {

            // Get the values of the first, middle, and last elements 

            string first = moves[low];

            string middleElement = moves[middle];

            string last = moves[high];



            // Determine the median value 

            if (String.Compare(first, middleElement) > 0)

            {

                Swap(moves, low, middle); // Swap first and middle 

            }



            if (String.Compare(first, last) > 0)

            {

                Swap(moves, low, high); // Swap first and last 

            }



            if (String.Compare(middleElement, last) > 0)

            {

                Swap(moves, middle, high); // Swap middle and last 

            }



            // After the above swaps, the middle element will be the median 

            return moves[middle];

        }



        // Swap two elements in the list 

        private void Swap(List<string> moves, int i, int j)

        {

            string temp = moves[i];

            moves[i] = moves[j];

            moves[j] = temp;

        }

    }

}