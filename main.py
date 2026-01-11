from fastapi import FastAPI
from pydantic import BaseModel
from pathlib import Path
import uuid
import os
import pyttsx3
from fastapi.staticfiles import StaticFiles

app = FastAPI()

# Prefer environment variable; fallback to relative path
ASSETS_DIR = Path(os.getenv("ASSETS_DIR","C:/Users/Samsung/source/repos/AI-Video-Stack/AI-Video-Stack.Server"))
ASSETS_DIR.mkdir(parents=True, exist_ok=True)

class TtsRequest(BaseModel):
    text: str
    voice: str | None = None   # optional: system voice id
    rate: int | None = None    # optional: words per minute (default ~200)
    volume: float | None = None  # optional: 0.0 to 1.0

@app.get("/")
def health():
    return {"status": "ok"}

@app.get("/voices")
def voices():
    engine = pyttsx3.init()
    vs = engine.getProperty('voices')
    return [{"id": v.id, "name": v.name} for v in vs]

@app.post("/tts")
def tts(req: TtsRequest):
    file_name = f"narration_{uuid.uuid4().hex}.mp3"
    out_path = ASSETS_DIR / file_name

    engine = pyttsx3.init()

    if req.voice:
        engine.setProperty('voice', req.voice)
    if req.rate:
        engine.setProperty('rate', req.rate)
    if req.volume is not None:
        engine.setProperty('volume', max(0.0, min(1.0, req.volume)))

    engine.save_to_file(req.text, str(out_path))
    engine.runAndWait()

    return {
        "fileName": file_name,
        "publicUrl": f"{file_name}",
        "durationSec": 0.0
    }

# Mount static files so generated audio can be served
app.mount("/assets", StaticFiles(directory=ASSETS_DIR), name="assets")
