# blazor-dragdrop
Drag and Drop Library for Blazor

[![Build Status](https://postlagerkarte.visualstudio.com/DragDrop/_apis/build/status/Postlagerkarte.blazor-dragdrop?branchName=master)](https://postlagerkarte.visualstudio.com/DragDrop/_build/latest?definitionId=2&branchName=master)

Demo:

https://blazordragdrop.azurewebsites.net/

Create a draggable list:

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
