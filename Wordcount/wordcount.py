from bs4 import BeautifulSoup
import requests
import json
import re
import sqlite3


def articleStore(i, article):
    string = ""

    req = requests.get(article)
    soup = BeautifulSoup(req.text, "lxml")
    length = len(soup.find_all('p'))

    for x in range(length):
        string += soup.find_all('p')[x].get_text()

    clean_text = re.sub("(\[.*?\])*(\\n)*", '', string)
    text = {
        "string": clean_text,
        "language": "en"
    }
    textJson = json.dumps(text)
    lemmatized_string = requests.post(
        f"http://127.0.0.1:5000/", data=textJson)
    lemmatized_string = re.sub(
        "[)(}{.,;:_—/”\"-]", " ", lemmatized_string.content.decode('UTF-8'))
    processed_list = re.sub("[\\\\]", " ", lemmatized_string).split(" ")

    cursor = db.cursor()
    data_tuple = (i, article, len(processed_list),
                  len(set(processed_list)), clean_text)
    cursor.execute(
        "INSERT or REPLACE INTO ARTICLE (ID,LINK,LENGTH,UNIQUE_LENGTH,TEXT) VALUES (?, ?, ?, ?, ?)", data_tuple)
    db.commit()
    cursor.close()
    return processed_list


db = sqlite3.connect("wordcount.db")

""" db.execute('''CREATE TABLE WORDCOUNT
         (ID INTEGER PRIMARY KEY    AUTOINCREMENT,
         WORD           TEXT    NOT NULL,
         ARTICLEID       INT    NOT NULL,
         OCCURRENCE      INT     NOT NULL,
         FOREIGN KEY (ARTICLEID) REFERENCES ARTICLE (ID));''') """

links = ["https://en.wikipedia.org/wiki/Eternals_(film)",
         "https://en.wikipedia.org/wiki/Kenosha_unrest_shooting",
         "https://en.wikipedia.org/wiki/Travis_Scott",
         "https://en.wikipedia.org/wiki/Shang-Chi_and_the_Legend_of_the_Ten_Rings",
         "https://en.wikipedia.org/wiki/Arcane_(TV_series)",
         "https://en.wikipedia.org/wiki/Spider-Man:_No_Way_Home",
         "https://en.wikipedia.org/wiki/Adele",
         "https://en.wikipedia.org/wiki/Taylor_Swift",
         "https://en.wikipedia.org/wiki/Elon_Musk",
         "https://en.wikipedia.org/wiki/Dune_(2021_film)",
         "https://en.wikipedia.org/wiki/Jake_Gyllenhaal",
         "https://da.wikipedia.org/wiki/Donald_Trump",
         "https://en.wikipedia.org/wiki/2022_FIFA_World_Cup_qualification",
         "https://en.wikipedia.org/wiki/Ghostbusters:_Afterlife",
         "https://en.wikipedia.org/wiki/Zac_Stacy",
         "https://en.wikipedia.org/wiki/Joe_Biden"]

for i, url in enumerate(links):
    article = articleStore(i, url)
    for word in set(article):
        if word != "":
            cursor = db.cursor()
            data_tuple = (word, i, article.count(word))
            cursor.execute(
                "INSERT INTO WORDCOUNT (WORD,ARTICLEID,OCCURRENCE) VALUES (?, ?, ?)", data_tuple)
            db.commit()
            cursor.close()

db.close()
