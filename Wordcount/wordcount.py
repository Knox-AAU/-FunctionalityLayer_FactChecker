from bs4 import BeautifulSoup
import requests
import json


def wordCount(word, article):

    req = requests.get(article)
    soup = BeautifulSoup(req.text, "lxml")
    clean_text = soup.get_text().split("Jump to search")[1]
    clean_text = clean_text.split(
        "Retrieved from \"https://en.wikipedia.org/w/index.php?title=")[0]

    text = {
        "string": clean_text,
        "language": "en"
    }
    textJson = json.dumps(text)
    lemmatized_string = requests.post(
        f"http://127.0.0.1:5000/", data=textJson)

    lemma = str(lemmatized_string.content.decode(encoding='UTF-8')).split(" ")
    return lemma.count(word)
