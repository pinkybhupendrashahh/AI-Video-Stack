import React from "react";

function AudioPlayer({ url }) {
  if (!url) return null;
  return (
    <div className="output">
      <h2>Generated Narration</h2>
      <audio controls src={url}></audio>
      <p><a href={url} target="_blank" rel="noreferrer">Download Audio</a></p>
    </div>
  );
}

export default AudioPlayer;
