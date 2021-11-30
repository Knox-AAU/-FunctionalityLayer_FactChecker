using System;
using System.Collections.Generic;
using Xunit;

namespace XUnitTestProject
{
    public class IOTesting
    {
        [Fact]
        public async void TestReadWriteFile()
        {
            FactChecker.IO.FileStreamHandler fileStreamHandler = new FactChecker.IO.FileStreamHandler();
            fileStreamHandler.WriteFile("testfile.txt", "lorum hugo");
            List<string> result = await fileStreamHandler.ReadFile("testfile.txt");
            Assert.Equal("lorum hugo", result[0]);
        }

        [Fact]
        public async void TestAppendFile()
        {
            FactChecker.IO.FileStreamHandler fileStreamHandler = new FactChecker.IO.FileStreamHandler();
            fileStreamHandler.WriteFile("testfile2.txt", "lorum");
            fileStreamHandler.AppendToFile("testfile2.txt", "hugo");
            List<string> result = await fileStreamHandler.ReadFile("testfile2.txt");
            Assert.Equal("lorumhugo", result[0] + result[1]);
        }
    }
}
