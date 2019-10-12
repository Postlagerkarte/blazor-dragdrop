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
        private readonly List<int> _dropzones = new List<int>();

        private readonly Dictionary<int, List<DataItem>> _dic = new Dictionary<int, List<DataItem>>();

        public ActiveItem ActiveItem { get; set; }

        public event Action StateHasChanged;

        private int _idCounter = 0;

        public int GetId()
        {
            _idCounter++;
            Debug.WriteLine($"GetId: {_idCounter}");
            return _idCounter;
        }

        public void DropActiveItem(int targetDropzoneId, int? otherDraggableId)
        { 
            Debug.WriteLine($"DropActiveItem: target {targetDropzoneId} , source: {ActiveItem.SourceDropzoneId} , draggable-id: {ActiveItem.DraggableId}");

            //if same dropzone - do nothing;
            if (targetDropzoneId == ActiveItem.SourceDropzoneId) return;

            //remove from sourcedropzone and add to target dropzone:
            var activeDataItem = _dic[ActiveItem.SourceDropzoneId].Single(x => x.DraggableId == ActiveItem.DraggableId);
            _dic[ActiveItem.SourceDropzoneId].Remove(activeDataItem);
            var index = otherDraggableId == null ? 0 : _dic[targetDropzoneId].FindIndex(x => x.DraggableId == otherDraggableId);
            _dic[targetDropzoneId].Insert(index, (activeDataItem));

            StateHasChanged?.Invoke();
        }

        public void SetActiveItem(int sourceId, int draggableId)
        {
            var activeItem = new ActiveItem() { SourceDropzoneId = sourceId, DraggableId = draggableId };
            Debug.WriteLine($"SetActiveItem: SourceId: {sourceId}, DraggableId: {draggableId}");
            ActiveItem = activeItem;
        }

        public int GetDropzoneForDraggableId(int draggableId)
        {
            return _dic.Where(v => v.Value != null).Single(x => x.Value.Any(y => y.DraggableId == draggableId)).Key;
        }

        public void Swap(int draggableId)
        {
            Debug.WriteLine($"Swap Request: draggedover id: {draggableId}");

            //find bucket
            var bucket = _dic.Where(v=>v.Value != null).Single(x => x.Value.Any(y => y.DraggableId == draggableId)).Value;

            //swap
            var dataItemForDraggedOverItem = bucket.Single(x=>x.DraggableId == draggableId);
            var indexForDraggedOverItem = bucket.IndexOf(dataItemForDraggedOverItem);

            var dataItemForActiveItem = bucket.Single(x => x.DraggableId == ActiveItem.DraggableId);
            var indexForActiveItem = bucket.IndexOf(dataItemForActiveItem);

            bucket[indexForDraggedOverItem] = dataItemForActiveItem;
            bucket[indexForActiveItem] = dataItemForDraggedOverItem;

            StateHasChanged?.Invoke();
        }

        public void RegisterDropzone(int dropzoneId)
        {
            if (_dic.ContainsKey(dropzoneId))
            {
                return;
            }
            else
            {
                Debug.WriteLine($"Register: dropzone {dropzoneId}");
                _dic.Add(dropzoneId, new List<DataItem>());
                _dropzones.Add(dropzoneId);
            }
        }

        public void RegisterDraggableForDropzone(int dropzoneId, DataItem dataItem)
        {
            //if(_dic[dropzoneId] == null)
            //{
            //    _dic[dropzoneId] = new List<DataItem>();
            //}

            _dic[dropzoneId].Add(dataItem);

            StateHasChanged?.Invoke();
        }

         public void RegisterDraggable(int draggableId, DataItem dataItem)
        {

            if (dataItem == null)
            {
                if (_dic.ContainsKey(draggableId)) throw new InvalidOperationException("registering draggable twice - will  clear list");
                Debug.WriteLine($"Register: Init {draggableId} - Null DataItem");
                _dic.Add(draggableId, new List<DataItem>());
            }
            else
            {
                if (_dic.ContainsKey(draggableId))
                {
                    return;
                }
                else
                {
                    Debug.WriteLine($"Register: DataItem {dataItem.DraggableId} to belong to element {draggableId}");
                    _dic.Add(draggableId, new List<DataItem>() { dataItem });
                }
            }
        }

        public bool HasDraggablesForElement(int elementId)
        {
            var result = _dic.ContainsKey(elementId) && _dic[elementId]?.Count > 0;
            Debug.WriteLine($"ShouldRenderDraggable for {elementId} -> {result}");
            return result;
        }

        public List<DataItem> GetRenderFragments(int id)
        {
            Debug.WriteLine($"GetRenderFragments for {id}");
            return _dic[id];
        }
    }
}
