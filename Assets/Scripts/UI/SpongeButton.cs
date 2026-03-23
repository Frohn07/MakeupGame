using MakeupGame.Services;
using UnityEngine;
using Zenject;

namespace MakeupGame.UI
{
    /// <summary>
    /// The blue sponge button.
    /// Per TZ: tapping it resets all makeup immediately — no animation, no drag.
    ///
    /// SRP: this class has one job — call ResetAll() when tapped.
    /// Wire up via Unity Button component → OnClick → SpongeButton.OnClick().
    /// </summary>
    public class SpongeButton : MonoBehaviour
    {
        [Inject] private IMakeupService _makeupService;

        /// <summary>Assigned to the Unity Button's OnClick event in the Inspector.</summary>
        public void OnClick() => _makeupService.ResetAll();
    }
}
