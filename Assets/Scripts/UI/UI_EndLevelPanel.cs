using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_EndLevelPanel : MonoBehaviour
{
    [HideInInspector]
    public bool win;
    [HideInInspector]
    public UI_HUD ui_hud;

    [SerializeField] public Button nextLevel;
    [SerializeField] public Button restartLevel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] TextMeshProUGUI levelName;

    private GameManager _gameManager;
    private int currentStage = 0;
    private string currentLevel = "0";
    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        if (_gameManager == null) { Debug.LogError("EndLevel Panel : no gamemanager found"); }
        currentStage = (int)_gameManager.CurrentStage + 1;
        currentLevel = _gameManager.LevelLoader.Levels[(int)_gameManager.LevelLoader.CurrentLevelIndex].name;
        currentLevel = currentLevel.Substring(currentLevel.Length - 1);
        string stage = "Floor " + currentStage;
        string level = "Level " + currentLevel;

        levelName.text = stage + " - " + level;
    }
    public void Init()
    {
        if (!win)
        {
            text.text = "Oh no !";
            nextLevel.gameObject.SetActive(false);
            winPanel.SetActive(false);
        }
        else
        {
            text.text = "Congrats !";
            restartLevel.gameObject.SetActive(false);
            losePanel.SetActive(false);
        }

        if (ui_hud != null)
        {
            restartLevel.onClick.AddListener(ui_hud.RestartLevel);
            nextLevel.onClick.AddListener(ui_hud.NextLevel);
            AkSoundEngine.PostEvent("Play_Menu_Settings_toggleIn", gameObject);
        }
    }
}
