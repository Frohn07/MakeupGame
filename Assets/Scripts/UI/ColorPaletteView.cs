using System.Collections.Generic;
using MakeupGame.Controllers;
using MakeupGame.Data;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace MakeupGame.UI
{
    /// <summary>
    /// Spawns colour-item buttons for one category.
    /// Category is assigned by MakeupTabsView.Init() — no manual Inspector setup needed.
    /// Items are created once and never destroyed while the scene is alive.
    /// </summary>
    public class ColorPaletteView : MonoBehaviour
    {
        [SerializeField] private Transform     _container;
        [SerializeField] private ColorItemView _itemPrefab;

        [Inject] private MakeupController _controller;
        [Inject] private MakeupConfig     _config;
        [Inject] private DiContainer      _diContainer;

        private readonly List<ColorItemView> _views = new();
        private ColorItemView                _selectedView;

        public void Init(MakeupCategory category)
        {
            foreach (var itemData in _config.GetItemsByCategory(category))
            {
                var view = _diContainer.InstantiatePrefabForComponent<ColorItemView>(_itemPrefab, _container);
                view.Setup(itemData);
                view.OnClicked += HandleItemClicked;
                _views.Add(view);
            }

            //Invoke(nameof(RemoveGridLayout), 1.0f);
        }

        private void HandleItemClicked(ColorItemView clickedView)
        {
            _selectedView?.SetSelected(false);
            _selectedView = clickedView;
            _selectedView.SetSelected(true);

            _controller.ChooseItem(clickedView.Data, clickedView.transform.position);
        }

        private void RemoveGridLayout()
        {
            var grid = _container.GetComponent<GridLayoutGroup>();
            if (grid != null) Destroy(grid);
        }

        private void OnDestroy()
        {
            foreach (var view in _views)
                view.OnClicked -= HandleItemClicked;
        }
    }
}
