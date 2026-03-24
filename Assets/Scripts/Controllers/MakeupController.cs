using System;
using MakeupGame.Data;
using MakeupGame.Services;
using UnityEngine;
using Zenject;

namespace MakeupGame.Controllers
{
    /// <summary>
    /// Mediator between UI and the rest of the system.
    ///
    /// Two distinct events:
    ///   OnItemChosen  — player tapped a colour in the palette; tools react by
    ///                   calling Hand.PickUp(). Does NOT apply the effect yet.
    ///   OnCategoryChanged — tab switched; palette rebuilds itself.
    ///
    /// SRP: orchestration only — no visual logic, no service calls from here
    ///      (Apply() in each tool calls the service at the right moment).
    /// DIP: depends on IMakeupService abstraction.
    /// </summary>
    public class MakeupController : IInitializable, IDisposable
    {
        public event Action<MakeupCategory> OnCategoryChanged;

        /// <summary>
        /// Fired when the player selects a colour from the palette.
        /// Tools subscribe to this, filter by category, and start the pick-up flow.
        /// Vector3 is the world position of the tapped colour swatch — brush tools
        /// use it as the dynamic dip target instead of a fixed anchor.
        /// </summary>
        public event Action<MakeupItemData, Vector3> OnItemChosen;

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

        /// <summary>
        /// Called by ColorPaletteView when the player taps a colour.
        /// dipWorldPosition — world position of the tapped swatch for brush dip.
        /// Notifies tools — does NOT write to MakeupService yet.
        /// </summary>
        public void ChooseItem(MakeupItemData item, Vector3 dipWorldPosition) =>
            OnItemChosen?.Invoke(item, dipWorldPosition);

        public void ResetMakeup() =>
            _makeupService.ResetAll();

        public void Dispose() { }
    }
}
