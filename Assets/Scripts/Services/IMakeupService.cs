using System;
using System.Collections.Generic;
using MakeupGame.Data;

namespace MakeupGame.Services
{
    /// <summary>
    /// Tracks the player's current makeup selections and notifies listeners.
    /// (DIP: consumers depend on this abstraction, not MakeupService directly)
    /// </summary>
    public interface IMakeupService
    {
        event Action<MakeupCategory, MakeupItemData> OnItemSelected;
        event Action                                  OnMakeupReset;

        void             SelectItem(MakeupItemData item);
        MakeupItemData   GetSelectedItem(MakeupCategory category);
        IReadOnlyDictionary<MakeupCategory, MakeupItemData> GetAllSelections();
        void             ResetAll();
    }
}
