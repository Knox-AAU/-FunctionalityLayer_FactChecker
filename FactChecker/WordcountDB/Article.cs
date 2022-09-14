using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;

namespace FactChecker.WordcountDB
{
    public class Article
    {
        private string source = "../../../../../../../wordcount.db";

        /// <summary>Takes parameter of type <paramref name="int"/> and fetches the matching article</summary>
        /// <param name="id"></param>
        /// <returns>The articles that matches the input parameter</returns>
        public ArticleItem FetchDB(int id)
        {
            return new ArticleItem(5, "https://en.wikipedia.org/wiki/Spider-Man:_No_Way_Home", 100, 80, "Spider-Man: No Way Home is an upcoming American superhero film based on the Marvel Comics character Spider-Man co-produced by Columbia Pictures and Marvel Studios and distributed by Sony Pictures Releasing. It is the sequel to Spider-Man: Homecoming (2017) and Spider-Man: Far From Home (2019), and the 27th film in the Marvel Cinematic Universe (MCU). The film is directed by Jon Watts, written by Chris McKenna and Erik Sommers, and stars Tom Holland as Peter Parker / Spider-Man alongside Zendaya, Benedict Cumberbatch, Jacob Batalon, Jon Favreau, Marisa Tomei, J. B. Smoove, Benedict Wong, Jamie Foxx, Alfred Molina, Willem Dafoe, Thomas Haden Church, and Rhys Ifans. In the film, Parker asks Dr. Stephen Strange (Cumberbatch)");
            List<ArticleItem> list = new List<ArticleItem>();
            string connection_string = $"Data Source={source}";
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
