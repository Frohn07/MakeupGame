using MakeupGame.Core;
using UnityEngine;
using Zenject;

namespace MakeupGame.Controllers
{
    /// <summary>
    /// Reads mouse / touch input and routes drag events to DragController.
    /// Only begins a drag when the Hand signals it is ready (IsDraggingEnabled).
    ///
    /// SRP: raw input reading only — no game logic here.
    /// Attach this MonoBehaviour to any persistent GameObject in the scene
    /// (e.g. the SceneContext or a dedicated InputHandler GameObject).
    /// </summary>
    public class InputHandler : MonoBehaviour
    {
        [Inject] private DragController _dragController;
        [Inject] private Hand           _hand;

        private void Update()
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            HandleMouse();
#else
            if (Input.touchCount > 0)
                HandleTouch(Input.GetTouch(0));
#endif
        }

        // ── Mouse (editor / standalone) ────────────────────────────────────────

        private void HandleMouse()
        {
            Vector2 pos = Input.mousePosition;

            if (Input.GetMouseButtonDown(0))
                TryBeginDrag(pos);
            else if (Input.GetMouseButton(0) && _dragController.IsDragging)
                _dragController.UpdateDrag(pos);
            else if (Input.GetMouseButtonUp(0) && _dragController.IsDragging)
                _dragController.EndDrag(pos);
        }

        // ── Touch (mobile) ─────────────────────────────────────────────────────

        private void HandleTouch(Touch touch)
        {
            Vector2 pos = touch.position;

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    TryBeginDrag(pos);
                    break;
                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    if (_dragController.IsDragging)
                        _dragController.UpdateDrag(pos);
                    break;
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    if (_dragController.IsDragging)
                        _dragController.EndDrag(pos);
                    break;
            }
        }

        // ── Helpers ────────────────────────────────────────────────────────────

        private void TryBeginDrag(Vector2 screenPosition)
        {
            if (!_hand.IsDraggingEnabled) return;
            _dragController.BeginDrag(_hand, screenPosition);
        }
    }
}
