from bs4 import BeautifulSoup
import requests
import re


def docRetriever(article):
    string = ''

    req = requests.get(article)
    soup = BeautifulSoup(req.text, "lxml")

    length = len(soup.find_all('p'))

    for i in range(length):
        string += soup.find_all('p')[i].get_text()

    return re.sub("(\[.*\])*(\\n)*", '', string)
