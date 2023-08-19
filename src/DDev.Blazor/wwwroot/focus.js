import { findElement } from "./common.js";

/**
 * Determines if the referenced element is or contains the focused element.
 * @param {any} reference
 */
export function hasFocus(reference) {
    const element = findElement(reference);

    if (!element) return false;

    return element.matches(":focus")
        || element.matches(":focus-within")
        || element == document.activeElement
        || element.contains(document.activeElement);
}

/**
 * Gives focus to the referenced element if found.
 * @param {any} reference
 */
export function setFocus(reference) {
    const element = findElement(reference);
    element?.focus?.();
}

export function setFocusToFirstChild(reference) {
    const element = findElement(reference);
    setFocus(getFocusableChildren(element).pop());
}

export function setFocusToLastChild(reference) {
    const element = findElement(reference);
    setFocus(getFocusableChildren(element).shift());
}

/**
 * Gets all focusable children of element in order.
 * @param {HTMLElement} element
 */
function getFocusableChildren(element) {
    if (!element) return [];
    const pattern = `a, button, input, textarea, select, dialog, details, iframe, embed, object, summary, audio[controls], video[controls], [tabindex='0'], [contenteditable]`;
    const matches = [...element.querySelectorAll(pattern)];
    return matches.filter(e => !e.hasAttribute("disabled") && !e.hasAttribute("hidden") && !e.hasAttribute("inert"));

}