namespace Plk.Blazor.DragDrop
{
    public partial class Dropzone<TItem>
    {
        /// <summary>
        /// The default state for the dropzone. All zones are in this state until OnDragStart is executed, then if the selected draggable cannot be thrown into the dropzone, it remains in this state
        /// </summary>
        private class DefaultState : DropzoneState
        {
            public DefaultState(Dropzone<TItem> dropzone) : base(dropzone)
            {
            }

            public override void OnDragEnd()
            {
                _defaultOnDragEnd();
            }

            public override bool IsDefaultState => true;

            public override void OnDragEnter(TItem item) { }
            public override void OnDragLeave() { }

            public override void OnDragStart(TItem item)
            {
                if (_dropzone.GroupService.CurrentDragItemGroupId != null)
                {
                    return;
                }

                _dropzone.ChangingStateManually = true;

                DragDropService.ShouldRender = true;

                _dropzone.DragDropService.ActiveItem = item;

                _dropzone.DragDropService.Items = _dropzone.Items;

                _dropzone.GroupService.SetSelectedGroupId(_dropzone.DropzoneGroupId);

                _dropzone.StateHasChanged();

                _dropzone.ChangingStateManually = false;

                DragDropService.ShouldRender = false;

                _dropzone.ChangeState(new DraggingFromCurrentZoneState(_dropzone));
            }

            public override void OnDrop()
            {
                DragDropService.ShouldRender = true;
            }

            public override void OnDropItemOnSpacing(int newIndex) { }

            public override void OnSpacingDragEnter(int id) { }
            public override void OnSpacingDragLeave() { }
        }
    }
}
