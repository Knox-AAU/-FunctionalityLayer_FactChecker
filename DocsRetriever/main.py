from fastapi import FastAPI
import uvicorn
import docretriever as retriever
from pydantic import BaseModel


class Text(BaseModel):
    article: str


app = FastAPI()


@app.post("/")
async def queryDocretriever(text: Text):
    return {"doc": retriever.docRetriever(text.article)}

if __name__ == "__main__":
    uvicorn.run(app, host="0.0.0.0", port=3000)
