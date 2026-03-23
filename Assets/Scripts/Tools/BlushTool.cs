using MakeupGame.Controllers;
using MakeupGame.Core;
using MakeupGame.Data;
using MakeupGame.Services;
using UnityEngine;
using Zenject;

namespace MakeupGame.Tools
{
    /// <summary>
    /// Blush brush tool. Has a dip step: hand moves to _dipAnchor before waiting.
    ///
    /// Flow:
    ///   1. Player taps a blush colour → MakeupController.OnItemChosen fires.
    ///   2. Brush tip colour is updated to match the chosen item.
    ///   3. Hand picks up brush → dips into _dipAnchor → moves to WaitPosition.
    ///   4. Apply() → MakeupService → FaceApplier updates blush overlay.
    /// </summary>
    public class BlushTool : BaseTool
    {
        [SerializeField] private Transform      _dipAnchor;
        [SerializeField] private SpriteRenderer _brushTip;   // tip whose colour changes

        [Inject] private IMakeupService   _makeupService;
        [Inject] private MakeupController _controller;
        [Inject] private Hand             _hand;

        public override Vector3? DipPosition =>
            _dipAnchor != null ? _dipAnchor.position : (Vector3?)null;

        private void Start()     => _controller.OnItemChosen += OnItemChosen;
        private void OnDestroy() => _controller.OnItemChosen -= OnItemChosen;

        private void OnItemChosen(MakeupItemData item)
        {
            if (item.Category != MakeupCategory.Blush) return;
            SetItem(item);

            if (_brushTip != null)
                _brushTip.color = item.TintColor;

            _hand.PickUp(this);
        }

        public override void Apply() => _makeupService.SelectItem(CurrentItem);
    }
}
