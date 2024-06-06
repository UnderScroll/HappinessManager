using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_TabButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [Header("Selected Attributes")]
    [SerializeField] Color backgroundColor;
    [SerializeField] Color textColor;

    [Header("Disabled Attributes")]
    [SerializeField] Color _backgroundColor;
    [SerializeField] Color _textColor;

    [Header("Properties")]
    private Image background;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] public bool Selected;
    private void Start()
    {
        background = GetComponent<Image>();
        if (Selected)
            GetComponent<Selectable>().Select();
    }
    public void OnSelect(BaseEventData eventData)
    {
        Selected = true;
        UpdateColors();
    }
    public void OnDeselect(BaseEventData eventData)
    {
        Selected = false;
        UpdateColors();
    }

    private void UpdateColors()
    {
        if (Selected)
        {
            background.color = backgroundColor;
            text.color = textColor;
        }
        else
        {
            background.color = _backgroundColor;
            text.color = _textColor;
        }
    }
}
