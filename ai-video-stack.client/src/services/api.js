  // // src/services/api.js
  // const FASTAPI_BASE = "http://localhost:8000";
  // const DOTNET_BASE = "https://localhost:5001";

  // export async function generateSpeech(topic) {
  //   const res = await fetch(`${FASTAPI_BASE}/narrate`, {
  //     method: "POST",
  //     headers: { "Content-Type": "application/json" },
  //     body: JSON.stringify({ topic}),
  //   });
  //   return res.json();
  // }

  // export async function generateVideo(prompt) {
  //   const res = await fetch(`${DOTNET_BASE}/api/video/render`, {
  //     method: "POST",
  //     headers: { "Content-Type": "application/json" },
  //    body: JSON.stringify({
  //      Topic: prompt.topic, 
  //      Style: prompt.style, 
  //      LengthSec: prompt.lengthSec, 
  //      Voice: prompt.voice, 
  //      BackgroundVideoUrl: prompt.backgroundVideoUrl,
  //       Script: prompt.script,
  //        Title: prompt.title }),
  //   });
  //   return res.json();
  // }

  // export async function getVoices() {
  //   const res = await fetch(`${FASTAPI_BASE}/voices`);
  //   return res.json();
  // }
  // export async function checkStatus(jobId) {
  //   const res = await fetch(`${DOTNET_BASE}/api/video/status/${jobId}`);
  //   return res.json();
  // }
// src/services/api.js
const FASTAPI_BASE = "http://localhost:8000";
const DOTNET_BASE = "https://localhost:5001";

export async function generateSpeech(topic) {
  const res = await fetch(`${FASTAPI_BASE}/narrate`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ topic }),
  });
  return res.json();
}

export async function generateVideo(prompt) {
  const res = await fetch('/api/video/render', {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({
      Topic: prompt.topic,
      Style: prompt.style,
      LengthSec: prompt.lengthSec,
      Voice: prompt.voice,
      BackgroundVideoUrl: prompt.backgroundVideoUrl,
      Script: prompt.script,
      Title: prompt.title,
    }),
  });
  return res.json(); // backend returns { id: "...", status: "queued" }
}

// export async function checkStatus(jobId) {
//   const res = await fetch(`${DOTNET_BASE}/api/video/status/${jobId}`);
//   return res.json(); // backend returns { id: "...", status: "done", url: "..." }
// }

export  const checkStatus = async (id) => {
  const res = await fetch(`$/api/video/status/${id}`);
  if (!res.ok) {
    const text = await res.text();
    throw new Error(text); // will show backend error
  }
  return res.json(); // only parse if valid JSON
};

