window.playAudio = (elementName) => {
    document.getElementById(elementName).play();
}

window.blazorCulture = {
    get: () => window.localStorage['BlazorCulture'],
    set: (value) => window.localStorage['BlazorCulture'] = value
};

window.helpers = {
    scrollIntoView: function (id) {
        const element = document.getElementById(id);
        element.scrollIntoView();
    }
}