using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_EndLevelPanel : MonoBehaviour
{
    [HideInInspector]
    public bool win;

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
    }
}
