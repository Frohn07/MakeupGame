using MakeupGame.Core.Interfaces;
using UnityEngine;
using Zenject;

namespace MakeupGame.Controllers
{
    /// <summary>
    /// Manages the lifecycle of a single drag session.
    /// Decouples input handling from individual tool implementations.
    /// (SRP: only responsible for routing drag events to the active draggable)
    /// </summary>
    public class DragController : IInitializable
    {
        private IDraggable _activeDraggable;

        public bool IsDragging => _activeDraggable != null;

        public void Initialize() { }

        public void BeginDrag(IDraggable draggable, Vector2 screenPosition)
        {
            _activeDraggable = draggable;
            _activeDraggable.OnDragStart(screenPosition);
        }

        public void UpdateDrag(Vector2 screenPosition) =>
            _activeDraggable?.OnDrag(screenPosition);

        public void EndDrag(Vector2 screenPosition)
        {
            if (!IsDragging) return;

            _activeDraggable.OnDragEnd(screenPosition);
            _activeDraggable = null;
        }
    }
}
