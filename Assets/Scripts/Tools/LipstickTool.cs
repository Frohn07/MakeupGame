using MakeupGame.Controllers;
using MakeupGame.Core;
using MakeupGame.Data;
using MakeupGame.Services;
using UnityEngine;
using Zenject;

namespace MakeupGame.Tools
{
    /// <summary>
    /// Lipstick tool. No dip step — hand goes straight to WaitPosition.
    ///
    /// Flow:
    ///   1. Player taps a lipstick colour → MakeupController.OnItemChosen fires.
    ///   2. This tool catches the event (filters by Lipstick category).
    ///   3. Stores the item, asks Hand to pick up this tool.
    ///   4. When Hand arrives on FaceZone → Apply() is called.
    ///   5. Apply() notifies MakeupService → FaceApplier updates the lip overlay.
    /// </summary>
    public class LipstickTool : BaseTool
    {
        [Inject] private IMakeupService   _makeupService;
        [Inject] private MakeupController _controller;
        [Inject] private Hand             _hand;

        private void Start()  => _controller.OnItemChosen += OnItemChosen;
        private void OnDestroy() => _controller.OnItemChosen -= OnItemChosen;

        private void OnItemChosen(MakeupItemData item)
        {
            if (item.Category != MakeupCategory.Lipstick) return;
            SetItem(item);
            _hand.PickUp(this);
        }

        public override void Apply() => _makeupService.SelectItem(CurrentItem);
    }
}
