import unittest
import docsretriever as doc
import requests


class docRetrieverTest(unittest.TestCase):

    def tesLinktoWiki(self):
        doc.testdocretriever("https://en.wikipedia.org/wiki/John_B._Larson")
        self.assertTrue(len(), len())

if __name__ == '__main__':
    unittest.main()
