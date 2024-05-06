using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Hoverable : MonoBehaviour
{
    [SerializeField] Sprite baseImg;
    [SerializeField] Sprite hoveredImg;

    Image image;
    private void Start()
    {
        image = GetComponent<Image>();
        image.sprite = baseImg;
    }
    public void OnOver()
    {
        image.sprite = hoveredImg;
    }
    public void OnExit()
    {
        image.sprite = baseImg;
    }
}
