export default function onLongTextAreaInput(longTextArea: HTMLTextAreaElement) {
    longTextArea.style.height = "50px"

    let scrollHeight = longTextArea.scrollHeight + 5
    if (scrollHeight > 250) {
        scrollHeight = 250
    }

    longTextArea.style.height = scrollHeight + "px"
}