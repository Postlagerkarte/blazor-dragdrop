# blazor-dragdrop
Drag and Drop Library for Blazor

[![Build Status](https://dev.azure.com/postlagerkarte/blazor-dragdrop/_apis/build/status/Postlagerkarte.blazor-dragdrop?branchName=master)](https://dev.azure.com/postlagerkarte/blazor-dragdrop/_build/latest?definitionId=3&branchName=master)

#### Demo:

https://blazordragdrop.azurewebsites.net/

#### Install:

Install-Package blazor-dragdrop

[![NuGet version (blazor-dragdrop)](https://img.shields.io/nuget/v/blazor-dragdrop.svg?style=flat-square)](https://www.nuget.org/packages/blazor-dragdrop)

#### Usage:

1) Add DragDropServiceFactory to your Startup.cs

```csharp
services.AddScoped<DragDropServiceFactory>();
```

2)  Add the css to your _host.cshtml

```html
<link href="_content/Blazor.DragDrop.Core/dragdrop.css" rel="stylesheet" />
```

------

#### Make element draggable:

```html
<Dropzone>
    <Draggable>
        <li class="list-group-item">Cras justo odio</li>
    </Draggable>
<Dropzone>
```

#### Create a dropzone:

```html
<Dropzone>

</Dropzone>
```

#### Features:

Only allow limited number of items in a Dropzone: 

```html
<Dropzone MaxItems="2">

</Dropzone>
```

Restricted Dropzone: (executes the give accept func before accepting the draggable)

```html
<Dropzone Accepts='(d) => d.Gender == "Male"'>

</Dropzone>
```

Attach data to draggables:
```html
<Draggable Tag='new { Gender = "Female" , Age = 22}'>
<li class="list-group-item">Cras justo odio</li>
</Draggable>
```
------

#### Examples:

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

------
#### Mobile Devices:

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
