using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_SelectableBlock : MonoBehaviour
{
    bool selected = false;
    public UI_HUD parentScript = null;
    public CellType blockInfo = null;

    [SerializeField] Image blockImg;
    [SerializeField] TextMeshProUGUI price;

    public void Init()
    {
        blockImg.sprite = blockInfo.BlockIcon;
        price.text = blockInfo.Price + " $";
    }
    public void OnClick()
    {
        parentScript.SelectBlock(blockInfo);
    }
}
