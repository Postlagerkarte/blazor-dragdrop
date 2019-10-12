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

        public DraggableItem ActiveItem { get; set; }
        
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
            Debug.WriteLine($"DropActiveItem: target {targetDropzoneId} , source: {ActiveItem.DropzoneId} , draggable-id: {ActiveItem.Id}");

            //if same dropzone - do nothing - we already swapped the items;
            if (targetDropzoneId == ActiveItem.DropzoneId) return;

            //remove from sourcedropzone and add to target dropzone:
            var activeDataItem = _dic[ActiveItem.DropzoneId].Single(x => x.Id == ActiveItem.Id);
            _dic[ActiveItem.DropzoneId].Remove(activeDataItem);
            var index = otherDraggableId == null ? 0 : _dic[targetDropzoneId].FindIndex(x => x.Id == otherDraggableId);
            _dic[targetDropzoneId].Insert(index, (activeDataItem));

            StateHasChanged?.Invoke();
        }

        public void SetActiveItem(int dropzoneId, int draggableId)
        {
            ActiveItem = _dic[dropzoneId].Single(x => x.Id == draggableId);

            Debug.WriteLine($"Set item with {ActiveItem.Id} as active - item belongs to dropzone {ActiveItem.DropzoneId}");
           
        }

        public int GetDropzoneForDraggableId(int draggableId)
        {
            return _dic.Where(v => v.Value != null).Single(x => x.Value.Any(y => y.Id == draggableId)).Key;
        }

        public void AssignNewDropzoneToActiveItem(int targetDropzoneId, int? otherDraggableId)
        {
            Debug.WriteLine($"Assign new Dropzone to Active Item");


            //remove from sourcedropzone and add to target dropzone:
            var activeDataItem = _dic[ActiveItem.DropzoneId].Single(x => x.Id == ActiveItem.Id);
            _dic[ActiveItem.DropzoneId].Remove(activeDataItem);
            var index = otherDraggableId == null ? 0 : _dic[targetDropzoneId].FindIndex(x => x.Id == otherDraggableId);
            _dic[targetDropzoneId].Insert(index, (activeDataItem));

            StateHasChanged?.Invoke();

        }

        public void Swap(int draggableId)
        {
            Debug.WriteLine($"Swap Request: draggedover id: {draggableId}");

            //find bucket
            var bucket = _dic.Where(v=>v.Value != null).Single(x => x.Value.Any(y => y.Id == draggableId)).Value;

            //swap
            var dataItemForDraggedOverItem = bucket.Single(x=>x.Id == draggableId);
            var indexForDraggedOverItem = bucket.IndexOf(dataItemForDraggedOverItem);

            var dataItemForActiveItem = bucket.Single(x => x.Id == ActiveItem.Id);
            var indexForActiveItem = bucket.IndexOf(dataItemForActiveItem);

            bucket[indexForDraggedOverItem] = dataItemForActiveItem;
            bucket[indexForActiveItem] = dataItemForDraggedOverItem;


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
