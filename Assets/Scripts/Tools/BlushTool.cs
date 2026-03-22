using MakeupGame.Core;
using MakeupGame.Services;
using UnityEngine;
using Zenject;

namespace MakeupGame.Tools
{
    /// <summary>
    /// Applies blush by notifying the makeup service with the selected item.
    /// </summary>
    public class BlushTool : BaseTool
    {
        [Inject] private IMakeupService _makeupService;

        protected override void ApplySpecificEffect()
        {
            _makeupService.SelectItem(CurrentItem);
            Debug.Log($"[BlushTool] Applied: {CurrentItem.name}");
        }
    }
}
