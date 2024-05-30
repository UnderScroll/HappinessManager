using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LevelLoader
{
    public class LevelLoader : MonoBehaviour
    {
        public List<Level> Levels;

        private GameManager _gameManager;
        private uint _CurrentLevelIndex;

        [HideInInspector]
        public UI_HUD UI_HUD;

        public string nextFloorname;

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
                Debug.LogWarning($"Tried to load level {nextLevelIndex} but there is no such level, trying to load next scene");
                if (nextFloorname == null || nextFloorname is "")
                {
                    Debug.LogWarning($"Tried to load next Floot but there is none");
                    return;
                }

                SceneManager.LoadScene(nextFloorname, LoadSceneMode.Single);
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

            if (level.WindDirection.magnitude > 0 && level.WindStrength > 0)
            {
                level.WindDirection.Normalize();
                Physics.gravity = new Vector3(0, -9.81f, 0) + level.WindDirection * level.WindStrength;
            }
            else
            {
                Physics.gravity = new Vector3(0, -9.81f, 0);
            }


            _gameManager.RuleManager.Reset();
            
            _gameManager.RuleManager.Rules = level.Rules;
            _gameManager.RuleManager.Initialize();

            _gameManager.RuleManager.Debug_DisplayAllRules();
            //Load Placeableblocks in HUD
            UI_HUD.blocks = level.PlaceableCellTypes.Get();
        }

        public void UnloadCurrentLevel()
        {
            _gameManager.Builder.ResetPreviewer();
        }
    }
}
