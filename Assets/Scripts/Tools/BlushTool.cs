using MakeupGame.Controllers;
using MakeupGame.Core;
using MakeupGame.Core.Interfaces;
using MakeupGame.Data;
using MakeupGame.Services;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace MakeupGame.Tools
{
    /// <summary>
    /// Blush brush tool. Implements IPreparable — Hand dips brush into the
    /// selected colour swatch before handing control back to the player.
    ///
    /// Flow:
    ///   1. Player taps a blush colour → MakeupController.OnItemChosen fires.
    ///   2. Hand picks up brush → flies to PreparePosition (colour swatch) → OnPrepared().
    ///   3. Brush tip is coloured → Hand moves to WaitPosition → player drags to face.
    ///   4. Apply() → MakeupService → FaceApplier updates the blush overlay.
    ///   5. Hand returns to shelf → OnReturned() → brush tip hidden.
    /// </summary>
    public class BlushTool : BaseTool, IPreparable
    {
        [SerializeField] private Image _brushTip;

        [Inject] private IMakeupService   _makeupService;
        [Inject] private MakeupController _controller;
        [Inject] private Hand             _hand;

        private Color _pendingTintColor;

        // ── IPreparable ────────────────────────────────────────────────────────

        public Vector3 PreparePosition { get; private set; }

        public void OnPrepared()
        {
            if (_brushTip == null) return;
            _brushTip.color = _pendingTintColor;
            _brushTip.gameObject.SetActive(true);
        }

        public void OnReturned() => _brushTip?.gameObject.SetActive(false);

        // ── Lifecycle ──────────────────────────────────────────────────────────

        private void Start()     => _controller.OnItemChosen += OnItemChosen;
        private void OnDestroy() => _controller.OnItemChosen -= OnItemChosen;

        private void OnItemChosen(MakeupItemData item, Vector3 colorWorldPosition)
        {
            if (item.Category != MakeupCategory.Blush) return;
            if (_hand.CurrentTool != null) return;
            SetItem(item);
            PreparePosition   = colorWorldPosition;
            _pendingTintColor = item.TintColor;
            _hand.PickUp(this);
        }

        // ── ITool ──────────────────────────────────────────────────────────────

        public override void Apply() => _makeupService.SelectItem(CurrentItem);
    }
}
