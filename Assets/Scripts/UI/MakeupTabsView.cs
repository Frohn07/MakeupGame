using System;
using MakeupGame.Controllers;
using MakeupGame.Data;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace MakeupGame.UI
{
    /// <summary>
    /// Renders the category tab bar (Lipstick / Eyeshadow / Blush / Powder).
    /// Switches active/inactive sprites and delegates to MakeupController.
    /// (SRP: only visual state of tabs; zero business logic)
    /// </summary>
    public class MakeupTabsView : MonoBehaviour
    {
        [Serializable]
        private struct TabEntry
        {
            public Button        Button;
            public MakeupCategory Category;
            public Image         Icon;
            public Sprite        NormalSprite;
            public Sprite        ActiveSprite;
        }

        [SerializeField] private TabEntry[] _tabs;

        [Inject] private MakeupController _controller;

        private void Start()
        {
            foreach (var tab in _tabs)
            {
                var captured = tab;
                tab.Button.onClick.AddListener(() => _controller.SelectCategory(captured.Category));
            }

            _controller.OnCategoryChanged += UpdateTabVisuals;
            UpdateTabVisuals(_controller.CurrentCategory);
        }

        private void UpdateTabVisuals(MakeupCategory activeCategory)
        {
            foreach (var tab in _tabs)
            {
                bool isActive    = tab.Category == activeCategory;
                tab.Icon.sprite  = isActive ? tab.ActiveSprite : tab.NormalSprite;
            }
        }

        private void OnDestroy() =>
            _controller.OnCategoryChanged -= UpdateTabVisuals;
    }
}
