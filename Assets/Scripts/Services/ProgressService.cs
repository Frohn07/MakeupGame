using System;
using System.Collections.Generic;
using UnityEngine;

namespace MakeupGame.Services
{
    /// <summary>
    /// Persists unlocked levels via PlayerPrefs.
    /// Level 0 is always unlocked (first level free).
    /// (SRP: only responsible for progress persistence)
    /// </summary>
    public class ProgressService : IProgressService
    {
        private const string SaveKey = "MakeupGame_UnlockedLevels";

        public event Action<int> OnLevelUnlocked;

        private readonly HashSet<int> _unlockedLevels = new();

        public ProgressService()
        {
            Load();
        }

        public bool IsLevelUnlocked(int levelIndex) =>
            levelIndex == 0 || _unlockedLevels.Contains(levelIndex);

        public void UnlockLevel(int levelIndex)
        {
            if (!_unlockedLevels.Add(levelIndex)) return;

            OnLevelUnlocked?.Invoke(levelIndex);
            Save();
        }

        public void Save()
        {
            PlayerPrefs.SetString(SaveKey, string.Join(",", _unlockedLevels));
            PlayerPrefs.Save();
        }

        public void Load()
        {
            _unlockedLevels.Clear();
            var raw = PlayerPrefs.GetString(SaveKey, string.Empty);
            if (string.IsNullOrEmpty(raw)) return;

            foreach (var part in raw.Split(','))
                if (int.TryParse(part, out var index))
                    _unlockedLevels.Add(index);
        }
    }
}
