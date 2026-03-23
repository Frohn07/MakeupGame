using MakeupGame.Core;
using UnityEngine;
using Zenject;

namespace MakeupGame.Tools
{
    /// <summary>
    /// Cream tool — removes the acne overlay from the face.
    /// No colour selection, no dip step.
    /// Player taps the cream object directly → Hand picks it up.
    /// Wire up: Unity Button on this GameObject → OnClick → CreamTool.OnTapped().
    /// </summary>
    public class CreamTool : BaseTool
    {
        [SerializeField] private GameObject _acneOverlay;

        [Inject] private Hand _hand;

        public override Vector3? DipPosition => null;

        /// <summary>Called by Unity Button when player taps the cream.</summary>
        public void OnTapped() => _hand.PickUp(this);

        public override void Apply()
        {
            if (_acneOverlay != null)
                _acneOverlay.SetActive(false);
        }
    }
}
