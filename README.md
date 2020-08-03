# blazor-dragdrop
Drag and Drop Library for Blazor

[![Build Status](https://dev.azure.com/postlagerkarte/blazor-dragdrop/_apis/build/status/Postlagerkarte.blazor-dragdrop?branchName=master)](https://dev.azure.com/postlagerkarte/blazor-dragdrop/_build/latest?definitionId=3&branchName=master)

#### Demo:

https://blazordragdrop.azurewebsites.net/

#### Install:

Install-Package blazor-dragdrop

[![NuGet version (blazor-dragdrop)](https://img.shields.io/nuget/v/blazor-dragdrop.svg?style=flat-square)](https://www.nuget.org/packages/blazor-dragdrop)

#### Usage:

1) Add BlazorDragDrop to your Startup.cs

```csharp
services.AddBlazorDragDrop();
```

2)  Include relevant stylesheet either in your _host.cshtml (server-side blazor) or index.html (client-side blazor) 

```html
<link href="_content/blazor-dragdrop/dragdrop.css" rel="stylesheet" />
```

------

#### Create a draggable list for your items

You have to create a dropzone and assign your items to it:

```html
    <Dropzone Items="MyItems">
        
    </Dropzone>
```
This will not yet render anything - you have to provide a render template so that the dropzone knows how to render your items. 

```html
    <Dropzone Items="MyItems">
        <div>@context.MyProperty</div>
    </Dropzone>
```

You can also use your own component as a render template:

```html
    <Dropzone Items="MyItems">
        <MyCompoenent Item="@context"></MyComponent>
    </Dropzone>
```
By default the dropzone will use a build-in style for the drag/drop animations. 

If you add or remove items to the underlying list the dropzone will automatically update. 

After a drag operation you can inspect your list to get the current position of items. 

#### Features:

Only allow limited number of items in a Dropzone: 

```html
<Dropzone MaxItems="2">

</Dropzone>
```
If you drop something and the limit you can get a notification by providing a callback.

Restrict Drop: (executes the give accept func before accepting the draggable)

```html
<Dropzone Accepts='(d) => d.Gender == "Male"'>

</Dropzone>
```
If you drop something and the item is rejected you can get a notification by providing a callback.

Multiple Dropzone:

You can create more than one dropzone and move items betweens them.

#### Styling:

If you want to provide your own style you have to set the Style attribute to 'custom':

```html
<Dropzone Items="MyItems" Style="DragDropStyle.Custom">
```
You can then target:

```html
plk-dd-dragged-over
plk-dd-in-transit
```

#### Examples:

Check the demo page.

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
