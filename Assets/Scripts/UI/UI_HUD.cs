using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class UI_HUD : MonoBehaviour
{
    [Header("Player Money")]
    public int money;
    [SerializeField] TextMeshProUGUI text_money;

    [Header("Construct Menu")]
    [SerializeField] List<string> blocs;
    [SerializeField] List<string> decos;
    [SerializeField] List<string> adhesifs;

    [Header("Initialisation Menu")]
    [SerializeField] GameObject PrefabItem;
    [SerializeField] GameObject BlocsDisplayMenuPrefab;
    [SerializeField] Transform parentTranform;

    private GameObject actualMenu;


    private void Start()
    {
        UpdateMoneyText();
    }

    #region Money
    private void UpdateMoneyText()
    {
        text_money.text = money.ToString() + " $";
    }
    #endregion

    #region Construct Menu Initialisation

    public void InitBlocsMenu()
    {
        if (actualMenu == null)
        {
            actualMenu = Instantiate(BlocsDisplayMenuPrefab, parentTranform);
            foreach (string bloc in blocs)
                Instantiate(PrefabItem, actualMenu.transform);
        }
        else
            CloseMenu();
    }
    public void InitDecoMenu()
    {
        if (actualMenu == null)
        {
            actualMenu = Instantiate(BlocsDisplayMenuPrefab, parentTranform);
            foreach (string deco in decos)
                Instantiate(PrefabItem, actualMenu.transform);
        }
        else
            CloseMenu();
    }

    public void InitAdhesifsMenu()
    {
        if (actualMenu == null)
        {
            actualMenu = Instantiate(BlocsDisplayMenuPrefab, parentTranform);
            foreach (string adhesif in adhesifs)
                Instantiate(PrefabItem, actualMenu.transform);
        }
        else
            CloseMenu();
    }

    public void CloseMenu()
    {
        Destroy(actualMenu);
    }

    #endregion
}
