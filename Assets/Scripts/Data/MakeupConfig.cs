using System.Collections.Generic;
using UnityEngine;

namespace MakeupGame.Data
{
    /// <summary>
    /// Central ScriptableObject config holding all makeup items per category.
    /// Bind as a single instance in GameInstaller.
    /// </summary>
    [CreateAssetMenu(fileName = "MakeupConfig", menuName = "MakeupGame/Makeup Config")]
    public class MakeupConfig : ScriptableObject
    {
        [SerializeField] private List<MakeupItemData> _lipstickItems   = new();
        [SerializeField] private List<MakeupItemData> _eyeshadowItems  = new();
        [SerializeField] private List<MakeupItemData> _blushItems      = new();
        [SerializeField] private List<MakeupItemData> _powderItems     = new();

        public IReadOnlyList<MakeupItemData> GetItemsByCategory(MakeupCategory category) =>
            category switch
            {
                MakeupCategory.Lipstick   => _lipstickItems,
                MakeupCategory.Eyeshadow  => _eyeshadowItems,
                MakeupCategory.Blush      => _blushItems,
                MakeupCategory.Powder     => _powderItems,
                _                         => new List<MakeupItemData>()
            };
    }
}
