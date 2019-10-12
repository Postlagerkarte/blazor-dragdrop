using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.DragDrop.Core
{
    public class DragDropService
    {
        private readonly Dictionary<int, List<DraggableItem>> _dic = new Dictionary<int, List<DraggableItem>>();

        private int _idDropzoneCounter = 0;

        private int _idDraggableCounter = 0;
        private DraggableItem _activeItem;

        public bool EnableDebug => false;

        public DraggableItem ActiveItem {
            get => _activeItem;
            set
            {
                if(value != null)
                {
                    Debug.WriteLine($"Set item with {value.Id} as active - item belongs to dropzone {value.DropzoneId}");
                }
                else
                {
                    Debug.WriteLine($"Clearing active item");
                }
                
                SupressRendering = false;

                _activeItem = value;

                StateHasChanged?.Invoke();

                SupressRendering = true;
            }
        }

        public bool SupressRendering { get; set; }

        public event Action StateHasChanged;


        public int GetDropzoneId()
        {
            _idDropzoneCounter++;
            return _idDropzoneCounter;
        }

        public int GetDraggableId()
        {
            _idDraggableCounter++;
            return _idDraggableCounter;
        }

        public void DropActiveItem(int targetDropzoneId, int? otherDraggableId)
        {
            Debug.WriteLine($"DropActiveItem {ActiveItem.Id} on dropzone {targetDropzoneId} - sourcedropzone: {ActiveItem.DropzoneId}");

            //if same dropzone - do nothing - we already swapped the items;
            if (targetDropzoneId == ActiveItem.DropzoneId)
            {
                ActiveItem = null;
                return;
            }

            //remove from sourcedropzone 
            _dic[ActiveItem.DropzoneId].Remove(ActiveItem);

            //insert into new dropzone
            var index = otherDraggableId == null ? 0 : _dic[targetDropzoneId].FindIndex(x => x.Id == otherDraggableId);
            _dic[targetDropzoneId].Insert(index, ActiveItem);

            //assign new dropzone
            ActiveItem.DropzoneId = targetDropzoneId;

            ActiveItem = null;

        }

        public void SetActiveItem(int dropzoneId, int draggableId)
        {
            ActiveItem = _dic[dropzoneId].Single(x => x.Id == draggableId);
        }

        public void SwapOrInsert(int draggableId)
        {
            Debug.WriteLine($"Swap Request: draggedover id: {draggableId}");

            //find dropzone
            var dropzone = _dic.Where(v => v.Value != null).Single(x => x.Value.Any(y => y.Id == draggableId)).Value;

            //get dragged over item
            var draggedOverItem = dropzone.Single(x => x.Id == draggableId);

            var indexForDraggedOverItem = dropzone.IndexOf(draggedOverItem);

            // if same dropzone -> swap
            if (ActiveItem.DropzoneId == draggedOverItem.DropzoneId)
            {
                var indexForActiveItem = dropzone.IndexOf(ActiveItem);

                dropzone[indexForDraggedOverItem] = ActiveItem;
                dropzone[indexForActiveItem] = draggedOverItem;
            }
            else // different dropzone
            {
                //remove from old dropzone
                _dic[ActiveItem.DropzoneId].Remove(ActiveItem);

                //assign correct dropzone
                ActiveItem.DropzoneId = draggedOverItem.DropzoneId;

                //insert into new dropzone
                _dic[ActiveItem.DropzoneId].Insert(indexForDraggedOverItem, ActiveItem);

            }


            SupressRendering = false;

            StateHasChanged?.Invoke();

            SupressRendering = true;


        }

        public void RegisterDropzone(int dropzoneId)
        {
            Debug.WriteLine($"Register dropzone {dropzoneId}");

            _dic.Add(dropzoneId, new List<DraggableItem>());
        }

        public void RegisterDraggableForDropzone(DraggableItem dataItem)
        {
            Debug.WriteLine($"Register draggable {dataItem.Id} for dropzone {dataItem.DropzoneId}");

            _dic[dataItem.DropzoneId].Add(dataItem);

            StateHasChanged?.Invoke();
        }


        public bool HasDraggablesForDropzone(int dropzoneId)
        {
            var result = _dic.ContainsKey(dropzoneId) && _dic[dropzoneId]?.Count > 0;

            Debug.WriteLine($"HasDraggablesForDropzone {dropzoneId} returns {result} with draggable count: {_dic[dropzoneId]?.Count}");

            return result;
        }

        public List<DraggableItem> GetDraggablesForDropzone(int id)
        {
            var draggables = _dic[id];

            Debug.WriteLine($"GetDraggablesForDropzone {id} returned {draggables.Count} items");

            return draggables;
        }
    }
}
