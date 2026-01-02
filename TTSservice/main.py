import requests
from fastapi import FastAPI
from fastapi.middleware.cors import CORSMiddleware
import pyttsx3
import os

app = FastAPI()

app.add_middleware(
    CORSMiddleware,
    allow_origins=["http://localhost:5173"],
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

OLLAMA_URL = "http://localhost:11434/api/generate"

@app.post("/narrate")
def narrate(payload: dict):
    topic = payload.get("topic", "Give me a demo script about AI video generation")

    # Step 1: Ask Ollama to generate narration text
    ollama_resp = requests.post(OLLAMA_URL, json={
        "model": "llama2",   # or whichever model you pulled
        "prompt": f"Write a 30-second narration script about {topic}."
    }, stream=True)

    narration = ""
    for line in ollama_resp.iter_lines():
        if line:
            data = line.decode("utf-8")
            # Ollama streams JSON lines, extract 'response'
            if '"response":' in data:
                narration += data.split('"response":"')[1].split('"')[0]

    # Step 2: Convert narration to speech
    engine = pyttsx3.init()
    engine.setProperty("rate", 150)
    file_name = f"narration_{topic.replace(' ', '_')}.mp3"
    file_path = os.path.join("C:/Users/Samsung/source/repos/AI-Video-Stack/AI-Video-Stack.Server/wwwroot/Assets", file_name)
    engine.save_to_file(narration, file_path)
    engine.runAndWait()

    return {
        "script": narration,
        "publicUrl": f"/assets/{file_name}"
    }
