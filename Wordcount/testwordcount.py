import unittest
from wordcount import articleStore

class wordcountTest(unittest.TestCase):

    def testWebscraper(self):
        self.assertIsNotNone(articleStore(
            1, "https://en.wikipedia.org/wiki/Eternals_(film)"))

    def testWebscraper(self):
        self.assertIsNotNone(
            articleStore(
            1, "https://en.wikipedia.org/wiki/Eternals_(film)"))



if __name__ == '__main__':
    unittest.main()
