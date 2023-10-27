export function getWindowSize() {
    return {
        width: window.innerWidth,
        height: window.innerHeight
    };
};

function handleClick(cellText) {
    DotNet.invokeMethodAsync('BlazorTTS', 'HandleClick', cellText);
}

window.PlaySound = function (sound) {
    document.getElementById(sound).play();
}

