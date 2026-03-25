using MakeupGame.Core;
using MakeupGame.Services;
using MakeupGame.UI;
using UnityEngine;
using Zenject;

namespace MakeupGame.Tools
{
    /// <summary>
    /// Lipstick tool. No dip step — hand goes straight to WaitPosition.
    ///
    /// Each lipstick colour lives in the palette as its own prefab instance
    /// that has BOTH ColorItemView and LipstickTool on the same GameObject.
    /// So instead of listening to the global OnItemChosen event (which would
    /// fire on ALL instances), each LipstickTool subscribes to the click of
    /// its own ColorItemView via GetComponent — only THIS instance reacts.
    ///
    /// Flow:
    ///   1. Player taps this lipstick → ColorItemView.OnClicked fires.
    ///   2. HandleClicked: stores item, ShelfPosition = this object's world pos.
    ///   3. Hand.PickUp(this) starts the animation sequence.
    ///   4. When Hand arrives on FaceZone → Apply() is called.
    ///   5. Apply() notifies MakeupService → FaceApplier updates the lip overlay.
    /// </summary>
    public class LipstickTool : BaseTool
    {

        [Inject] private IMakeupService _makeupService;
        [Inject] private Hand           _hand;

        private ColorItemView _view;

        private void Start()
        {
            _view = GetComponent<ColorItemView>();
            if (_view != null)
                _view.OnClicked += HandleClicked;
        }

        private void OnDestroy()
        {
            if (_view != null)
                _view.OnClicked -= HandleClicked;
        }

        private void HandleClicked(ColorItemView view)
        {
            if (_hand.CurrentTool != null) return;
            SetItem(view.Data);
            _hand.PickUp(this);
        }

        public override void Apply() => _makeupService.SelectItem(CurrentItem);
    }
}
