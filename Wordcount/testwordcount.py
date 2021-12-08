import unittest
from wordcount import articleStore


class wordcountTest(unittest.TestCase):

    def testWebscraper(self):
        self.assertIsNotNone(articleStore(
            "https://en.wikipedia.org/wiki/Eternals_(film)"))

    def testArticleInTuple(self):
        self.assertIsInstance(articleStore(
            "https://en.wikipedia.org/wiki/Eternals_(film)")[0], str)

    def testLenghtInTuple(self):
        self.assertIsInstance(articleStore(
            "https://en.wikipedia.org/wiki/Eternals_(film)")[1], int)

    def testUniqueLenght(self):
        testdata = articleStore(
            "https://en.wikipedia.org/wiki/Eternals_(film)")
        self.assertLessEqual(testdata[2], testdata[1])


if __name__ == '__main__':
    unittest.main()
