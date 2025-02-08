window.navMenuFunctions = {
    initialize: function (dotNetRef) {
        if (document.readyState === 'loading') {
            document.addEventListener('DOMContentLoaded', function () {
                dotNetRef.invokeMethodAsync('OnPageLoaded');
            });
        } else {
            dotNetRef.invokeMethodAsync('OnPageLoaded');
        }
    }
}; 