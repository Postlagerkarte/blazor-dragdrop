using Plk.Blazor.DragDrop.Services;

namespace Plk.Blazor.DragDrop
{
    public partial class Dropzone<TItem>
    {
        /// <summary>
        /// State-dependent dropzone actions
        /// </summary>
        private abstract class DropzoneState
        {
            protected readonly Dropzone<TItem> _dropzone;

            protected DropzoneGroupService GroupService => _dropzone.GroupService;

            protected DragDropService<TItem> DragDropService => _dropzone.DragDropService;

            public DropzoneState(Dropzone<TItem> dropzone)
            {
                this._dropzone = dropzone;
            }

            public abstract void OnDrop();
            public abstract void OnDragStart(TItem item);
            public abstract void OnDragEnd();
            public abstract void OnDragEnter(TItem item);
            public abstract void OnDragLeave();
            public abstract void OnDropItemOnSpacing(int newIndex);

            public abstract void OnSpacingDragEnter(int id);
            public abstract void OnSpacingDragLeave();

            public virtual bool IsDefaultState { get => false; }

            public virtual bool ShouldRender
            {
                get
                {
                    return DragDropService.ShouldRender;
                }
            }

            protected void _defaultOnDragEnd()
            {
                DragDropService.Reset();
                GroupService.UnsetSelectedGroup();
            }

            protected void _defaultOnDragLeave()
            {
                _dropzone.dragTargetItem = default(TItem);
                DragDropService.ShouldRender = true;
                _dropzone.StateHasChanged();
                DragDropService.ShouldRender = false;
            }

            protected bool DropzoneMaxItemLimitReached()
            {
                return _dropzone.MaxItems != null && _dropzone.MaxItems == _dropzone.Items.Count;
            }

            protected void ChangeSpacerId(int? id)
            {
                DragDropService.ActiveSpacerId = id;
            }
        }
    }
}
