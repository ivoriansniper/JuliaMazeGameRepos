window.AddOnKeyDownEvent = (dotNetInstance) => {
    document.onkeydown = function(evt) {
        evt = evt || window.event;
        dotNetInstance.invokeMethodAsync('OnKeyPress', evt.keyCode);
    };
};