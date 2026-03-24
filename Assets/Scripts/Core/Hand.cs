using DG.Tweening;
using MakeupGame.Core.Interfaces;
using System;
using UnityEngine;

namespace MakeupGame.Core
{
    /// <summary>
    /// The Hand is the single draggable actor in the scene.
    ///
    /// Responsibilities (SRP):
    ///   • Animates to a tool's positions (Shelf → Dip? → Wait).
    ///   • Moves with the player's finger/mouse while IsDraggingEnabled.
    ///   • Returns the tool to the shelf after Apply().
    ///
    /// NOT responsible for:
    ///   • Deciding WHAT effect to apply — that's ITool.Apply().
    ///   • Detecting face-zone collision — that's FaceZone.
    ///   • Reading raw input — that's InputHandler.
    ///
    /// Zenject: bind via FromComponentInHierarchy().AsSingle() in GameInstaller.
    /// </summary>
    public class Hand : MonoBehaviour, IDraggable
    {
        [SerializeField] private float     _moveDuration    = 0.35f;
        [SerializeField] private Ease      _moveEase        = Ease.InOutSine;
        [SerializeField] private Transform _waitAnchor;       // chest-level wait position (same for all tools)
        [SerializeField] private Transform _defaultAnchor;    // resting position when idle
        [SerializeField] private Transform brushPoint;

        // ── State ──────────────────────────────────────────────────────────────

        public ITool CurrentTool       { get; private set; }
        public bool  IsDraggingEnabled { get; private set; }

        private Transform _toolOriginalParent;
        private Vector3   _toolOriginalLocalPos;

        /// <summary>Fired when the hand finishes the dip animation (reached colour position).</summary>
        public event Action OnDipReached;

        /// <summary>Fired when the hand has placed the tool back on the shelf (before flying to default).</summary>
        public event Action OnReturnedToShelf;

        // ── Pick-up flow ───────────────────────────────────────────────────────

        /// <summary>
        /// Starts the automatic animation sequence:
        ///   1. Move to ShelfPosition (pick-up).
        ///   2. Move to DipPosition if the tool has one (brush dip into colour).
        ///   3. Move to WaitPosition — then enable player drag.
        /// </summary>
        public void PickUp(ITool tool)
        {
            CurrentTool       = tool;
            IsDraggingEnabled = false;

            DOTween.Kill(transform);

            transform.DOMove(tool.ShelfPosition, _moveDuration)
                     .SetEase(_moveEase)
                     .OnComplete(() => AfterShelfReached(tool));
        }

        private void AfterShelfReached(ITool tool)
        {
            SetToolPosition(tool);

            if (tool.DipPosition.HasValue)
            {
                transform.DOMove(tool.DipPosition.Value, _moveDuration)
                         .SetEase(_moveEase)
                         .OnComplete(() =>
                         {
                             OnDipReached?.Invoke();
                             MoveToWait(tool);
                         });
            }
            else
            {
                MoveToWait(tool);
            }
        }

        private void MoveToWait(ITool tool)
        {
            transform.DOMove(_waitAnchor.position, _moveDuration)
                     .SetEase(_moveEase)
                     .OnComplete(() => IsDraggingEnabled = true);
        }

        // ── Return flow ────────────────────────────────────────────────────────

        /// <summary>
        /// Called by FaceZone after Apply().
        /// Animates the hand back to the shelf, then to the default resting position.
        /// </summary>
        public void ReturnToShelf()
        {
            IsDraggingEnabled = false;
            var shelfPos = CurrentTool.ShelfPosition;

            DOTween.Kill(transform);

            transform.DOMove(shelfPos, _moveDuration)
                     .SetEase(_moveEase)
                     .OnComplete(() =>
                     {
                         CurrentTool.ToolTransform.rotation = Quaternion.identity;
                         CurrentTool.ToolTransform.SetParent(_toolOriginalParent, worldPositionStays: false);
                         CurrentTool.ToolTransform.localPosition = _toolOriginalLocalPos;

                         OnReturnedToShelf?.Invoke();
                         CurrentTool = null;
                         transform.DOMove(_defaultAnchor.position, _moveDuration)
                                  .SetEase(_moveEase);
                     });
        }

        // ── IDraggable — player-controlled phase ───────────────────────────────

        public void OnDragStart(Vector2 screenPosition) => MoveTo(screenPosition);
        public void OnDrag(Vector2 screenPosition)      => MoveTo(screenPosition);

        /// <summary>
        /// Drag end itself does nothing — FaceZone's trigger detects the drop.
        /// Per TZ: releasing outside the face zone has no effect.
        /// </summary>
        public void OnDragEnd(Vector2 screenPosition) { }

        // ── Helpers ────────────────────────────────────────────────────────────

        private void MoveTo(Vector2 screenPosition)
        {
            // Hand is a UI element inside a Screen Space Overlay Canvas.
            // In that mode, transform.position == screen pixel position, so we
            // assign the raw screen coordinates directly — no camera projection needed.
            transform.position = new Vector3(screenPosition.x, screenPosition.y,
                                             transform.position.z);
        }

        private void OnDestroy() => DOTween.Kill(transform);

        private void SetToolPosition(ITool tool)
        {
            _toolOriginalParent   = tool.ToolTransform.parent;
            _toolOriginalLocalPos = tool.ToolTransform.localPosition;
            tool.ToolTransform.SetParent(transform, worldPositionStays: true);
            tool.ToolTransform.position = brushPoint.position;
            tool.ToolTransform.rotation = brushPoint.rotation;
        }
    }
}
