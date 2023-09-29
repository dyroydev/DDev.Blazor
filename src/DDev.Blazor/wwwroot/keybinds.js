import { findElement } from "./common.js"

export function create(dotNetObject, reference) {

    const target = !reference ? document : findElement(reference);

    const abortController = new AbortController()

    const callbacks = []

    function findBestMatchCallback(event) {
        let bestMatchCallbackScore = -1
        let bestMatchCallback = null;

        for (let callback of callbacks) {
            let score = 0;
            if (callback.key != event.key.toLowerCase()) continue;
            if (callback.shift == event.shiftKey) score++;
            if (callback.control == event.ctrlKey) score++;
            if (callback.alt == event.altKey) score++;
            if (callback.meta == event.metaKey) score++;

            if (score > bestMatchCallbackScore) {
                bestMatchCallbackScore = score
                bestMatchCallback = callback
            }
        }

        return bestMatchCallback;
    }

    if (target) {

        target.addEventListener("keydown", event => {

            let callback = findBestMatchCallback(event);

            if (!callback) return;

            event.preventDefault();
            event.stopPropagation();

            dotNetObject.invokeMethodAsync("InvokeCallbackAsync", callback.id);

        }, { signal: abortController.signal })

        // Stop key-up for handled key-bindings
        target.addEventListener("keyup", event => {
            if (findBestMatchCallback(event)) {
                event.preventDefault();
                event.stopPropagation();
            }
        }, { signal: abortController.signal })

        // Stop key-press for handled key-bindings
        target.addEventListener("keypress", event => {
            if (findBestMatchCallback(event)) {
                event.preventDefault();
                event.stopPropagation();
            }
        }, { signal: abortController.signal })

    }

    return {

        dispose() {
            abortController.abort()
        },

        addCallback(id, key, shift, control, alt, meta) {

            callbacks.push({
                id, key:key.toLowerCase(), shift, control, alt, meta
            })

        }

    }
}