using System;

namespace Plk.Blazor.DragDrop
{
    public partial class Dropzone<TItem>
    {
        /// <summary>
        /// The state for the zone from which the current draggable is taken
        /// </summary>
        private class DraggingFromCurrentZoneState : DropzoneState
        {
            public DraggingFromCurrentZoneState(Dropzone<TItem> dropzone) : base(dropzone)
            {
            }

            public override void OnDragEnd() => _defaultOnDragEnd();

            public override void OnDragEnter(TItem item)
            {
                if (item.Equals(DragDropService.ActiveItem))
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
                DragDropService.ShouldRender = true;

                var activeItem = DragDropService.ActiveItem;

                // Should we have this check here? Item is already in this dropzone and for now the only available action is swap
                //if (_dropzone.IsItemAccepted() == false)
                //{
                //    _dropzone.OnItemDropRejected.InvokeAsync(activeItem);
                //    DragDropService.Reset();
                //    return;
                //}

                if (_dropzone.dragTargetItem != null) // we have a direct target
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

                if (_dropzone.CopyItem == null)
                {
                    var currentIndex = _dropzone.Items.IndexOf(activeItem);
                    _dropzone.Items.Insert(newIndex, activeItem);
                    _dropzone.Items.RemoveAt(currentIndex > newIndex ? currentIndex + 1 : currentIndex);
                }
                else
                {
                    if (DropzoneMaxItemLimitReached() == false)
                    {
                        _dropzone.Items.Insert(newIndex, _dropzone.CopyItem(activeItem));
                    }
                    else
                    {
                        _dropzone.OnItemDropRejectedByMaxItemLimit.InvokeAsync(activeItem);
                        return;
                    }
                }

                _dropzone.OnItemDrop.InvokeAsync(activeItem);
            }

            public override void OnSpacingDragEnter(int id)
            {
                if (_dropzone.CopyItem == null || DropzoneMaxItemLimitReached() == false)
                {
                    ChangeSpacerId(id);
                }
            }

            public override void OnSpacingDragLeave() => ChangeSpacerId(null);
        }
    }
}
