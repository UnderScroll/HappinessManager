using System.Collections.Generic;
using UnityEngine;

namespace LevelLoader
{
    public class LevelLoader : MonoBehaviour
    {
        public List<Level> Levels;

        private GameManager _gameManager;
        private uint _CurrentLevelIndex;

        [HideInInspector]
        public UI_HUD UI_HUD;

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
            if (_CurrentLevelIndex==0)
            {
                AkSoundEngine.PostEvent("Play_Music_cafeteria", gameObject);
            }
            else
            {
                AkSoundEngine.PostEvent("Play_Music_SetSwitch_build", gameObject);
            }
            
            AkSoundEngine.PostEvent("Play_Amb_boss", gameObject);
            _gameManager.ResetSimulation();   
            
            UnloadCurrentLevel();

            _gameManager.Builder.Level = Instantiate(level);

            _gameManager.Builder.Initialize();

            if (level.WindDirection.magnitude > 0 && level.WindStrength > 0)
            {
                level.WindDirection.Normalize();
                Physics.gravity = new Vector3(0, -9.81f, 0) + level.WindDirection * level.WindStrength;
            }
            else
            {
                Physics.gravity = new Vector3(0, -9.81f, 0);
            }

            //Load Placeableblocks in HUD
            UI_HUD.blocks = level.PlaceableCellTypes.Get();
        }

        public void UnloadCurrentLevel()
        {
            _gameManager.Builder.ResetPreviewer();
        }
    }
}
