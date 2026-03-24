using MakeupGame.Core.Interfaces;
using MakeupGame.Data;
using UnityEngine;

namespace MakeupGame.Core
{
    /// <summary>
    /// Base class for colour-based makeup tools (lipstick, blush, eyeshadow, powder).
    ///
    /// Responsibilities (SRP):
    ///   • Exposes the three world-space positions the Hand needs.
    ///   • Holds the currently selected MakeupItemData.
    ///   • Delegates the actual visual effect to each subclass via Apply().
    ///
    /// NOT responsible for:
    ///   • Movement — that belongs to Hand.
    ///   • Drag input — that belongs to InputHandler / DragController.
    ///   • Show/hide — tools are always present in the scene.
    ///
    /// SOLID:
    ///   OCP — extend by subclassing and overriding Apply() / DipPosition.
    ///   LSP — any subclass can substitute BaseTool wherever ITool is expected.
    ///   DIP — consumers depend on ITool, not on concrete tool types.
    /// </summary>
    public abstract class BaseTool : MonoBehaviour, ITool
    {
        [SerializeField] private Transform _shelfAnchor;

        // ── ITool ──────────────────────────────────────────────────────────────

        public virtual Vector3 ShelfPosition => _shelfAnchor.position;
        public Transform       ToolTransform => transform;

        /// <summary>
        /// Brush tools override this to return the dip anchor position.
        /// Returns null by default (no dip step).
        /// </summary>
        public virtual Vector3? DipPosition => null;

        public abstract void Apply();

        // ── Item data ──────────────────────────────────────────────────────────

        protected MakeupItemData CurrentItem { get; private set; }

        /// <summary>
        /// Called by the tool itself (via MakeupController.OnItemChosen handler)
        /// before asking the Hand to PickUp.
        /// </summary>
        public virtual void SetItem(MakeupItemData item) => CurrentItem = item;
    }
}
