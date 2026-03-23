using System.Collections.Generic;
using MakeupGame.Controllers;
using MakeupGame.Data;
using UnityEngine;
using Zenject;

namespace MakeupGame.UI
{
    /// <summary>
    /// Displays a scrollable list of color items for the active category.
    /// Rebuilds when the category changes; delegates selection to MakeupController.
    /// (SRP: only responsible for rendering the palette; selection logic lives in Controller)
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

        private void Start()
        {
            _controller.OnCategoryChanged += Rebuild;
            Rebuild(_controller.CurrentCategory);
        }

        private void Rebuild(MakeupCategory category)
        {
            ClearViews();

            foreach (var itemData in _config.GetItemsByCategory(category))
            {
                var view = _diContainer.InstantiatePrefabForComponent<ColorItemView>(_itemPrefab, _container);
                view.Setup(itemData);
                view.OnClicked += HandleItemClicked;
                _views.Add(view);
            }
        }

        private void HandleItemClicked(ColorItemView clickedView)
        {
            _selectedView?.SetSelected(false);
            _selectedView = clickedView;
            _selectedView.SetSelected(true);

            _controller.ChooseItem(clickedView.Data);
        }

        private void ClearViews()
        {
            foreach (var view in _views)
            {
                view.OnClicked -= HandleItemClicked;
                Destroy(view.gameObject);
            }
            _views.Clear();
            _selectedView = null;
        }

        private void OnDestroy() =>
            _controller.OnCategoryChanged -= Rebuild;
    }
}
