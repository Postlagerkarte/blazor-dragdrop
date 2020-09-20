using System;

namespace Plk.Blazor.DragDrop.Services
{
    public class DropzoneGroupService
    {
        private string _currentDragItemGroupId = null;

        /// <summary>
        /// Current drag item dropzone group id
        /// </summary>
        public string CurrentDragItemGroupId
        {
            get => _currentDragItemGroupId;
        }

        /// <summary>
        /// Sets current drag item dropzone group id, invokes OnGroupSet
        /// </summary>
        /// <param name="id"></param>
        public void SetSelectedGroupId(string id)
        {
            _currentDragItemGroupId = id;
            OnGroupSet(id);
        }

        /// <summary>
        /// Sets current drag item dropzone group id to null, invokes OnGroupUnset
        /// </summary>
        public void UnsetSelectedGroup()
        {
            var unselectedGroupId = _currentDragItemGroupId;
            _currentDragItemGroupId = null;
            OnGroupUnset(unselectedGroupId);
        }

        /// <summary>
        /// Invokes when the group id is set, the parameter is the id of the selected group
        /// </summary>
        public event Action<string> OnGroupSet;


        /// <summary>
        /// Invokes when the group id is unset, the parameter is the id of the last selected group
        /// </summary>
        public event Action<string> OnGroupUnset;
    }
}
