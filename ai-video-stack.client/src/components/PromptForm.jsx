// import React from "react";

// function PromptForm({ prompt, setPrompt, onSpeech, onVideo, loading }) {
//   return (
//     <div className="prompt-form">
//       <textarea
//         value={prompt}
//         onChange={(e) => setPrompt(e.target.value)}
//         placeholder="Enter your narration prompt..."
//         rows={5}
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
import React from "react";

function PromptForm({
  prompt, setPrompt,
  style, setStyle,
  lengthSec, setLengthSec,
  voice, setVoice,
  backgroundVideoUrl, setBackgroundVideoUrl,
  title, setTitle,
  onSpeech, onVideo, loading
}) {
    return (
    <div className="prompt-form">
      <textarea
        value={prompt}
        onChange={(e) => setPrompt(e.target.value)}
        placeholder="Enter your narration prompt..."
        rows={5}
      />
      <input
        value={style}
        onChange={(e) => setStyle(e.target.value)}
        placeholder="Style (informative, funny, serious)"
      />
      <input
        type="number"
        value={lengthSec}
        onChange={(e) => setLengthSec(Number(e.target.value))}
        placeholder="Length in seconds"
      />
      <input
        value={voice}
        onChange={(e) => setVoice(e.target.value)}
        placeholder="Voice (e.g. en-US)"
      />
      <input
        value={backgroundVideoUrl}
        onChange={(e) => setBackgroundVideoUrl(e.target.value)}
        placeholder=""
      />
      <input
        value={title}
        onChange={(e) => setTitle(e.target.value)}
        placeholder="Title"
      />

      <div className="buttons">
        <button onClick={onSpeech} disabled={loading || !prompt}>
          🎤 Generate Speech
        </button>
        <button onClick={onVideo} disabled={loading || !prompt}>
          🎬 Generate Video
        </button>
      </div>
    </div>
  );
}

export default PromptForm;
