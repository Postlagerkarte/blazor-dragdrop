using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace Blazor.DragDrop.Core
{
    public class DragDropServiceFactory
    {
        private NavigationManager _navigationManager;
        private readonly ILogger<DragDropService> _logger;
        private Dictionary<string, DragDropService> _state = new Dictionary<string, DragDropService>();

        public DragDropServiceFactory(NavigationManager navigationManager, ILogger<DragDropService> logger)
        {
            _navigationManager = navigationManager;
            _logger = logger;
            _navigationManager.LocationChanged += (s, e) =>{

                _state.Remove(_state.Keys.Single(x => x != e.Location));
            };
        }
        
        public DragDropService Get() 
        {
            if(!_state.ContainsKey(_navigationManager.Uri))
            {
                _state.Add(_navigationManager.Uri, new DragDropService(_logger));
            }

            return _state[_navigationManager.Uri];
        }

    }
}
