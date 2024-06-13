using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UI_BetweenStages : MonoBehaviour
{
    [SerializeField] List<UI_SpriteSwap> floors;
    [SerializeField] RectTransform background;

    private GameManager _gameManager;
    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
    }
    public void Init()
    {
        GameManager.Stage currentFloor = _gameManager.CurrentStage + 1;

        Debug.Log(currentFloor + " : " + (int)currentFloor);

        for (int i = 0; i < floors.Count; i++)
        {
            if (i < (int)currentFloor)
                floors[i].SetSprite(UI_SpriteSwap.State.Overlined);
            else if (i == (int)currentFloor)
                floors[i].SetSprite(UI_SpriteSwap.State.Highlighted);
            else
                floors[i].SetSprite(UI_SpriteSwap.State.Normal);
        }
        background.DOMove(Vector3.zero, 0.2f);

        _gameManager.SoundManager.PlayOnElevatorEnter();
    }
}
