window.playAudio = (audioSrc) => {
    const audioPlayer = document.getElementById('audioPlayer');
    audioPlayer.src = audioSrc;
    audioPlayer.play();
};

window.pauseAudio = () => {
    const audioPlayer = document.getElementById('audioPlayer');
    audioPlayer.pause();
};

window.resumeAudio = () => {
    const audioPlayer = document.getElementById('audioPlayer');
    audioPlayer.play();
};

window.stopAudio = () => {
    const audioPlayer = document.getElementById('audioPlayer');
    audioPlayer.pause();
    audioPlayer.currentTime = 0;
};
