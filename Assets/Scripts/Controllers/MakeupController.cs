using System;
using MakeupGame.Data;
using MakeupGame.Services;
using Zenject;

namespace MakeupGame.Controllers
{
    /// <summary>
    /// Coordinates category selection and item selection.
    /// Acts as mediator between UI views and MakeupService.
    /// (SRP: only orchestrates makeup flow; SRP on service side handles state)
    /// </summary>
    public class MakeupController : IInitializable, IDisposable
    {
        public event Action<MakeupCategory> OnCategoryChanged;

        private readonly IMakeupService _makeupService;

        private MakeupCategory _currentCategory = MakeupCategory.Lipstick;

        [Inject]
        public MakeupController(IMakeupService makeupService)
        {
            _makeupService = makeupService;
        }

        public MakeupCategory CurrentCategory => _currentCategory;

        public void Initialize() { }

        public void SelectCategory(MakeupCategory category)
        {
            if (_currentCategory == category) return;

            _currentCategory = category;
            OnCategoryChanged?.Invoke(category);
        }

        public void SelectItem(MakeupItemData item) =>
            _makeupService.SelectItem(item);

        public void ResetMakeup() =>
            _makeupService.ResetAll();

        public void Dispose() { }
    }
}
