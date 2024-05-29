using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class UI_Hoverable : MonoBehaviour
{
    [SerializeField] Sprite baseImg;
    [SerializeField] Sprite hoveredImg;
    [SerializeField] int decalValue;

    public bool selected = false;

    Image image;
    private void Start()
    {
        image = GetComponent<Image>();
        image.sprite = baseImg;
    }
    public void ChangeImgOver()
    {
        image.sprite = hoveredImg;
    }
    public void OnExit()
    {
        image.sprite = baseImg;
    }
    public void MoveUp()
    {
        this.transform.DOLocalMoveY(decalValue, 0.7f);
    }
    public void MoveDown()
    {
        if (!selected)
            this.transform.DOLocalMoveY(-decalValue, 0.5f);
    }
}
