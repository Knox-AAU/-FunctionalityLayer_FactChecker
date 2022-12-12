from bs4 import BeautifulSoup
import requests
import json
import re
import psycopg2


def articleStore(article):
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

    data_tuple = (article, len(processed_list),
                  len(set(processed_list)), clean_text, processed_list)
    return data_tuple


if __name__ == '__main__':
    conn = psycopg2.connect("dbname=test user=postgres password=1234")
    cur = conn.cursor()

    links = [
             "https://en.wikipedia.org/wiki/Wikipedia:Random",
             "https://en.wikipedia.org/wiki/Manufacturers_Association_of_Nigeria",
             "https://en.wikipedia.org/wiki/Eternals_(film)",
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
             "https://en.wikipedia.org/wiki/Donald_Trump",
             "https://en.wikipedia.org/wiki/2022_FIFA_World_Cup_qualification",
             "https://en.wikipedia.org/wiki/Ghostbusters:_Afterlife",
             "https://en.wikipedia.org/wiki/Zac_Stacy",
             "https://en.wikipedia.org/wiki/Joe_Biden"

             ]

    for i, url in enumerate(links):
        cur = conn.cursor()
        data_tuple = articleStore(url)
        article = data_tuple[-1]
        data_tuple = data_tuple[:-1]
        data_tuple = (i,) + data_tuple[:len(data_tuple)]
        cur.execute(
            "INSERT INTO ARTICLE (LINK,LENGTH,UNIQUE_LENGTH,TEXT) VALUES (%s, %s, %s, %s)", (data_tuple[1], data_tuple[2], data_tuple[3], data_tuple[4]))
        conn.commit()
        cur.close()

        for word in set(article):
            if word != "":
                cursor = conn.cursor()
                data_tuple = (word, i+1, article.count(word))
                cursor.execute(
                    "INSERT INTO WORDCOUNT (WORD,ARTICLEID,OCCURRENCE) VALUES (%s, %s, %s)", data_tuple)
                conn.commit()
                cursor.close()

    print(f"{url}")

    conn.close()
