using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace Green_Masters
{
    internal class Databas
    {
      
            private string connectionString = "Data Source=scores.db;Version=3;";

            public Databas()
            {
                CreateDatabase();
            }

            // Skapar databasen och poängtavlan om den inte finns
            private void CreateDatabase()
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "CREATE TABLE IF NOT EXISTS Scores (Id INTEGER PRIMARY KEY AUTOINCREMENT, PlayerName TEXT, Score INTEGER)";
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }

            // Lägger till en ny poäng i databasen
            public void SaveScore(string PlayerName, float _score)
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Scores (PlayerName, Score) VALUES (@playerName, @score)";
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@playerName", PlayerName);
                        command.Parameters.AddWithValue("@score", _score);
                        command.ExecuteNonQuery();
                    }
                }
            }

            // Hämtar topplistan (de 5 bästa poängen)
            public List<(string PlayerName, int Score)> GetTopScores()
            {
                List<(string, int)> scores = new List<(string, int)>();

                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT PlayerName, Score FROM Scores ORDER BY Score DESC LIMIT 5";
                    using (var command = new SQLiteCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            scores.Add((reader["PlayerName"].ToString(), Convert.ToInt32(reader["Score"])));
                        }
                    }
                }

                return scores;
            }
        }
    }

