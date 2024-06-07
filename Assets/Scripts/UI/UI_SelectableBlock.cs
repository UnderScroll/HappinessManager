using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Compiler;

public class UI_SelectableBlock : MonoBehaviour
{
    public UI_HUD ui_hud = null;
    public CellType blockInfo = null;

    [SerializeField] Image blockImg;
    [SerializeField] TextMeshProUGUI price;

    [SerializeField] int decalOverValue;
    public Vector3 basePosition;
    public void Init()
    {
        blockImg.sprite = blockInfo.BlockIcon;
        price.text = blockInfo.Price + " $";
        basePosition = this.gameObject.transform.position;
    }
    public void OnClick()
    {
        ui_hud.SelectBlock(blockInfo);
    }

    private void Start()
    {
        ui_hud = FindObjectOfType<UI_HUD>();
        if (ui_hud == null)
            Debug.LogError("Failed to get the UI_HUD in UI_Hoverable");
    }

    public void Over()
    {
        if (!ui_hud.IsThisBlockSelected(blockInfo))
        {
            this.gameObject.transform.DOBlendableLocalMoveBy(new Vector3(0, decalOverValue, 0), 0.7f);
            ui_hud.TemporaryDescription(blockInfo, true);
        }
    }
    public void Unover()
    {
        if (!ui_hud.IsThisBlockSelected(blockInfo))
        {
            this.gameObject.transform.DOBlendableLocalMoveBy(new Vector3(0, -decalOverValue, 0), 0.5f);
            ui_hud.TemporaryDescription(blockInfo, false);
            AkSoundEngine.PostEvent("Play_UI_item_offTabHover", gameObject);
        }
    }
}
