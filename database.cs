using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Threading.Tasks;

namespace Green_Masters
{
    internal class database
    {
        List<string> Names = new List<string>();
        List<float> Score = new List<float>();
        string newName = "";
        int newScore = -1;


        public database(string _name, int _score)
        {
            newName = _name;
            newScore = _score;

            ReadFile();
            UpdateScoreBoard();
        }

        private void ReadFile()
        {
            StreamReader sr = new StreamReader("../database.csv");
            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                string[] subDiv = line.Split(';');
                Names.Add(subDiv[0]);
                Score.Add(Convert.ToInt32(subDiv[1]));
            }
            sr.Close();
        }

        private void UpdateScoreBoard()
        {
            for (int i = 0; i < Score.Count; i++)
            {
                if (newScore > Score[i])
                {
                    Score.Insert(i, newScore);
                    Names.Insert(i, newName);
                    break;
                }
            }
            ShortenScoreboard();
            StreamWriter sw = new StreamWriter("../database.csv");

            for (int i = 0; i < Score.Count; i++)
            {
                sw.WriteLine($"{Names[i]};{Score[i]}");
            }
            sw.Close();
        }

        private void ShortenScoreboard()
        {
            if (Score.Count > 5)
            {
                Score.RemoveRange(5, Score.Count - 5);
                Names.RemoveRange(5, Names.Count - 5);
            }
        }

        public string PrintFileContent()
        {
            if (!File.Exists("../database.csv"))
            {
                Console.WriteLine("Filen hittades inte.");
                return"";
            }

            using (StreamReader sr = new StreamReader("../database.csv"))
            {
                return sr.ReadToEnd(); // Läser hela filen och returnerar som en sträng
            }
        }
    }
}
