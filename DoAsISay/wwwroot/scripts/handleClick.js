function handleClick(cellText) {
    DotNet.invokeMethodAsync('BlazorTTS', 'HandleClick', cellText);
    }