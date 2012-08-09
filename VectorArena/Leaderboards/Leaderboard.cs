using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Xml.Linq;

namespace VectorArena
{
    public class Leaderboard
    {
        public List<LeaderboardEntry> Entries;

        IsolatedStorageFile file;
        IsolatedStorageFileStream stream;
        StreamWriter writer;

        const string path = "leaderboard.xml";

        public Leaderboard()
        {
            Entries = new List<LeaderboardEntry>(10);
            for (int e = 0; e < Entries.Capacity; e++)
            {
                Entries.Add(new LeaderboardEntry(e + 1, 0));
            }

            file = IsolatedStorageFile.GetUserStoreForApplication();
            stream = new IsolatedStorageFileStream(path, FileMode.OpenOrCreate, file);
            writer = new StreamWriter(stream);
        }

        public void Save()
        {
            XDocument document = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement("Leaderboard"));

            foreach(LeaderboardEntry entry in Entries)
            {
                document.Add(new XElement("LeaderboardEntry", entry));
            }
        }

        public void Load()
        {

        }

        public void AddEntry(int score)
        {
            for (int e = 0; e < Entries.Count; e++)
            {
                if (score >= Entries[e].Score)
                {
                    Entries.Insert(e, new LeaderboardEntry(e + 1, score));
                    Entries.RemoveAt(10);
                    for (int i = e + 1; i < Entries.Count; i++)
                    {
                        Entries[i].Rank = i + 1;
                    }
                    //Save();
                    return;
                }
            }
        }
    }

    public class LeaderboardEntry
    {
        public int Rank { get; set; }
        public DateTime Date { get; set; }
        public int Score { get; set; }

        public LeaderboardEntry(int rank, int score)
        {
            Rank = rank;
            Date = DateTime.Now;
            Score = score;
        }
    }
}
