//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Xunit;
//using System.Data.SQLite;
//using FactChecker.Rake;
//using FactChecker.Interfaces;
//using FactChecker.IO;
//using System.Runtime;

//namespace XUnitTestProject
//{
//    public class RakeUnitTest
//    {
//        [Fact]
//        public async void ReturnPasagesMatch() { 
//            List<string> Expted_Passages = new List<string>() {"late 2006, biden's stance shifted considerably  ."};
//            List<string> Retrived_Passages = new List<string>();
//            FileStreamHandler f = new FileStreamHandler();
//            List<string> stopwords = (await f.ReadFile("../../../../FactChecker/TestData/stopwords.txt"));
//            Rake rake = new(stopwords: stopwords, sentences_min_length: 10);
//            rake.extract_keywords_from_text("late 2006, Biden's stance had shifted considerably.");
//            List<Passage> ps = rake.get_ranked_phrases();
//            foreach (Passage passage in ps) {
//                Retrived_Passages.Add(passage.ProcessedPassageAsString);
//            }
//            Assert.True(Expted_Passages.Contains(Retrived_Passages.First()) && Expted_Passages.Contains(Retrived_Passages.Last()));
            
//        }
//        [Fact]
//        public async void ReturnPasagesCount() {
//            FileStreamHandler f = new FileStreamHandler();
//            List<string> stopwords = (await f.ReadFile("../../../../FactChecker/TestData/stopwords.txt"));
//            Rake rake = new(stopwords: stopwords, sentences_min_length: 15);
//            rake.extract_keywords_from_text("A smale test string, that is will spilt up. It is only for testing.");
//            List<Passage> ps = rake.get_ranked_phrases();
//            Assert.Equal(2, ps.Count());
            
//        }
//        [Fact]
//        public async void WordTokenizeToWords() {
//            FileStreamHandler f = new FileStreamHandler();
//            List<string> stopwords = (await f.ReadFile("../../../../FactChecker/TestData/stopwords.txt"));
//            Rake rake = new(stopwords: stopwords);
//            List<string> expedted_words = new() { "he", "died", "of", "brain", "cancer", "in", "2015", "."};
//            List<string> retrived_words = rake._tokenize_sentence_to_words("he died of brain cancer in 2015.");
//            Assert.Equal(expedted_words.Count(), retrived_words.Count());
//        }
//        [Fact]
//        public async void StopwordsSet() { 
//            List<string> stopwords = new() { "a", "test", "List" };
//            Rake rake = new(stopwords: stopwords);
//            bool issame = true;
//            foreach (var stopword in rake.stopwords) {
//                if (!stopwords.Contains(stopword))
//                {
//                    issame = false;
//                }
//            }
//            Assert.True(issame);
//        }
//    }
//}
