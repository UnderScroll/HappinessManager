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
    [SerializeField] TextMeshProUGUI text;

    public void Init()
    {
        if (!win)
        {
            text.text = "Level failed !";
            nextLevel.gameObject.SetActive(false);
        }

        if (ui_hud != null)
        {
            restartLevel.onClick.AddListener(ui_hud.RestartLevel);
            nextLevel.onClick.AddListener(ui_hud.NextLevel);
        }
    }
}
