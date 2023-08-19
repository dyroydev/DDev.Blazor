import { findElement } from "./common.js";

/**
 * Gets the bounding rectangle for the referenced element.
 * @param {any} reference
 * @returns {{x:number, y:number, width:number, height:number }}
 */
export function getBoundingRectangle(reference) {

    const element = findElement(reference);

    if (!element) return { x:0, y:0, width:0, height:0 };

    const bounds = element.getBoundingClientRect();

    return {
        x: bounds.left + window.scrollX,
        y: bounds.top + window.scrollY,
        width: bounds.width,
        height: bounds.height
    }
}

/**
 * Gets the bounding rectangle of the viewport.
 * @returns
 */
export function getViewportBoundingRectangle() {

    return {
        x: scrollX,
        y: scrollY,
        width: window.innerWidth,
        height: window.innerHeight
    }

}