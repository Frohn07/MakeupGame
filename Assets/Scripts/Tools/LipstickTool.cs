using MakeupGame.Core;
using MakeupGame.Services;
using UnityEngine;
using Zenject;

namespace MakeupGame.Tools
{
    /// <summary>
    /// Applies lipstick by notifying the makeup service with the selected item.
    /// Visual feedback (sprite swap / tint) is handled by an IMakeupApplier on the face.
    /// </summary>
    public class LipstickTool : BaseTool
    {
        [Inject] private IMakeupService _makeupService;

        protected override void ApplySpecificEffect()
        {
            _makeupService.SelectItem(CurrentItem);
            Debug.Log($"[LipstickTool] Applied: {CurrentItem.name}");
        }
    }
}
