using System;
using MakeupGame.Data;
using UnityEngine;
using UnityEngine.UI;

namespace MakeupGame.UI
{
    /// <summary>
    /// Passive view for a single makeup color option.
    /// Raises an event with itself as sender so the palette can track selection.
    /// (SRP: only handles its own visual state and click forwarding)
    /// </summary>
    public class ColorItemView : MonoBehaviour
    {
        [SerializeField] private Button    _button;
        [SerializeField] private Image     _icon;
        [SerializeField] private GameObject _selectionFrame;

        public event Action<ColorItemView> OnClicked;

        public MakeupItemData Data { get; private set; }

        public void Setup(MakeupItemData data)
        {
            Data = data;
            _icon.sprite = data.Icon;
            _icon.color  = data.TintColor;
            SetSelected(false);

            _button.onClick.AddListener(HandleClick);
        }

        public void SetSelected(bool selected) =>
            _selectionFrame.SetActive(selected);

        private void HandleClick() => OnClicked?.Invoke(this);

        private void OnDestroy() => _button.onClick.RemoveListener(HandleClick);
    }
}
