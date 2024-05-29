using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class UI_HUD : MonoBehaviour
{
    [Header("Player Money")]
    public int money;
    [SerializeField] TextMeshProUGUI text_money;

    [Header("Construct Menu")]
    [SerializeField]
    public List<CellType> blocks = new List<CellType>();
    [SerializeField] List<string> decos;
    [SerializeField] List<string> adhesifs;
    [SerializeField] string BlockName = "";
    [SerializeField] string BlockDescription = "";
    private CellType selected;

    [Header("Initialisation Menu")]
    [SerializeField] GameObject PrefabItem;
    [SerializeField] GameObject BlocsDisplayMenuPrefab;
    [SerializeField] Transform parentTranform;

    private GameObject actualMenu;
    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        if (_gameManager == null)
            Debug.LogError("Failed to find GameManager in UI_HUD");
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
            foreach (CellType cellType in blocks)
            {
                GameObject _go = Instantiate(PrefabItem, actualMenu.transform);
                _go.GetComponent<UI_SelectableBlock>().parentScript = this;
                _go.GetComponent<UI_SelectableBlock>().blockInfo = cellType;
            }
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

    #region Fonctionnal functions
    // region fonctionnement reel
    // TODO : dans le builder (dans le _gm), fct Select
    // GameManager.Builder.SelectBlock(indice du bloc de la liste)

    // TODO : afficher ou non le budget et le bon!
    // dans gamemanager builder level get rule budget limit, si la rule n'existe pas ne pas afficher le budget 

    public void SelectBlock(CellType _block)
    {
        for (int i = 0; i < blocks.Count; i++)
        {
            if (blocks[i].name == _block.name)
            {
                _gameManager.Builder.SelectBlock((uint)i);
                selected = blocks[i];
            }
        }
    }

    #endregion 
}
