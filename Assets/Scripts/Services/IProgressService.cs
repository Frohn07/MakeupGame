using System;

namespace MakeupGame.Services
{
    /// <summary>
    /// Manages level unlock state and persistence.
    /// (DIP: UI and gameplay depend on this interface, not the concrete class)
    /// </summary>
    public interface IProgressService
    {
        event Action<int> OnLevelUnlocked;

        bool IsLevelUnlocked(int levelIndex);
        void UnlockLevel(int levelIndex);
        void Save();
        void Load();
    }
}
