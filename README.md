# blazor-dragdrop
Drag and Drop Library for Blazor

[![Build Status](https://dev.azure.com/postlagerkarte/blazor-dragdrop/_apis/build/status/Postlagerkarte.blazor-dragdrop?branchName=master)](https://dev.azure.com/postlagerkarte/blazor-dragdrop/_build/latest?definitionId=3&branchName=master)

Demo:

https://blazordragdrop.azurewebsites.net/

Install:

Install-Package blazor-dragdrop -Version 1.0.0-alpha

Usage:

Add DragDropServiceFactory to your StartUp.cs under ConfigureServices

```csharp
services.AddScoped<DragDropServiceFactory>();
```

Use the ```<Dropzone>``` and ```<Draggable>``` components in your code.

Example:

Create a draggable list:
```html
    <Dropzone>
        <ul class="list-group">
            <Draggable>
                <li class="list-group-item">Cras justo odio</li>
            </Draggable>
            <Draggable>
                <li class="list-group-item">Dapibus ac facilisis in</li>
            </Draggable>
            <Draggable>
                <li class="list-group-item">Morbi leo risus</li>
            </Draggable>
            <Draggable>
                <li class="list-group-item">Porta ac consectetur ac</li>
            </Draggable>
            <Draggable>
                <li class="list-group-item">Vestibulum at eros</li>
            </Draggable>
        </ul>
    </Dropzone>
```

Mobile Devices:

The HTML 5 drag'n'drop API allows to implement drag'n'drop on most desktop browsers.

Unfortunately, most mobile browsers don't support it. 

You need to make use of a polyfill library, e.g. mobile-drag-drop

Add this to your _host.html:

```html
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/mobile-drag-drop@2.3.0-rc.2/default.css">
    <script src="https://cdn.jsdelivr.net/npm/mobile-drag-drop@2.3.0-rc.2/index.min.js"></script>

    <script>
        // options are optional ;)
        MobileDragDrop.polyfill({
            // use this to make use of the scroll behaviour
            dragImageTranslateOverride: MobileDragDrop.scrollBehaviourDragImageTranslateOverride
        });
    </script>
```
