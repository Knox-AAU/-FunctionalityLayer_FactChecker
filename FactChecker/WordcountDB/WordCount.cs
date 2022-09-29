using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace FactChecker.WordcountDB
{
    public class WordCount
    {
        private string source = "wordcount.db";
        /// <summary>Takes parameter of type <paramref name="string"/> and fetches all matching articles.</summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public List<WordCountItem> FetchDB(string word)
        {
            Console.WriteLine(Directory.GetCurrentDirectory());
            List<WordCountItem> list = new List<WordCountItem>();
            string connection_string = $"Data Source={source}";
            using var connection = new SQLiteConnection(connection_string);
            connection.Open();

            string statement = $"SELECT * FROM WORDCOUNT WHERE WORD = \"{word}\"";

            using var cmd = new SQLiteCommand(statement, connection);
            using SQLiteDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                //Console.WriteLine($"{rdr.GetInt32(0)} {rdr.GetString(1)} {rdr.GetInt32(2)} {rdr.GetInt32(3)}");
                list.Add(new WordCountItem(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2), reader.GetInt32(3)));
            }
            connection.Close();
            return list;
        }

        public int FetchArticlesCountContainingWord(string word)
        {
            Console.WriteLine(Directory.GetCurrentDirectory());
            List<WordCountItem> list = new List<WordCountItem>();
            string connection_string = $"Data Source={source}";
            using var connection = new SQLiteConnection(connection_string);
            connection.Open();

            string statement = $"SELECT * FROM WORDCOUNT WHERE WORD = \"{word}\"";

            using var cmd = new SQLiteCommand(statement, connection);
            using SQLiteDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                //Console.WriteLine($"{rdr.GetInt32(0)} {rdr.GetString(1)} {rdr.GetInt32(2)} {rdr.GetInt32(3)}");
                list.Add(new WordCountItem(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2), reader.GetInt32(3)));
            }
            connection.Close();
            return list.Count;
        }

        public int FetchSumOfOccurences(string word)
        {
            string connection_string = $"Data Source={source}";
            using var connection = new SQLiteConnection(connection_string);
            connection.Open();

            string statement = $"SELECT SUM(OCCURRENCE) FROM WORDCOUNT WHERE WORD = \"{word}\"";

            using var cmd = new SQLiteCommand(statement, connection);
            using SQLiteDataReader reader = cmd.ExecuteReader();

            int sum = 0;
            if(reader.Read() && reader.GetFieldType(0) == typeof(System.Int64))
            {
              sum = reader.GetInt32(0);
            }
            connection.Close();
            return sum; 
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="articleid">The articleId to fetch sum of word Ocurences from</param>
        /// <returns></returns>
        public int FetchSumOfOccurencesFromArticleId(int articleid)
        {
            string connection_string = $"Data Source={source}";
            using var connection = new SQLiteConnection(connection_string);
            connection.Open();

            string statement = $"SELECT SUM(OCCURRENCE) FROM WORDCOUNT WHERE ARTICLEID = \"{articleid}\"";

            using var cmd = new SQLiteCommand(statement, connection);
            using SQLiteDataReader reader = cmd.ExecuteReader();

            int sum = 0;
            if (reader.Read() && reader.GetFieldType(0) == typeof(System.Int64))
            {
                sum = reader.GetInt32(0);
            }
            connection.Close();
            return sum;
        }
        public int FetchTotalDocuments()
        {
            string connection_string = $"Data Source={source}";
            using var connection = new SQLiteConnection(connection_string);
            connection.Open();

            string statement = $"SELECT COUNT(ID) FROM ARTICLE";

            using var cmd = new SQLiteCommand(statement, connection);
            using SQLiteDataReader reader = cmd.ExecuteReader();

            int sum = 0;
            if (reader.Read() && reader.GetFieldType(0) == typeof(System.Int64))
            {
                sum = reader.GetInt32(0);
            }
            connection.Close();
            return sum;
        }
    }
}

