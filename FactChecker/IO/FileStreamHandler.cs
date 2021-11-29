using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FactChecker.IO
{
    public class FileStreamHandler
    {

        public void WriteFile(string path, string content)
        {
            using FileStream fs = File.OpenWrite(path);
            StreamWriter sr = new StreamWriter(fs);
            sr.WriteLine(content);
        }

        public void AppendToFile(string path, string content)
        {
            StreamWriter sr = new StreamWriter(path, append:true);
            sr.WriteLine(content);
        }

        public List<string> ReadFile(string path)
        {
            using FileStream fs = File.OpenRead(path);
            StreamReader sr = new StreamReader(fs);

            string line;
            List<string> output = new List<string>();

            while ((line = sr.ReadLine()) != null)
            {
                output.Add(line);
            }

            return output;
        }
    }
}
