// // import React from "react";

// // function PromptForm({ prompt, setPrompt, onSpeech, onVideo, loading }) {
// //   return (
// //     <div className="prompt-form">
// //       <textarea
// //         value={prompt}
// //         onChange={(e) => setPrompt(e.target.value)}
// //         placeholder="Enter your narration prompt..."
// //         rows={5}
// //       />
// //       <div className="buttons">
// //         <button onClick={onSpeech} disabled={loading || !prompt}>
// //           🎤 Generate Speech
// //         </button>
// //         <button onClick={onVideo} disabled={loading || !prompt}>
// //           🎬 Generate Video
// //         </button>
// //       </div>
// //     </div>
// //   );
// // }

// // export default PromptForm;
// import React from "react";

// function PromptForm({
//   prompt, setPrompt,
//   style, setStyle,
//   lengthSec, setLengthSec,
//   voice, setVoice,
//   backgroundVideoUrl, setBackgroundVideoUrl,
//   title, setTitle,
//   onSpeech, onVideo, loading
// }) {
//     return (
//     <div className="prompt-form">
//       <textarea
//         value={prompt}
//         onChange={(e) => setPrompt(e.target.value)}
//         placeholder="Enter your narration prompt..."
//         rows={5}
//       />
//       <input
//         value={style}
//         onChange={(e) => setStyle(e.target.value)}
//         placeholder="Style (informative, funny, serious)"
//       />
//       <input
//         type="number"
//         value={lengthSec}
//         onChange={(e) => setLengthSec(Number(e.target.value))}
//         placeholder="Length in seconds"
//       />
//       <input
//         value={voice}
//         onChange={(e) => setVoice(e.target.value)}
//         placeholder="Voice (e.g. en-US)"
//       />
//       <input
//         value={backgroundVideoUrl}
//         onChange={(e) => setBackgroundVideoUrl(e.target.value)}
//         placeholder=""
//       />
//       <input
//         value={title}
//         onChange={(e) => setTitle(e.target.value)}
//         placeholder="Title"
//       />

//       <div className="buttons">
//         <button onClick={onSpeech} disabled={loading || !prompt}>
//           🎤 Generate Speech
//         </button>
//         <button onClick={onVideo} disabled={loading || !prompt}>
//           🎬 Generate Video
//         </button>
//       </div>
//     </div>
//   );
// }

// export default PromptForm;

import React, { useState, useEffect } from "react";
import { generateVideo, checkStatus } from "../services/api";
const DOTNET_BASE = "https://localhost:5001";
function PromptForm({
  prompt, setPrompt,
  style, setStyle,
  lengthSec, setLengthSec,
  voice, setVoice,
  backgroundVideoUrl, setBackgroundVideoUrl,
  title, setTitle,
  onSpeech, loading
}) {
  const [jobId, setJobId] = useState(null);
  const [videoUrl, setVideoUrl] = useState(null);
  const [status, setStatus] = useState(null);

  const onVideo = async () => {
    setStatus("starting...");
    const result = await generateVideo({
      topic: prompt,
      style,
      lengthSec,
      voice,
      backgroundVideoUrl,
      title,
    });
    setJobId(result.id);
    setStatus(result.status);
  };

  // Poll Shotstack status every 5s until done
  // useEffect(() => {
  //   let interval;
  //   if (jobId && status !== "done") {
  //     interval = setInterval(async () => {
  //       const res = await checkStatus(jobId);
  //       setStatus(res.status);
  //       if (res.status === "done") {
  //         setVideoUrl(res.url);
  //         clearInterval(interval);
  //       }
  //     }, 5000);
  //   }
  //   return () => clearInterval(interval);
  // }, [jobId, status]);
//   useEffect(() => {
//   if (jobId) {
//     const interval = setInterval(async () => {
//       const res = await fetch(`${DOTNET_BASE}/api/video/status//${jobId}`);
//       const data = await res.json();
//       setStatus(data.response.status);
//       if (data.response.status === "done") {
//         setVideoUrl(data.response.url);
//         clearInterval(interval);
//       }
//       if (data.response.status === "failed") {
//         clearInterval(interval);
//       }
//     }, 2000);
//     return () => clearInterval(interval);
//   }
// }, [jobId]);
useEffect(() => {
  if (!jobId) return;

  const interval = setInterval(async () => {
    const res = await fetch(`/api/video/status/${jobId}`);
    const data = await res.json();

    setStatus(data.response.status);

    if (data.response.status === "done") {
      setVideoUrl(data.response.url);
      clearInterval(interval);
    }

    if (data.response.status === "failed") {
      clearInterval(interval);
    }
  }, 3000);

  return () => clearInterval(interval);
}, [jobId]);


  useEffect(() => {
  if (videoUrl) {
    console.log("Final video URL:", videoUrl);
  }
}, [videoUrl]);

  return (
    <div className="prompt-form">
      <textarea
        value={prompt}
        onChange={(e) => setPrompt(e.target.value)}
        placeholder="Enter your narration prompt..."
        rows={5}
      />
      <input value={style} onChange={(e) => setStyle(e.target.value)} placeholder="Style" />
      <input type="number" value={lengthSec} onChange={(e) => setLengthSec(Number(e.target.value))} placeholder="Length in seconds" />
      <input value={voice} onChange={(e) => setVoice(e.target.value)} placeholder="Voice (e.g. en-US)" />
      <input value={backgroundVideoUrl} onChange={(e) => setBackgroundVideoUrl(e.target.value)} placeholder="Background video URL (GitHub Pages)" />
      <input value={title} onChange={(e) => setTitle(e.target.value)} placeholder="Title" />

      <div className="buttons">
        <button onClick={onSpeech} disabled={loading || !prompt}>🎤 Generate Speech</button>
        <button onClick={onVideo} disabled={loading || !prompt}>🎬 Generate Video</button>
      </div>

      {jobId && <p>Job ID: {jobId}</p>}
      {status && <p>Status: {status}</p>}
      {videoUrl && (
        <div>
          <p>Video ready:</p>
          <video src={videoUrl} controls width="600" />
        </div>
      )}
    </div>
  );
}

export default PromptForm;
