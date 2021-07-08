using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Text;

namespace Plk.Blazor.DragDrop
{
    public partial class Dropzone<TItem>
    {
        private void OnDropItemOnSpacing(int newIndex)
        {
            if (!IsDropAllowed())
            {
                DragDropService.Reset();
                return;
            }

            var activeItem = DragDropService.ActiveItem;
            var oldIndex = Items.IndexOf(activeItem);
            var sameDropZone = false;
            if (oldIndex == -1) // item not present in target dropzone
            {
                if (CopyItem == null)
                {
                    DragDropService.Items.Remove(activeItem);
                }
            }
            else // same dropzone drop
            {
                sameDropZone = true;
                Items.RemoveAt(oldIndex);
                // the actual index could have shifted due to the removal
                if (newIndex > oldIndex)
                    newIndex--;
            }

            if (CopyItem == null)
            {
                Items.Insert(newIndex, activeItem);
            }
            else
            {
                // for the same zone - do not call CopyItem
                Items.Insert(newIndex, sameDropZone ? activeItem : CopyItem(activeItem));
            }

            //Operation is finished
            DragDropService.Reset();
            OnItemDrop.InvokeAsync(activeItem);
        }

        private bool IsMaxItemLimitReached()
        {
            var activeItem = DragDropService.ActiveItem;
            return (!Items.Contains(activeItem) && MaxItems.HasValue && MaxItems == Items.Count());
        }

        private string IsItemDragable(TItem item)
        {
            if (AllowsDrag == null)
                return "true";
            if (item == null)
                return "false";
            return AllowsDrag(item).ToString();
        }

        private bool IsItemAccepted(TItem dragTargetItem)
        {
            if (Accepts == null)
                return true;
            return Accepts(DragDropService.ActiveItem, dragTargetItem);
        }

        private bool IsValidItem()
        {
            return DragDropService.ActiveItem != null;
        }

        protected override bool ShouldRender()
        {
            return DragDropService.ShouldRender;
        }

        private void ForceRender(object sender, EventArgs e)
        {
            StateHasChanged();
        }

        protected override void OnInitialized()
        {
            DragDropService.StateHasChanged += ForceRender;
            base.OnInitialized();
        }

        public string CheckIfDraggable(TItem item)
        {
            if (AllowsDrag == null)
                return "";
            if (item == null)
                return "";
            if (AllowsDrag(item))
                return "";
            return "plk-dd-noselect";
        }

        public string CheckIfDragOperationIsInProgess()
        {
            var activeItem = DragDropService.ActiveItem;
            return activeItem == null ? "" : "plk-dd-inprogess";
        }

        public void OnDragEnd()
        {
            if (DragEnd != null)
            {
                DragEnd(DragDropService.ActiveItem);
            }

            DragDropService.Reset();
        //dragTargetItem = default;
        }

        public void OnDragEnter(TItem item)
        {
            var activeItem = DragDropService.ActiveItem;
            if (item.Equals(activeItem))
                return;
            if (!IsValidItem())
                return;
            if (IsMaxItemLimitReached())
                return;
            if (!IsItemAccepted(item))
                return;
            DragDropService.DragTargetItem = item;
            if (InstantReplace)
            {
                Swap(DragDropService.DragTargetItem, activeItem);
            }

            DragDropService.ShouldRender = true;
            StateHasChanged();
            DragDropService.ShouldRender = false;
        }

        public void OnDragLeave()
        {
            DragDropService.DragTargetItem = default;
            DragDropService.ShouldRender = true;
            StateHasChanged();
            DragDropService.ShouldRender = false;
        }

        public void OnDragStart(TItem item)
        {
            if (BeforeDragStart != null)
            {
                BeforeDragStart(item);
            }

            DragDropService.ShouldRender = true;
            DragDropService.ActiveItem = item;
            DragDropService.Items = Items;
            StateHasChanged();
            DragDropService.ShouldRender = false;

            if (AfterDragStart != null)
            {
                AfterDragStart(item);
            }
        }

        public string CheckIfItemIsInTransit(TItem item)
        {
            return item.Equals(DragDropService.ActiveItem) ? "plk-dd-in-transit no-pointer-events" : "";
        }

        public string CheckIfItemIsDragTarget(TItem item)
        {
            if (item.Equals(DragDropService.ActiveItem))
                return "";
            if (item.Equals(DragDropService.DragTargetItem))
            {
                return IsItemAccepted(DragDropService.DragTargetItem) ? "plk-dd-dragged-over" : "plk-dd-dragged-over-denied";
            }

            return "";
        }

        private string GetClassesForDraggable(TItem item)
        {
            var builder = new StringBuilder();
            builder.Append("plk-dd-draggable");
            if (ItemWrapperClass != null)
            {
                var itemWrapperClass = ItemWrapperClass(item);
                builder.AppendLine(" " + itemWrapperClass);
            }

            return builder.ToString();
        }

        private string GetClassesForDropzone()
        {
            var builder = new StringBuilder();
            builder.Append("plk-dd-dropzone");
            if (!String.IsNullOrEmpty(Class))
            {
                builder.AppendLine(" " + Class);
            }

            return builder.ToString();
        }

        private string GetClassesForSpacing(int spacerId)
        {
            var builder = new StringBuilder();
            builder.Append("plk-dd-spacing");
            //if active space id and item is from another dropzone -> always create insert space
            if (DragDropService.ActiveSpacerId == spacerId && Items.IndexOf(DragDropService.ActiveItem) == -1)
            {
                builder.Append(" plk-dd-spacing-dragged-over");
            } // else -> check if active space id and that it is an item that needs space
            else if (DragDropService.ActiveSpacerId == spacerId && (spacerId != Items.IndexOf(DragDropService.ActiveItem)) && (spacerId != Items.IndexOf(DragDropService.ActiveItem) + 1))
            {
                builder.Append(" plk-dd-spacing-dragged-over");
            }

            return builder.ToString();
        }

        /// <summary>
        /// Allows to pass a delegate which executes if something is dropped and decides if the item is accepted
        /// </summary>
        [Parameter]
        public Func<TItem, TItem, bool> Accepts { get; set; }

        /// <summary>
        /// Allows to pass a delegate which executes if something is dropped and decides if the item is accepted
        /// </summary>
        [Parameter]
        public Func<TItem, bool> AllowsDrag { get; set; }

        /// <summary>
        /// Allows to pass a delegate which executes if a drag operation ends
        /// </summary>
        [Parameter]
        public Action<TItem> DragEnd { get; set; }

        [Parameter]
        public Action<TItem> BeforeDragStart { get; set; }

        [Parameter]
        public Action<TItem> AfterDragStart { get; set; }

        /// <summary>
        /// Raises a callback with the dropped item as parameter in case the item can not be dropped due to the given Accept Delegate
        /// </summary>
        [Parameter]
        public EventCallback<TItem> OnItemDropRejected { get; set; }

        /// <summary>
        /// Raises a callback with the dropped item as parameter
        /// </summary>
        [Parameter]
        public EventCallback<TItem> OnItemDrop { get; set; }

        /// <summary>
        /// Raises a callback with the replaced item as parameter
        /// </summary>
        [Parameter]
        public EventCallback<TItem> OnReplacedItemDrop { get; set; }

        /// <summary>
        /// If set to true, items will we be swapped/inserted instantly.
        /// </summary>
        [Parameter]
        public bool InstantReplace { get; set; }

        /// <summary>
        /// List of items for the dropzone
        /// </summary>
        [Parameter]
        public IList<TItem> Items { get; set; }

        /// <summary>
        /// Maximum Number of items which can be dropped in this dropzone. Defaults to null which means unlimited.
        /// </summary>
        [Parameter]
        public int? MaxItems { get; set; }

        /// <summary>
        /// Raises a callback with the dropped item as parameter in case the item can not be dropped due to item limit.
        /// </summary>
        [Parameter]
        public EventCallback<TItem> OnItemDropRejectedByMaxItemLimit { get; set; }

        [Parameter]
        public RenderFragment<TItem> ChildContent { get; set; }

        /// <summary>
        /// Specifies one or more classnames for the Dropzone element.
        /// </summary>
        [Parameter]
        public string Class { get; set; }

        /// <summary>
        /// Specifies the id for the Dropzone element.
        /// </summary>
        [Parameter]
        public string Id { get; set; }

        /// <summary>
        /// Allows to pass a delegate which specifies one or more classnames for the draggable div that wraps your elements.
        /// </summary>
        [Parameter]
        public Func<TItem, string> ItemWrapperClass { get; set; }

        /// <summary>
        /// If set items dropped are copied to this dropzone and are not removed from their source.
        /// </summary>
        [Parameter]
        public Func<TItem, TItem> CopyItem { get; set; }

        private bool IsDropAllowed()
        {
            var activeItem = DragDropService.ActiveItem;
            if (!IsValidItem())
            {
                return false;
            }

            if (IsMaxItemLimitReached())
            {
                OnItemDropRejectedByMaxItemLimit.InvokeAsync(activeItem);
                return false;
            }

            if (!IsItemAccepted(DragDropService.DragTargetItem))
            {
                OnItemDropRejected.InvokeAsync(activeItem);
                return false;
            }

            return true;
        }

        private void OnDrop()
        {
            DragDropService.ShouldRender = true;
            if (!IsDropAllowed())
            {
                DragDropService.Reset();
                return;
            }

            var activeItem = DragDropService.ActiveItem;
            if (DragDropService.DragTargetItem == null) //no direct drag target
            {
                if (!Items.Contains(activeItem)) //if dragged to another dropzone
                {
                    if (CopyItem == null)
                    {
                        Items.Insert(Items.Count, activeItem); //insert item to new zone
                        DragDropService.Items.Remove(activeItem); //remove from old zone
                    }
                    else
                    {
                        Items.Insert(Items.Count, CopyItem(activeItem)); //insert item to new zone
                    }
                }
                else
                {
                //what to do here?
                }
            }
            else // we have a direct target
            {
                if (!Items.Contains(activeItem)) // if dragged to another dropzone
                {
                    if (CopyItem == null)
                    {
                        if (!InstantReplace)
                        {
                            Swap(DragDropService.DragTargetItem, activeItem); //swap target with active item
                        }
                    }
                    else
                    {
                        if (!InstantReplace)
                        {
                            Swap(DragDropService.DragTargetItem, CopyItem(activeItem)); //swap target with a copy of active item
                        }
                    }
                }
                else
                {
                    // if dragged to the same dropzone
                    if (!InstantReplace)
                    {
                        Swap(DragDropService.DragTargetItem, activeItem); //swap target with active item
                    }
                }
            }

            DragDropService.Reset();
            StateHasChanged();
            OnItemDrop.InvokeAsync(activeItem);
        }

        private void Swap(TItem draggedOverItem, TItem activeItem)
        {
            var indexDraggedOverItem = Items.IndexOf(draggedOverItem);
            var indexActiveItem = Items.IndexOf(activeItem);
            if (indexActiveItem == -1) // item is new to the dropzone
            {
                //insert into new zone
                Items.Insert(indexDraggedOverItem + 1, activeItem);
                //remove from old zone
                DragDropService.Items.Remove(activeItem);
            }
            else if (InstantReplace) //swap the items
            {
                if (indexDraggedOverItem == indexActiveItem)
                    return;
                TItem tmp = Items[indexDraggedOverItem];
                Items[indexDraggedOverItem] = Items[indexActiveItem];
                Items[indexActiveItem] = tmp;
                OnReplacedItemDrop.InvokeAsync(Items[indexActiveItem]);
            }
            else //no instant replace, just insert it after 
            {
                if (indexDraggedOverItem == indexActiveItem)
                    return;
                var tmp = Items[indexActiveItem];
                Items.RemoveAt(indexActiveItem);
                Items.Insert(indexDraggedOverItem, tmp);
            }
        }

        public void Dispose()
        {
            DragDropService.StateHasChanged -= ForceRender;
        }
    }
}