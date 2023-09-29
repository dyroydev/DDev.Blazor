import { findElement } from "./common.js"

export function getProperty(reference, property) {
    let value = findElement(reference)
    for (let key in property.split(".")) value = value?.[key]
    return value
}

export function setProperty(reference, property, value) {
    let target = findElement(reference)
    const path = property.split(".")
    const finalKey = path.pop()
    for (let key in path) target = target?.[key]
    if (target) target[finalKey] = value
}

export function invokeMethod(reference, method, ...parameters) {
    let target = findElement(reference)
    const path = method.split(".")
    const finalKey = path.pop()
    for (let key in path) target = target?.[key]
    return target?.[finalKey]?.(parameters)
}