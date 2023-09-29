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

export function setFocusToNext(reference) {
    let element = findElement(reference);
    while (element) {
        element = element.nextElementSibling
        if (isFocusable(element)) {
            setFocus(element)
            return true
        }
    }
    return false
}

export function setFocusToPrevious(reference) {
    let element = findElement(reference);
    while (element) {
        element = element.previousElementSibling
        if (isFocusable(element)) {
            setFocus(element)
            return true
        }
    }
    return false
}

/**
 * Gets all focusable children of element in order.
 * @param {HTMLElement} element
 */
function getFocusableChildren(element) {
    if (!element) return [];
    const matches = [...element.querySelectorAll(FOCUSABLE_PATTERN)];
    return matches.filter(isFocusEnabled);

}

function isFocusable(element) {
    return element && element.matches(FOCUSABLE_PATTERN) && isFocusEnabled(element)
}

function isFocusEnabled(focusableElement) {
    return focusableElement && !focusableElement.hasAttribute("disabled") && !focusableElement.hasAttribute("hidden") && !focusableElement.hasAttribute("inert")
}

const FOCUSABLE_PATTERN = `a, button, input, textarea, select, dialog, details, iframe, embed, object, summary, audio[controls], video[controls], [tabindex='0'], [contenteditable]`;