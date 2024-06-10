using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class UI_ONOFFButton : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] Button on;
    [SerializeField] TextMeshProUGUI textOn;
    [SerializeField] Button off;
    [SerializeField] TextMeshProUGUI textOff;

    [SerializeField] GameObject slider;

    [SerializeField] Color _OnColor;
    [SerializeField] Color _OffColor;


    public void Move(bool _on)
    {
        if (_on)
        {
            slider.transform.DOMoveX(on.transform.position.x, 0.5f);
            ChangeColors(_on);
        }
        else
        {
            slider.transform.DOMoveX(off.transform.position.x, 0.5f);
            ChangeColors(_on);
        }
    }
    private void ChangeColors(bool _on)
    {
        if (_on)
        {
            textOn.color = _OnColor;
            textOff.color = _OffColor;
        }
        else
        {
            textOn.color = _OffColor;
            textOff.color= _OnColor;
        }
    }
}
