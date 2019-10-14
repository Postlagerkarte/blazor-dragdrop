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
        private Dictionary<int, DropzoneOptions> _options = new Dictionary<int, DropzoneOptions>();
        private Dictionary<int, List<DraggableItem>> _dic = new Dictionary<int, List<DraggableItem>>();

        private int _idDropzoneCounter = 0;

        private int _idDraggableCounter = 0;
        private DraggableItem _activeItem;


        private bool AcceptsElement(int dropzoneId)
        {
            bool acceptsElement = true;

            if (_options[dropzoneId].Accepts != null && ActiveItem.Tag != null)
            {
                acceptsElement = (bool)_options[dropzoneId].Accepts(ActiveItem.Tag);
            }

            return acceptsElement;
        }

        public DragDropService()
        {

        }

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

        public void DropActiveItem(int targetDropzoneId)
        {
            bool acceptsDrop = AcceptsElement(targetDropzoneId);

            Debug.WriteLine($"Accept drop func returns {acceptsDrop}");

            Debug.WriteLine($"DropActiveItem {ActiveItem.Id} on dropzone {targetDropzoneId} - sourcedropzone: {ActiveItem.DropzoneId}");

            //if same dropzone // no drop accept // max-item limit
            if (targetDropzoneId == ActiveItem.DropzoneId ||
                !acceptsDrop ||   
                _dic[targetDropzoneId].Count >= _options[targetDropzoneId].MaxItems)
            {
                ActiveItem = null;
                return;
            }

            //remove from sourcedropzone 
            _dic[ActiveItem.DropzoneId].Remove(ActiveItem);

            //insert into new dropzone
            var index = _dic[targetDropzoneId].Count();
            _dic[targetDropzoneId].Insert(index, ActiveItem);

            //assign new dropzone
            ActiveItem.DropzoneId = targetDropzoneId;

            ActiveItem = null;

        }

        public void SetActiveItem(int dropzoneId, int draggableId)
        {
            ActiveItem = _dic[dropzoneId].Single(x => x.Id == draggableId);
        }

        public void SwapOrInsert(int draggedOverId)
        {

            var dropzoneId = _dic.Where(v => v.Value != null).Single(x => x.Value.Any(y => y.Id == draggedOverId)).Key;

            Debug.WriteLine($"Accept element func returns {AcceptsElement(dropzoneId)}");

            Debug.WriteLine($"Swap Request - Active item {ActiveItem.Id} was dragged over item with id {draggedOverId}");

            // accept-element? // max-items?
            if (!AcceptsElement(dropzoneId) ||
                _dic[dropzoneId].Count >= _options[dropzoneId].MaxItems)
            {
                return;
            }

            //find dropzone
            var dropzone = _dic.Where(v => v.Value != null).Single(x => x.Value.Any(y => y.Id == draggedOverId)).Value;

            //get dragged over item
            var draggedOverItem = dropzone.Single(x => x.Id == draggedOverId);

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

        public void RegisterDropzone(int dropzoneId, DropzoneOptions options)
        {
            Debug.WriteLine($"Register dropzone {dropzoneId}");

            _dic.Add(dropzoneId, new List<DraggableItem>());
            _options.Add(dropzoneId, options);
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
