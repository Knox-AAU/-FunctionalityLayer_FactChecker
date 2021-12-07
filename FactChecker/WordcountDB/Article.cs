using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;

namespace FactChecker.WordcountDB
{
    public class Article
    {
        /// <summary>Takes parameter of type <paramref name="int"/> and fetches the matching article</summary>
        /// <param name="id"></param>
        /// <returns>The articles that matches the input parameter</returns>
        public ArticleItem FetchDB(int id)
        {
            List<ArticleItem> list = new List<ArticleItem>();
            string connection_string = "Data Source=wordcount.db";
            using var connection = new SQLiteConnection(connection_string);
            connection.Open();

            string statement = $"SELECT * FROM ARTICLE WHERE ID = {id}";

            using var cmd = new SQLiteCommand(statement, connection);
            using SQLiteDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new ArticleItem(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2), reader.GetInt32(3), reader.GetString(4)));
            }
            connection.Close();
            return list[0];
        }
    }
}
