using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FactChecker.IO
{
    /// <summary>
    /// A class used for Creating files, writing in files and reading from files.
    /// </summary>
    public class FileStreamHandler
    {

        /// <summary>
        /// Method taking two parameters of type (<paramref name="string"/>, <paramref name="string"/>).
        /// Used to create new files or overwrite existing ones.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="content"></param>
        public async void WriteFile(string path, string content)
        {
            using FileStream fs = File.OpenWrite(path);
            using var sr = new StreamWriter(fs);
            await sr.WriteLineAsync(content);
        }

        /// <summary>
        /// Method taking two parameters of type (<paramref name="string"/>, <paramref name="string"/>).
        /// Used to append text to a file.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="content"></param>
        public async void AppendToFile(string path, string content)
        {
            using var sr = new StreamWriter(path, append:true);
            await sr.WriteLineAsync(content);
        }

        /// <summary>
        /// Method taking a parameter of type (<paramref name="string"/>). Used to read from files.
        /// </summary>
        /// <param name="path"></param>
        /// <returns>A list of strings. Each string corresponds to one line from the document which is read from</returns>
        public async Task<List<string>> ReadFile(string path)
        {
            using FileStream fs = File.OpenRead(path);
            using var sr = new StreamReader(fs);

            string line;
            List<string> output = new List<string>();

            while ((line = await sr.ReadLineAsync()) != null)
            {
                output.Add(line);
            }

            return output;
        }
    }
    
}
