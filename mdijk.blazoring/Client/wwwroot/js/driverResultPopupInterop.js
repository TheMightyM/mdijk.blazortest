window.BlazorDriverInterop = {
    driverEventPopup: function(dotnetHelper, driverResults) {


        tippy(driverResults.elementId, {
            content: 'Loading...',
            allowHTML: true,
               onShow(instance) {
                   // Code here is executed every time the tippy shows
                   dotnetHelper.invokeMethodAsync('GetEventInformation')
                       .then(data => {
                           instance.setContent(data);
                           console.log(data);
                       });
               },
               onHidden(instance) {
                   instance.setContent('Loading...');
            },
        });
       
        return true;
    }
}