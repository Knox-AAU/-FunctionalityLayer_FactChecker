from bs4 import BeautifulSoup
import requests
import json


def wordCount():

    number = 0
    wordstring = ''

    userArticle = input(
        'Please input a Wikipedia article URL. Try "http://en.wikipedia.org/wiki/Special:Random" to get a random article!: ')
    word = input(
        'Please enter a word to get the word count for (THIS IS CASE SENSITIVE): ')

    req = requests.get(userArticle)
    soup = BeautifulSoup(req.text, "lxml")
    
    text = {
        "string": soup.get_text(),
        "language" : "en"
    }
    textJson = json.dumps(text)
    lemmatized_string = requests.post(
        f"http://127.0.0.1:5000/", data=textJson)

    lemma = str(lemmatized_string.content.decode(encoding='UTF-8').split(" "))

    length = len(soup.find_all('p'))

    for w in lemma:
        if w == word:
            number+=1

    print(soup.get_text())

    print(lemmatized_string.content.decode(encoding='UTF-8'))
    
if __name__ == "__main__":
    wordCount()
