using System;
using MakeupGame.Controllers;
using MakeupGame.Core;
using MakeupGame.Data;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace MakeupGame.UI
{
    /// <summary>
    /// Renders the category tab bar (Lipstick / Eyeshadow / Blush / Powder).
    /// Switches active/inactive sprites and delegates to MakeupController.
    /// Also initialises each page's ColorPaletteView with the correct category —
    /// so no manual category assignment is needed in Inspector.
    /// (SRP: visual state of tabs + one-time palette bootstrap; zero business logic)
    /// </summary>
    public class MakeupTabsView : MonoBehaviour
    {
        [Serializable]
        private struct TabEntry
        {
            public Button         Button;
            public MakeupCategory Category;
            public Image          Icon;
            public Sprite         NormalSprite;
            public Sprite         ActiveSprite;
            public GameObject     Page;
        }

        [SerializeField] private TabEntry[] _tabs;

        [Inject] private MakeupController _controller;
        [Inject] private Hand             _hand;

        private void Start()
        {
            foreach (var tab in _tabs)
            {
                var captured = tab;
                tab.Button.onClick.AddListener(() =>
                {
                    if (_hand.CurrentTool != null) return;
                    _controller.SelectCategory(captured.Category);
                });

                // Initialise the palette on this page once, right here.
                // MakeupTabsView already knows Category ↔ Page — no need to set it in Inspector.
                if (tab.Page != null)
                {
                    var palette = tab.Page.GetComponent<ColorPaletteView>();
                    palette?.Init(tab.Category);
                }
            }

            _controller.OnCategoryChanged += UpdateTabVisuals;
            UpdateTabVisuals(_controller.CurrentCategory);
        }

        private void UpdateTabVisuals(MakeupCategory activeCategory)
        {
            foreach (var tab in _tabs)
            {
                bool isActive   = tab.Category == activeCategory;
                tab.Icon.sprite = isActive ? tab.ActiveSprite : tab.NormalSprite;
                tab.Page?.SetActive(isActive);
            }
        }

        private void OnDestroy() =>
            _controller.OnCategoryChanged -= UpdateTabVisuals;
    }
}
