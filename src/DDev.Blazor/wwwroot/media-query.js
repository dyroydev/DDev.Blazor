export function createMediaQuery(queryString, dotNetObjectReference)
{
    const mediaQuery = window.matchMedia(queryString);
    mediaQuery.onchange = () => dotNetObjectReference.invokeMethodAsync('SetMatchesAsync', mediaQuery.matches);
    mediaQuery.onchange();

    return {
        dispose() {
            if (mediaQuery) {
                mediaQuery.onchange = null;
                mediaQuery = null;
            }
        }
    }
}