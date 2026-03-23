using MakeupGame.Controllers;
using MakeupGame.Core;
using MakeupGame.Data;
using MakeupGame.Services;
using UnityEngine;
using Zenject;

namespace MakeupGame.Tools
{
    /// <summary>
    /// Powder tool. Uses a loofah/sponge applicator — no dip step by default.
    /// Add a _dipAnchor in the Inspector if the design requires a colour-dip animation.
    /// </summary>
    public class PowderTool : BaseTool
    {
        [SerializeField] private Transform _dipAnchor;   // optional; leave null for no dip

        [Inject] private IMakeupService   _makeupService;
        [Inject] private MakeupController _controller;
        [Inject] private Hand             _hand;

        public override Vector3? DipPosition =>
            _dipAnchor != null ? _dipAnchor.position : (Vector3?)null;

        private void Start()     => _controller.OnItemChosen += OnItemChosen;
        private void OnDestroy() => _controller.OnItemChosen -= OnItemChosen;

        private void OnItemChosen(MakeupItemData item)
        {
            if (item.Category != MakeupCategory.Powder) return;
            SetItem(item);
            _hand.PickUp(this);
        }

        public override void Apply() => _makeupService.SelectItem(CurrentItem);
    }
}
