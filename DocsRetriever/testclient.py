import requests
import json

text = {
    "article": "https://en.wikipedia.org/wiki/John_B._Larson",
}
textJson = json.dumps(text)
lemmatized_string = requests.post(
    f"http://127.0.0.1:3000/", data=textJson)

print(str(lemmatized_string.content.decode(encoding='UTF-8')))
