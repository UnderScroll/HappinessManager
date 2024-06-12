using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LevelLoader
{
    public class LevelLoader : MonoBehaviour
    {
        public List<Level> Levels;

        private GameManager _gameManager;
        public uint CurrentLevelIndex;

        [HideInInspector]
        public UI_HUD UI_HUD;

        public string nextFloorname;

        private void Awake()
        {
            if (!TryGetComponent(out _gameManager))
                Debug.LogError("Failted to get the Game Manager in LevelLoader");

            CurrentLevelIndex = 0;
        }
        private void Start()
        {
            UI_HUD.GoToNextFloor += LoadNextFloor;
        }
        public void ReloadLevel()
        {
            LoadLevel(CurrentLevelIndex);
        }

        public void LoadNextLevel()
        {
            uint nextLevelIndex = CurrentLevelIndex + 1;
            if (!(nextLevelIndex < Levels.Count))
            {
                Debug.LogWarning($"Tried to load level {nextLevelIndex} but there is no such level, trying to load next scene");
                if (nextFloorname == null || nextFloorname is "")
                {
                    Debug.LogWarning($"Tried to load next Floot but there is none");
                    return;
                }
                UI_HUD.DisplayNextFloorUI?.Invoke();
                return;
            }

            CurrentLevelIndex++;
            LoadLevel(CurrentLevelIndex);
        }

        public void LoadPreviousLevel()
        {
            if (CurrentLevelIndex == 0)
            {
                Debug.LogWarning($"Tried to load previous level but there is no such level");
                return;
            }

            LoadLevel(--CurrentLevelIndex);
        }

        public void LoadLevel(uint index)
        {
            LoadLevel(Levels[(int)index]);
            UI_HUD.ResetConstructMenu();
            // faire lacher le bloc selectionné au joueur
        }

        private void LoadLevel(Level level)
        {
            Debug.Log($"Loading {level.name}");

            if (CurrentLevelIndex == 0) //If loading the first level of the floor, play music and ambiance
                _gameManager.SoundManager.PLayOnFirstLevelLoaded();

            _gameManager.ResetSimulation();

            UnloadCurrentLevel();

            _gameManager.Builder.Level = Instantiate(level);

            _gameManager.Builder.Initialize();

            if (level.IsWindEnabled)
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

            _gameManager.SoundManager.PlayOnBuilding();

            //Load Placeableblocks in HUD
            if (UI_HUD != null)
                UI_HUD.blocks = level.PlaceableCellTypes.Get();
        }

        public void UnloadCurrentLevel()
        {
            _gameManager.Builder.ResetPreviewer();
        }
        private void LoadNextFloor()
        {
            Debug.Log("flqifjhlfqz");
            SceneManager.LoadScene(nextFloorname, LoadSceneMode.Single);
        }
    }
}
