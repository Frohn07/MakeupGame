using UnityEngine;

namespace MakeupGame.Data
{

    [CreateAssetMenu(fileName = "MakeupItem", menuName = "MakeupGame/Makeup Item")]
    public class MakeupItemData : ScriptableObject
    {
        [SerializeField] private MakeupCategory _category;
        [SerializeField] private Sprite _icon;
        [SerializeField] private Sprite _resultOverlay;
        [SerializeField] private Color  _tintColor = Color.white;
        [SerializeField] private bool   _isLockedByDefault;

        public MakeupCategory Category => _category;
        public Sprite Icon => _icon;
        public Sprite ResultOverlay => _resultOverlay;
        public Color  TintColor  => _tintColor;
        public bool   IsLockedByDefault => _isLockedByDefault;
    }
}
