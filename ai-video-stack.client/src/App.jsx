// import React, { useState } from "react";
// import PromptForm from "./components/PromptForm";
// import AudioPlayer from "./components/AudioPlayer";
// import VideoPlayer from "./components/VideoPlayer";
// import { generateSpeech, generateVideo } from "./services/api";

// function App() {
//   const [prompt, setPrompt] = useState("");
//   const [audioUrl, setAudioUrl] = useState(null);
//   const [videoUrl, setVideoUrl] = useState(null);
//   const [loading, setLoading] = useState(false);
//   const [error, setError] = useState(null);
 

// const handleSpeech = 
// async () => { setLoading(true); setError(null);
//    try { const data = await generateSpeech(prompt); 
    
//     setAudioUrl(`https://localhost:5001${data.publicUrl}`);
//        const Script=data.script;
//       // console.log("Generated script:", data.script);)
//    } catch (error) { setError("Error generating speech" + error); }
//       finally { setLoading(false); } };
//   const handleVideo = async () => {
//     setLoading(true);
//     setError(null);
//     try {
//       const data = await generateVideo(prompt);
//       if (data.videoUrl) {
//         setVideoUrl(data.videoUrl);
//       } else setError("Video generation failed");
//     } catch {
//       setError("Error calling video API");
//     } finally {
//       setLoading(false);
//     }
//   };

//   return (
//     <div className="home-page">
//       <h1>AI Narration & Video Generator</h1>
//       <PromptForm
//         prompt={prompt}
//         setPrompt={setPrompt}
//         onSpeech={handleSpeech}
//         onVideo={handleVideo}
//         loading={loading}
//       />
//       {loading && <p className="loading">Processing...</p>}
//       {error && <p className="error">{error}</p>}
//       <AudioPlayer url={audioUrl} />
//       <VideoPlayer url={videoUrl} />
//     </div>
//   );
// }

// export default App;
import React, { useState } from "react";
import PromptForm from "./components/PromptForm";
import { generateVideo ,generateSpeech } from "./services/api";

function App() {
  const [prompt, setPrompt] = useState("");
  const [style, setStyle] = useState("informative");
  const [lengthSec, setLengthSec] = useState(30);
  const [voice, setVoice] = useState("en-US");
  const [backgroundVideoUrl, setBackgroundVideoUrl] = useState("");
  const [title, setTitle] = useState("AI Narration Demo");

  const [script, setScript] = useState("");
  const [videoUrl, setVideoUrl] = useState(null);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);
    const [audioUrl, setAudioUrl] = useState(null);
    
   const handleSpeech =  async () =>  
           {
            
            setLoading(true);
           setError(null);
   try { const data = await generateSpeech(prompt); 
    
    setAudioUrl(`https://localhost:5001${data.publicUrl}`);
       const Script=data.script;
      // console.log("Generated script:", data.script);)
   } catch (error) {
       const Error = error;
       setError("Error generating speech");
       
}
      finally { setLoading(false); } };

  const handleVideo = async () => {
    setLoading(true);
    try {
      const payload = {
        topic: prompt,
        style,
        lengthSec,
        voice,
        backgroundVideoUrl,
        script: null,
        title
      };

      const data = await generateVideo(payload);
      setScript(data.script);
      setVideoUrl(data.audioUrl); // later poll status for final video
    } finally {
      setLoading(false);
    }
  };

  return (
    <div>
      <h1>AI Narration & Video Generator</h1>
      <PromptForm
        prompt={prompt} setPrompt={setPrompt}
        style={style} setStyle={setStyle}
        lengthSec={lengthSec} setLengthSec={setLengthSec}
        voice={voice} setVoice={setVoice}
        backgroundVideoUrl={backgroundVideoUrl} setBackgroundVideoUrl={setBackgroundVideoUrl}
        title={title} setTitle={setTitle}
        onSpeech={handleSpeech}
        onVideo={handleVideo}
        loading={loading}
      />
      {script && <p><strong>Script:</strong> {script}</p>}
      {loading && <p className="loading">Processing...</p>}
     
          {videoUrl && <video controls width="600" src={videoUrl}></video>}
          {audioUrl && <audio controls src={audioUrl}></audio>}
          {error && <p style={{ color: "red" }}>{error}</p>}
   </div>
  );
}

export default App;
