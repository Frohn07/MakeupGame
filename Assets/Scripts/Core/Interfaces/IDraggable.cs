using UnityEngine;

namespace MakeupGame.Core.Interfaces
{
    /// <summary>
    /// Defines drag-and-drop behaviour. (ISP: separated from ITool)
    /// </summary>
    public interface IDraggable
    {
        void OnDragStart(Vector2 screenPosition);
        void OnDrag(Vector2 screenPosition);
        void OnDragEnd(Vector2 screenPosition);
    }
}
