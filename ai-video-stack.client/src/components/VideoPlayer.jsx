import React from "react";

function VideoPlayer({ url }) {
  if (!url) return null;
  return (
    <div className="output">
      <h2>Generated Video</h2>
      <video controls width="600" src={url}></video>
      <p><a href={url} target="_blank" rel="noreferrer">Open Video</a></p>
    </div>
  );
}

export default VideoPlayer;
