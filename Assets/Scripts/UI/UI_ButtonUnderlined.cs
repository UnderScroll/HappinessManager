using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using UnityEngine.UI;

public class UI_ButtonUnderlined : MonoBehaviour
{
    UnityAction OnPop;
    UnityAction OnUnPop;

    private void Start()
    {
        OnPop += Pop;
        OnUnPop += UnPop;
    }
    public void Pop(bool _active)
    {
        if (_active)
            OnPop?.Invoke();
        else
            OnUnPop?.Invoke();
    }
    private void Pop()
    {
        this.transform.DOScale(1.2f, 0.5f);
        // sound
    }
    private void UnPop()
    {
        this.transform.DOScale(1.0f, 0.5f);
        // sound

    }
}
