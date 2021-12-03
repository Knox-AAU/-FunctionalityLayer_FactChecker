using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FactChecker.IO
{
    public class FileStreamHandler
    {

        public async void WriteFile(string path, string content)
        {
            using FileStream fs = File.OpenWrite(path);
            using var sr = new StreamWriter(fs);
            await sr.WriteLineAsync(content);
        }

        public async void AppendToFile(string path, string content)
        {
            using var sr = new StreamWriter(path, append:true);
            await sr.WriteLineAsync(content);
        }

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
