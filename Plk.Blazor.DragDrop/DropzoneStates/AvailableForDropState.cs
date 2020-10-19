using System;

namespace Plk.Blazor.DragDrop
{
    public partial class Dropzone<TItem>
    {
        /// <summary>
        /// State for all zones where the current draggable can be dropped (depends on the TItem type and group of the current zone)
        /// </summary>
        private class AvailableForDropState : DropzoneState
        {
            public AvailableForDropState(Dropzone<TItem> dropzone) : base(dropzone)
            {
            }

            public override void OnDragEnd() => _defaultOnDragEnd();

            public override void OnDragEnter(TItem item)
            {
                if (_dropzone.IsItemAccepted() == false)
                {
                    return;
                }

                _dropzone.dragTargetItem = item;

                if (_dropzone.InstantReplace)
                {
                    _dropzone.PlaceItem(_dropzone.dragTargetItem, DragDropService.ActiveItem);
                }

                DragDropService.ShouldRender = true;
                _dropzone.StateHasChanged();
                DragDropService.ShouldRender = false;
            }

            public override void OnDragLeave() => _defaultOnDragLeave();

            public override void OnDragStart(TItem item) { }

            public override void OnDrop()
            {
                var activeItem = DragDropService.ActiveItem;

                DragDropService.ShouldRender = true;

                if (_dropzone.IsItemAccepted() == false)
                {
                    _dropzone.OnItemDropRejected.InvokeAsync(activeItem);
                    DragDropService.Reset();
                    return;
                }

                if (_dropzone.dragTargetItem == null) //no direct drag target
                {
                    if (_dropzone.CopyItem == null)
                    {
                        _dropzone.Items.Add(activeItem); //insert item to new zone
                        DragDropService.Items.Remove(activeItem); //remove from old zone
                    }
                    else
                    {
                        _dropzone.Items.Add(_dropzone.CopyItem(activeItem)); //insert item to new zone
                    }
                }
                else // we have a direct target
                {
                    if (_dropzone.InstantReplace == false)
                    {
                        _dropzone.Swap(_dropzone.dragTargetItem, activeItem); //swap target with active item
                    }
                }

                _dropzone.dragTargetItem = default(TItem);

                _dropzone.StateHasChanged();

                _dropzone.OnItemDrop.InvokeAsync(activeItem);
            }

            public override void OnDropItemOnSpacing(int newIndex)
            {
                var activeItem = DragDropService.ActiveItem;

                if (_dropzone.IsItemAccepted() == false)
                {
                    _dropzone.OnItemDropRejected.InvokeAsync(activeItem);
                    return;
                }

                if (DropzoneMaxItemLimitReached())
                {
                    _dropzone.OnItemDropRejectedByMaxItemLimit.InvokeAsync(activeItem);
                    return;
                }

                var oldIndex = _dropzone.Items.IndexOf(activeItem);

                if (_dropzone.CopyItem == null)
                {
                    DragDropService.Items.Remove(activeItem);
                    _dropzone.Items.Insert(newIndex, activeItem);
                }
                else
                {
                    _dropzone.Items.Insert(newIndex, _dropzone.CopyItem(activeItem));
                }

                _dropzone.OnItemDrop.InvokeAsync(activeItem);
            }

            public override void OnSpacingDragEnter(int id)
            {
                if (DropzoneMaxItemLimitReached() == false && _dropzone.IsItemAccepted())
                {
                    ChangeSpacerId(id);
                }
            }

            public override void OnSpacingDragLeave() => ChangeSpacerId(null);
        }
    }
}
