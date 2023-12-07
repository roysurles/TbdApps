
var siteModule = (function () {

    function removeAttribute(elementId, attributeName) {
        let element = document.getElementById(elementId);
        if (element != null) {
            element.removeAttribute(attributeName)
        }
    }

    function setAttribute(elementId, attributeName, attributeValue) {
        let element = document.getElementById(elementId);
        if (element != null) {
            element.setAttribute(attributeName, attributeValue)
        }
    }

    function prefersDarkMode() {
        return window.matchMedia('(prefers-color-scheme: dark)').matches;
    }

    function setFocus(elementId) {
        let element = document.getElementById(elementId);
        if (element != null) {
            element.focus();
        }
    }

    function downloadFile(bytesBase64, mimeType, fileName) {
        let fileUrl = "data:" + mimeType + ";base64," + bytesBase64;
        fetch(fileUrl)
            .then(response => response.blob())
            .then(blob => {
                let link = window.document.createElement("a");
                link.href = window.URL.createObjectURL(blob, { type: mimeType });
                link.download = fileName;
                document.body.appendChild(link);
                link.click();
                document.body.removeChild(link);
            });
    };

    function uploadFile(inputID) {
        let inputEl = document.getElementById(inputID);
        if (inputEl.files.length == 0) {
            return "";
        }
        else if (inputEl.files[0].size > (4 * 1024 * 1024)) { // 4MB
            inputEl.value = "";
            alert("File size too large. Max allowed size is 4MB.");
            return "";
        }
        else if (inputEl.accept.length && inputEl.accept.indexOf(inputEl.files[0].name.split('.').pop()) < 0) {
            inputEl.value = "";
            alert("Allowed file types: " + inputEl.accept);
            return "";
        }
        const fileReader = new FileReader();
        return new Promise((resolve) => {
            fileReader.onloadend = function (e) {
                let data = {
                    fileName: inputEl.files[0].name,
                    fileData: e.target.result.split('base64,')[1]
                };
                resolve(data);
            };
            fileReader.readAsDataURL(inputEl.files[0]);
        });
    };

    // https://stackoverflow.com/questions/34191780/javascript-copy-string-to-clipboard-as-text-html
    function copyElementToClipboard(element) {
        window.getSelection().removeAllRanges();
        let range = document.createRange();
        range.selectNode(typeof element === 'string' ? document.getElementById(element) : element);
        window.getSelection().addRange(range);
        document.execCommand('copy');
        window.getSelection().removeAllRanges();
    }

    return {
        removeAttribute: removeAttribute,
        setAttribute: setAttribute,
        prefersDarkMode: prefersDarkMode,
        setFocus: setFocus,
        downloadFile: downloadFile,
        uploadFile: uploadFile,
        copyElementToClipboard: copyElementToClipboard
    }

}());