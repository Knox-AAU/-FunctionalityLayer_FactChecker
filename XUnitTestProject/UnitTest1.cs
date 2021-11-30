using System;
using System.Collections.Generic;
using Xunit;

namespace XUnitTestProject
{
    public class UnitTest1
    {
        [Fact]
        public async void TestReadWriteFile()
        {
            FactChecker.IO.FileStreamHandler fileStreamHandler = new FactChecker.IO.FileStreamHandler();
            fileStreamHandler.WriteFile("testfile.txt", "lorum hugo");
            List<string> result = await fileStreamHandler.ReadFile("testfile.txt");
            Assert.Equal("lorum hugo", result[0]);
        }
    }
}
