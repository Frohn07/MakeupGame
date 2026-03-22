using MakeupGame.Core;
using MakeupGame.Services;
using UnityEngine;
using Zenject;

namespace MakeupGame.Tools
{
    /// <summary>
    /// Applies powder by notifying the makeup service with the selected item.
    /// </summary>
    public class PowderTool : BaseTool
    {
        [Inject] private IMakeupService _makeupService;

        protected override void ApplySpecificEffect()
        {
            _makeupService.SelectItem(CurrentItem);
            Debug.Log($"[PowderTool] Applied: {CurrentItem.name}");
        }
    }
}
