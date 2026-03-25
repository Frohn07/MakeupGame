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
    /// Eyeshadow brush tool. Same IPreparable mechanic as BlushTool —
    /// Hand dips brush into the selected colour swatch before the player drags.
    /// </summary>
    public class EyeshadowTool : BaseTool, IPreparable
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
            if (item.Category != MakeupCategory.Eyeshadow) return;
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
