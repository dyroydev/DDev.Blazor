/**
 * Resolves the reference to a single node or null.  
 * @param {any} reference A node, an element id or a query.
 * @returns {Element | null}
 */
export function findElement(reference) {
    if (reference instanceof Element)
        return reference;

    if (typeof reference == "string")
        return document.getElementById(reference) || document.querySelector(reference);

    return null;
}

/**
 * Converts the AbortController to a object with a single dispose() method.
 * @param {AbortController | undefined} abortController
 * @returns {{dispose(): void}}
 */
export function toDisposable(abortController) {
    return {
        dispose() {
            abortController?.abort();
        }
    }
}