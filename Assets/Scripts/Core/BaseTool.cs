using MakeupGame.Core.Interfaces;
using MakeupGame.Data;
using UnityEngine;

namespace MakeupGame.Core
{
    /// <summary>
    /// Base class for all makeup tools.
    /// Implements shared drag movement; subclasses override ApplySpecificEffect.
    /// (OCP: open for extension via override, closed for modification of drag logic)
    /// (LSP: any BaseTool subclass can substitute wherever BaseTool is expected)
    /// </summary>
    public abstract class BaseTool : MonoBehaviour, ITool, IDraggable
    {
        [SerializeField] protected Transform toolTransform;

        protected MakeupItemData CurrentItem { get; private set; }

        // ── ITool ──────────────────────────────────────────────────────────────

        public bool CanInteract => CurrentItem != null;

        public virtual void OnToolSelected()  => gameObject.SetActive(true);
        public virtual void OnToolDeselected() => gameObject.SetActive(false);

        // ── IDraggable ─────────────────────────────────────────────────────────

        public virtual void OnDragStart(Vector2 screenPosition) =>
            MoveToScreen(screenPosition);

        public virtual void OnDrag(Vector2 screenPosition) =>
            MoveToScreen(screenPosition);

        public virtual void OnDragEnd(Vector2 screenPosition)
        {
            MoveToScreen(screenPosition);
            ApplyMakeupEffect();
        }

        // ── Public API ─────────────────────────────────────────────────────────

        public void SetItem(MakeupItemData item) => CurrentItem = item;

        public void ApplyMakeupEffect()
        {
            if (!CanInteract) return;
            ApplySpecificEffect();
        }

        // ── Protected ──────────────────────────────────────────────────────────

        /// <summary>
        /// Each subclass defines how its makeup effect is applied.
        /// </summary>
        protected abstract void ApplySpecificEffect();

        private void MoveToScreen(Vector2 screenPosition)
        {
            var worldPos = Camera.main.ScreenToWorldPoint(screenPosition);
            toolTransform.position = new Vector3(worldPos.x, worldPos.y, toolTransform.position.z);
        }
    }
}
