using MakeupGame.Controllers;
using MakeupGame.Core;
using MakeupGame.Data;
using MakeupGame.Services;
using UnityEngine;
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
        [SerializeField] private Transform      _dipAnchor;
        [SerializeField] private SpriteRenderer _brushTip;

        [Inject] private IMakeupService   _makeupService;
        [Inject] private MakeupController _controller;
        [Inject] private Hand             _hand;

        public override Vector3? DipPosition =>
            _dipAnchor != null ? _dipAnchor.position : (Vector3?)null;

        private void Start()     => _controller.OnItemChosen += OnItemChosen;
        private void OnDestroy() => _controller.OnItemChosen -= OnItemChosen;

        private void OnItemChosen(MakeupItemData item)
        {
            if (item.Category != MakeupCategory.Eyeshadow) return;
            SetItem(item);

            if (_brushTip != null)
                _brushTip.color = item.TintColor;

            _hand.PickUp(this);
        }

        public override void Apply() => _makeupService.SelectItem(CurrentItem);
    }
}
