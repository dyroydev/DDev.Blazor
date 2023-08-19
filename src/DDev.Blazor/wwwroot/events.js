import { findElement } from "./common.js";

/**
 * Creates a new event listener.
 * @param {string} eventName
 * @param {any} reference
 */
export function createListener(eventName, reference) {
    const element = findElement(reference) ?? document;

    const controller = new AbortController();

    let dotNetReference;

    element.addEventListener(eventName, args => {

        dotNetReference?.invokeMethodAsync("onNextAsync", args);

    }, { signal: controller.signal })

    return {

        dispose() {

            controller.abort();

        },

        setDotNetReference(newDotNetReference) {

            dotNetReference = newDotNetReference;

        }

    }
}