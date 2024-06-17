using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace LevelLoader
{
    public class LevelLoader : MonoBehaviour
    {
        public List<Level> Levels;
        public List<GameObject> AdditionamPrefabs;
        public List<GameObject> AdditionamStructurePrefabs;
        public List<Material> SkyBoxMat;

        private GameManager _gameManager;
        public uint CurrentLevelIndex;

        [HideInInspector]
        public UI_HUD UI_HUD;

        public string nextFloorName;

        private GameObject _additionalPrefabInstance;
        private GameObject _additionalStructurePrefabInstance;

        public UnityEvent<string> OnLoadLevel;

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
                if (nextFloorName == null || nextFloorName is "")
                {
                    Debug.LogWarning($"Tried to load next Floor but there is none");
                    return;
                }
                if (nextFloorName == "MainMenu")
                    _gameManager.OnLastLevelSucces.Invoke();
                else
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
            UI_HUD.ReloadBudgetUI();
        }

        private void LoadLevel(Level level)
        {
            Debug.Log($"Loading {level.name}");
            OnLoadLevel?.Invoke(level.name);

            if (CurrentLevelIndex == 0) //If loading the first level of the floor, play music and ambiance
                _gameManager.SoundManager.PlayOnFirstLevelLoaded();

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

            //Load Additional Prefab
            if (_additionalPrefabInstance != null)
                Destroy(_additionalPrefabInstance);

            if (_additionalStructurePrefabInstance != null)
                Destroy(_additionalStructurePrefabInstance);

            if (AdditionamPrefabs.Count > (int)CurrentLevelIndex && AdditionamPrefabs[(int)CurrentLevelIndex] != null)
                _additionalPrefabInstance = Instantiate(AdditionamPrefabs[(int)CurrentLevelIndex], Camera.main.transform);

            if (AdditionamStructurePrefabs.Count > (int)CurrentLevelIndex && AdditionamStructurePrefabs[(int)CurrentLevelIndex] != null)
                _additionalStructurePrefabInstance = Instantiate(AdditionamStructurePrefabs[(int)CurrentLevelIndex], _gameManager.StructureOrigin);

            if (SkyBoxMat.Count > (int)CurrentLevelIndex && SkyBoxMat[(int)CurrentLevelIndex] != null)
            {
                RenderSettings.skybox = SkyBoxMat[(int)CurrentLevelIndex];
                DynamicGI.UpdateEnvironment();
            }

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
            SceneManager.LoadScene(nextFloorName, LoadSceneMode.Single);
        }
    }
}
