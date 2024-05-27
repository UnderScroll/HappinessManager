using System.Collections.Generic;
using UnityEngine;

namespace LevelLoader
{
    public class LevelLoader : MonoBehaviour
    {
        public List<Level> Levels;

        private GameManager _gameManager;
        private uint _CurrentLevelIndex;

        private void Awake()
        {
            if (!TryGetComponent(out _gameManager))
                Debug.LogError("Failted to get the Game Manager in LevelLoader");

            _CurrentLevelIndex = 0;
        }

        public void ReloadLevel()
        {
            LoadLevel(_CurrentLevelIndex);
        }

        public void LoadNextLevel()
        {
            uint nextLevelIndex = _CurrentLevelIndex + 1;
            if (!(nextLevelIndex < Levels.Count))
            {
                Debug.LogWarning($"Tried to load level {nextLevelIndex} but there is no such level");
                return;
            }

            _CurrentLevelIndex++;
            LoadLevel(_CurrentLevelIndex);
        }

        public void LoadPreviousLevel()
        {
            if (_CurrentLevelIndex == 0)
            {
                Debug.LogWarning($"Tried to load previous level but there is no such level");
                return;
            }

            LoadLevel(--_CurrentLevelIndex);
        }

        public void LoadLevel(uint index)
        {
            LoadLevel(Levels[(int)index]);
        }

        private void LoadLevel(Level level)
        {
            Debug.Log($"Loading {level.name}");

            _gameManager.ResetSimulation();   
            
            UnloadCurrentLevel();

            _gameManager.Builder.Level = Instantiate(level);

            _gameManager.Builder.Initialize();
        }

        public void UnloadCurrentLevel()
        {
            _gameManager.Builder.ResetPreviewer();
        }
    }
}
