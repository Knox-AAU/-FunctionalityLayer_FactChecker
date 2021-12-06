using FactChecker.PassageRetrieval;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace XUnitTestProject
{
    public class PassageTesting
    {
        [Fact]
        public void TestOverlapThrowsException()
        {
            PassageItem pi = new PassageItem("Hej med dig")
            {
                PassageLength = 20
            };

            Assert.Throws<ArgumentOutOfRangeException>(() => pi.PassageOverlap = 22);
        }

        [Fact]
        public void TestOverlapCalculation()
        {
            PassageItem pi = new PassageItem("Hej med dig");
            pi.PassageLength = 10;

            Assert.Equal(2, pi.PassageOverlap = 2);
        }


        [Fact]
        public void TestTextInputSplit()
        {
            PassageItem pi = new PassageItem("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aenean laoreet risus id felis vulputate pellentesque. Phasellus tempor consequat nisl a.");

            Assert.Equal(20, pi.FullText.Split(' ').Length);
        }

        [Fact]
        public void TestNumberOfPassagesFound()
        {
            PassageItem pi = new PassageItem("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aenean laoreet risus id felis vulputate pellentesque. Phasellus tempor consequat nisl a.");

            pi.PassageLength = 8;
            pi.PassageOverlap = 4;

            Assert.Equal(4, pi.GetPassages().Count);
        }
    }
}
