using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Blazor.DragDrop.Core
{
    public class DragDropServiceFactory
    {
        private NavigationManager _navigationManager;

        private Dictionary<string, DragDropService> _state = new Dictionary<string, DragDropService>();

        public DragDropServiceFactory(NavigationManager navigationManager)
        {
            _navigationManager = navigationManager;

            _navigationManager.LocationChanged += (s, e) =>{

                _state.Remove(_state.Keys.Single(x => x != e.Location));
            };
        }
        
        public DragDropService Get() 
        {
            if(!_state.ContainsKey(_navigationManager.Uri))
            {
                _state.Add(_navigationManager.Uri, new DragDropService());
            }

            return _state[_navigationManager.Uri];
        }

    }
}
