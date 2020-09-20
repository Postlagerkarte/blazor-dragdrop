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

2) Add relevant styles to your app (since 2.2 no styles are included in the library anymore)

You can copy styles from [dragdrop.css](Plk.Blazor.DragDrop.Demo/wwwroot/css/dragdrop.css) to your site.css to get started. Read more about styling [here](#styling).

------

Version 1.x:

Please upgrade to 2.0 - there will be no support/bugfixes for the 1.x version.

Version 2.x :

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
You access your item via the @context - Alternatively, you can specify the Context attribute on the Dropzone element to assign a different name.

By default the dropzone will use a build-in style for the drag/drop animations. 

If you add or remove items to the underlying list the dropzone will automatically update. 

After a drag operation you can inspect your list to get the current position of items. 

Use the TItem property of the Dropzone to tell the compiler about the item type you are using. It is helpful when using some of the event handlers. 

#### Features:

**Only allow limited number of items in a Dropzone:** 

```html
<Dropzone MaxItems="2">

</Dropzone>
```
If you drop something and the limit is reached you can get a notification by providing a callback.

**Restrict Drop (executes the give accept func before accepting the draggable):**

```html
<Dropzone Accepts='(d) => d.Gender == "Male"'>

</Dropzone>
```
If you drop something and the item is rejected you can get a notification by providing a callback.

**Copy Items:**

By default, items are moved between dropzones. 

If you instead want the item to be copied you can make use of the "CopyItem" attribute:

```html
<Dropzone CopyItem="(item)=> { return new TodoItem() {Titel = item.Titel}; }" Items="MyThirdList" TItem="TodoItem" OnItemDrop="@((i)=>lastdropped = i)">
    <div style="border: 2px solid black">
        @context.Titel
    </div>
</Dropzone>
```
The CopyItem attribute expects a method telling the dropzone how to create a copy of the item. It receives the currently active item as input.

**Instant Replace:**

By default you get a visual clue for the drop operation. You can activate Instant Replace to instead swap out items on the fly. This option should only be used for small lists.

```html
<Dropzone InstantReplace="true">

</Dropzone>
```


**Multiple Dropzone:**

You can create more than one dropzone and move items between them.


**Dropzone Groups:**

You can set dropzone group id to restrict the ability to move items between multiple dropzones or to create nested dropzones.

```html
<Dropzone ... DropzoneGroupId="commonRootZone" Context="RootContext">
	<Dropzone ... DropzoneGroupId="commonChildZone" Context="ChildContext">
		...
	</Dropzone>
	<Dropzone ... DropzoneGroupId="uncommonChildZoneOne" Context="ChildContext">
		...
	</Dropzone>
</Dropzone>

<Dropzone ... DropzoneGroupId="commonRootZone" Context="RootContext">
	<Dropzone ... DropzoneGroupId="commonChildZone" Context="ChildContext">
		...
	</Dropzone>
	<Dropzone ... DropzoneGroupId="uncommonChildZoneTwo" Context="ChildContext">
		...
	</Dropzone>
</Dropzone>
```

#### Styling:

To style the dropzone divs you can either create a css selector for plk-dd-dropzone or you assign classes to the dropzone:

```html
<Dropzone Class="my-a my-b">
```

Furthermore, you create css selectors for the following classes:

```html
plk-dd-dragged-over (class added to the item that is currently dragged over)
plk-dd-in-transit (class added to the item that is currently dragged around)
plk-dd-spacing (class added to the div that sits between two items)
plk-dd-spacing-dragged-over (class added to div that is currently the drop target)
plk-dd-inprogess (class added to a dropzone when a drag operation is in progress)
```

You should always disable pointer events for a dropzone when a drag operation is in progress. Include this in your custom css:

```html
.plk-dd-inprogess > * {
    pointer-events: none;
}
```

#### Examples:

Check the [demo page](https://blazordragdrop.azurewebsites.net).

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


