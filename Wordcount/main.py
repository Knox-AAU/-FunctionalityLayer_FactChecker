from fastapi import FastAPI
import uvicorn
import wordcount as count
from pydantic import BaseModel
from typing import Optional

# The parameter text that is going to be lemmatized is contained within a body


class Text(BaseModel):
    word: str
    article: str


# Create an instance of FastAPI
app = FastAPI()


@app.post("/")
# Function that returns the lemmatized text
async def queryWordcount(text: Text):
    return {"wordcount": count.wordCount(text.word, text.article)}

# uvicorn controls which host and port the API is available at.
if __name__ == "__main__":
    uvicorn.run(app, host="0.0.0.0", port=4000)
