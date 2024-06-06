using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UI_ONOFFButton : MonoBehaviour
{
    [SerializeField] Button on;
    [SerializeField] Button off;
    [SerializeField] GameObject slider;

    public void Move(bool _on)
    {
        if(_on)
            slider.transform.DOMoveX(on.transform.position.x, 0.5f);
        else
            slider.transform.DOMoveX(off.transform.position.x, 0.5f);
    }
}
