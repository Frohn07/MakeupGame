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
        [Inject] private IMakeupService   _makeupService;
        [Inject] private MakeupController _controller;
        [Inject] private Hand             _hand;

        // Powder has no colour palette dip — DipPosition stays null.

        private void Start()     => _controller.OnItemChosen += OnItemChosen;
        private void OnDestroy() => _controller.OnItemChosen -= OnItemChosen;

        private void OnItemChosen(MakeupItemData item, Vector3 _)
        {
            if (item.Category != MakeupCategory.Powder) return;
            SetItem(item);
            _hand.PickUp(this);
        }

        public override void Apply() => _makeupService.SelectItem(CurrentItem);
    }
}
