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
        if (element)
        {
            element.scrollIntoView({
                behavior: 'smooth'
            });
        }
    },
    scrollToBottom : function() {
        return new Promise((resolve, reject) => { 
            window.scrollTo({ left: 0, top: document.body.scrollHeight, behavior: "smooth" }); 
            resolve(); 
        });
    },
    attachScrollEventHandler: function(objRef) {
        window.addEventListener("scroll", () => {
            return objRef.invokeMethodAsync('OnScroll', { X: window.scrollX , Y: window.scrollY });
        });
    }
}