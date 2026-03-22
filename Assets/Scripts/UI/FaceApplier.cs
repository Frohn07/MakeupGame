using MakeupGame.Data;
using MakeupGame.Services;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace MakeupGame.UI
{
    /// <summary>
    /// Listens to IMakeupService and updates face overlay Images.
    /// Each category has its own Image layer so effects stack independently.
    /// (SRP: only responsible for face visual state)
    /// </summary>
    public class FaceApplier : MonoBehaviour
    {
        [SerializeField] private Image _lipstickOverlay;
        [SerializeField] private Image _eyeshadowOverlay;
        [SerializeField] private Image _blushOverlay;
        [SerializeField] private Image _powderOverlay;

        private IMakeupService _makeupService;

        /// <summary>
        /// Zenject injects via method — guarantees service is ready before we subscribe.
        /// </summary>
        [Inject]
        private void Construct(IMakeupService makeupService)
        {
            _makeupService = makeupService;
            _makeupService.OnItemSelected += HandleItemSelected;
            _makeupService.OnMakeupReset  += HandleReset;
        }

        private void HandleItemSelected(MakeupCategory category, MakeupItemData item)
        {
            var overlay = GetOverlay(category);
            if (overlay == null) return;

            overlay.sprite = item.ResultOverlay != null ? item.ResultOverlay : overlay.sprite;
            overlay.color  = item.TintColor;
            overlay.gameObject.SetActive(true);
        }

        private void HandleReset()
        {
            SetOverlaysActive(false);
        }

        private Image GetOverlay(MakeupCategory category) => category switch
        {
            MakeupCategory.Lipstick   => _lipstickOverlay,
            MakeupCategory.Eyeshadow  => _eyeshadowOverlay,
            MakeupCategory.Blush      => _blushOverlay,
            MakeupCategory.Powder     => _powderOverlay,
            _                         => null
        };

        private void SetOverlaysActive(bool active)
        {
            _lipstickOverlay?.gameObject.SetActive(active);
            _eyeshadowOverlay?.gameObject.SetActive(active);
            _blushOverlay?.gameObject.SetActive(active);
            _powderOverlay?.gameObject.SetActive(active);
        }

        private void OnDestroy()
        {
            if (_makeupService == null) return;
            _makeupService.OnItemSelected -= HandleItemSelected;
            _makeupService.OnMakeupReset  -= HandleReset;
        }
    }
}
