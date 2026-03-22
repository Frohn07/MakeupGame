using MakeupGame.Core;
using MakeupGame.Services;
using UnityEngine;
using Zenject;

namespace MakeupGame.Tools
{
    /// <summary>
    /// Applies eyeshadow by notifying the makeup service with the selected item.
    /// </summary>
    public class EyeshadowTool : BaseTool
    {
        [Inject] private IMakeupService _makeupService;

        protected override void ApplySpecificEffect()
        {
            _makeupService.SelectItem(CurrentItem);
            Debug.Log($"[EyeshadowTool] Applied: {CurrentItem.name}");
        }
    }
}
