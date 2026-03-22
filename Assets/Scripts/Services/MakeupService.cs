using System;
using System.Collections.Generic;
using MakeupGame.Data;

namespace MakeupGame.Services
{
    /// <summary>
    /// Pure C# service (no MonoBehaviour). Stores selected makeup per category
    /// and broadcasts changes so any listener can react independently.
    /// (SRP: only manages selection state)
    /// </summary>
    public class MakeupService : IMakeupService
    {
        public event Action<MakeupCategory, MakeupItemData> OnItemSelected;
        public event Action                                  OnMakeupReset;

        private readonly Dictionary<MakeupCategory, MakeupItemData> _selections = new();

        public void SelectItem(MakeupItemData item)
        {
            if (item == null) return;

            _selections[item.Category] = item;
            OnItemSelected?.Invoke(item.Category, item);
        }

        public MakeupItemData GetSelectedItem(MakeupCategory category) =>
            _selections.TryGetValue(category, out var item) ? item : null;

        public IReadOnlyDictionary<MakeupCategory, MakeupItemData> GetAllSelections() =>
            _selections;

        public void ResetAll()
        {
            _selections.Clear();
            OnMakeupReset?.Invoke();
        }
    }
}
