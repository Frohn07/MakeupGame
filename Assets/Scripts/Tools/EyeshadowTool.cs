using MakeupGame.Controllers;
using MakeupGame.Core;
using MakeupGame.Data;
using MakeupGame.Services;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace MakeupGame.Tools
{
    /// <summary>
    /// Eyeshadow brush tool. Same dip mechanic as BlushTool.
    ///
    /// Flow:
    ///   1. Player taps an eyeshadow colour → MakeupController.OnItemChosen fires.
    ///   2. Brush tip colour updated.
    ///   3. Hand picks up brush → dips → waits → player drags to face → Apply().
    /// </summary>
    public class EyeshadowTool : BaseTool
    {
        [SerializeField] private Image _brushTip;  // UI Image (Canvas)

        [Inject] private IMakeupService   _makeupService;
        [Inject] private MakeupController _controller;
        [Inject] private Hand             _hand;

        private Vector3? _dipPosition;
        private Color    _pendingTintColor;

        public override Vector3? DipPosition => _dipPosition;

        private void Start()     => _controller.OnItemChosen += OnItemChosen;
        private void OnDestroy() => _controller.OnItemChosen -= OnItemChosen;

        private void OnItemChosen(MakeupItemData item, Vector3 colorWorldPosition)
        {
            if (item.Category != MakeupCategory.Eyeshadow) return;
            SetItem(item);
            _dipPosition       = colorWorldPosition;
            _pendingTintColor  = item.TintColor;

            _hand.OnDipReached       += HandleDipReached;
            _hand.OnReturnedToShelf  += HandleReturnedToShelf;
            _hand.PickUp(this);
        }

        private void HandleDipReached()
        {
            _hand.OnDipReached -= HandleDipReached;
            if (_brushTip == null) return;
            _brushTip.color = _pendingTintColor;
            _brushTip.gameObject.SetActive(true);
        }

        private void HandleReturnedToShelf()
        {
            _hand.OnReturnedToShelf -= HandleReturnedToShelf;
            _brushTip?.gameObject.SetActive(false);
        }

        public override void Apply() => _makeupService.SelectItem(CurrentItem);
    }
}
