using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using NaughtyAttributes.Test;

public class UI_SpriteSwap : MonoBehaviour
{
    [SerializeField] Sprite normal;
    [SerializeField] Sprite overlined;
    [SerializeField] Sprite highlighted;

    public enum State
    {
        Normal,
        Overlined,
        Highlighted
    }

    private Image img;
    private void Start()
    {
        img = GetComponent<Image>();
    }
    public void SetSprite(State state)
    {
        switch (state)
        {
            case State.Normal:
                img.sprite = normal;
                break;
            case State.Overlined:
                img.sprite = overlined;
                break;
            case State.Highlighted:
                img.sprite = highlighted;
                break;
        }
    }
}
