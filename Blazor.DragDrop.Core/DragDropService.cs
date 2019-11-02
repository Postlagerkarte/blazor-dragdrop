using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.DragDrop.Core
{
    public class DragDropService
    {
        private readonly ILogger<DragDropService> _logger;

        private Dictionary<int, DropzoneOptions> _dropzoneOptions = new Dictionary<int, DropzoneOptions>();
        private Dictionary<int, List<DraggableItem>> _dic = new Dictionary<int, List<DraggableItem>>();

        private int _idDropzoneCounter = 0;
        private int _idDraggableCounter = 0;
        
        private DraggableItem _activeItem;
        

        public DragDropService(ILogger<DragDropService> logger)
        {
            _logger = logger;
        }

        public bool EnableDebug => false;

        public DraggableItem ActiveItem {
            get => _activeItem;
            set
            {
                _logger?.LogTrace(value == null ? "Clearing active item" : $"Set draggable {value.Id} as active - item belongs to dropzone {value.DropzoneId}");
               
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
            _logger?.LogTrace($"Trying to Drop ActiveItem {ActiveItem.Id} from Dropzone {ActiveItem.DropzoneId} at Dropzone {targetDropzoneId}");

            bool acceptsDrop = AcceptsElement(targetDropzoneId);

            bool maxItemLimitReached = _dic[targetDropzoneId].Count >= _dropzoneOptions[targetDropzoneId].MaxItems;


            //if same dropzone // no drop accept // max-item limit
            if (targetDropzoneId == ActiveItem.DropzoneId ||
                !acceptsDrop ||
                maxItemLimitReached)
            {
                _logger?.LogTrace($"Drop rejected. Accept Func: {acceptsDrop} , Max-Item-Limit: {maxItemLimitReached}");

                ActiveItem = null;
                return;
            }

            _logger?.LogTrace($"Drop accepted");

            //remove active item from sourcedropzone 
            _dic[ActiveItem.DropzoneId].Remove(ActiveItem);

            //insert active into new dropzone
            var index = _dic[targetDropzoneId].Count();
            _dic[targetDropzoneId].Insert(index, ActiveItem);

            //assign new dropzone
            ActiveItem.DropzoneId = targetDropzoneId;

            //Clear active item
            ActiveItem = null;
        }

        public int GetOrderPosition(int dropzoneId, int draggableId)
        {
            var item = _dic[dropzoneId].Single(x => x.Id == draggableId);
            return _dic[dropzoneId].IndexOf(item);
        }

        public void SetActiveItem(int dropzoneId, int draggableId)
        {
            ActiveItem = _dic[dropzoneId].Single(x => x.Id == draggableId);
        }

        public void SwapOrInsert(int draggedOverId)
        {
            var targetDropzoneId = _dic.Where(v => v.Value != null).Single(x => x.Value.Any(y => y.Id == draggedOverId)).Key;

            _logger?.LogTrace($"Trying to SwapOrInsert draggable {ActiveItem.Id} with dragged over item {draggedOverId} in dropzone {targetDropzoneId}");

            bool acceptsDrop = AcceptsElement(targetDropzoneId);

            bool maxItemLimitReached = _dic[targetDropzoneId].Count >= _dropzoneOptions[targetDropzoneId].MaxItems;

            //if same dropzone // no drop accept // max-item limit
            if (!acceptsDrop || maxItemLimitReached)
            {
                _logger?.LogTrace($"SwapOrInsert rejected. Accept Func: {acceptsDrop} , Max-Item-Limit: {maxItemLimitReached}");
  
                return;
            }

            _logger?.LogTrace("SwapOrInsert accepted");

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
                var activeItemDropzone = ActiveItem.DropzoneId;

                //remove from old dropzone
                _dic[activeItemDropzone].Remove(ActiveItem);

                //assign correct dropzone
                ActiveItem.DropzoneId = draggedOverItem.DropzoneId;

                //insert into new dropzone
                _dic[ActiveItem.DropzoneId].Insert(indexForDraggedOverItem, ActiveItem);

                if(_dropzoneOptions[targetDropzoneId].AllowSwap)
                {
                    //remove draggedover item from old dropzone
                    _dic[draggedOverItem.DropzoneId].Remove(draggedOverItem);

                    //assign new dropzone to dragged over item
                    draggedOverItem.DropzoneId = activeItemDropzone;

                    //insert into other dropzone
                    _dic[activeItemDropzone].Insert(0, draggedOverItem);
                }

            }

                SupressRendering = false;

                StateHasChanged?.Invoke();

                SupressRendering = true;
        }



        public void RegisterDropzone(int dropzoneId, DropzoneOptions options)
        {
            _logger?.LogTrace($"Register dropzone {dropzoneId}");

            _dic.Add(dropzoneId, new List<DraggableItem>());
            
            _dropzoneOptions.Add(dropzoneId, options);
        }

        public void RegisterDraggableForDropzone(DraggableItem dataItem)
        {
            _logger?.LogTrace($"Register draggable {dataItem.Id} for dropzone {dataItem.DropzoneId}");

            _dic[dataItem.DropzoneId].Add(dataItem);

            StateHasChanged?.Invoke();
        }


        public bool HasDraggablesForDropzone(int dropzoneId)
        {
            var result = _dic.ContainsKey(dropzoneId) && _dic[dropzoneId]?.Count > 0;

            _logger?.LogTrace($"HasDraggablesForDropzone {dropzoneId} returns {result} with draggable count: {_dic[dropzoneId]?.Count}");

            return result;
        }

        public List<DraggableItem> GetDraggablesForDropzone(int id)
        {
            var draggables = _dic[id];

            _logger?.LogTrace($"GetDraggablesForDropzone {id} returned {draggables.Count} items");

            return draggables;
        }


        private bool AcceptsElement(int dropzoneId)
        {
            bool acceptsElement = true;

            if (_dropzoneOptions[dropzoneId].Accepts != null)
            {
                acceptsElement = (bool)_dropzoneOptions[dropzoneId].Accepts(ActiveItem.Tag);
            }

            return acceptsElement;
        }
    }
}
