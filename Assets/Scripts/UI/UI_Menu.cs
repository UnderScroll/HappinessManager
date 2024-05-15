using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_Menu : MonoBehaviour
{
    [SerializeField] Image imageFade;
    [SerializeField] Image splashScreen;
    public UnityEvent OnButtonClicked;
    public UnityEvent OnApplicationStarted;
    public UnityEvent OnQuittingGame;

    void Start()
    {
        // Binding events
        OnApplicationStarted.AddListener(OnApplicationStart);
        OnButtonClicked.AddListener(ButtonClicked);
        OnQuittingGame.AddListener(QuittingGame);

        // Start
        OnApplicationStarted?.Invoke();
    }

    public void StartGame()
    {
        Debug.Log("load scene");
    }
    public void QuitGame()
    {
        OnQuittingGame?.Invoke();
        imageFade.DOFade(1, 1).OnComplete(CloseApp);
    }

    public void FadeIn()
    {
        imageFade.DOFade(1, 1);
    }
    public void FadeOut()
    {
        imageFade.DOFade(0, 1);
    }

    #region Private Methods
    private void CloseApp()
    {
        Application.Quit();
    }
    #endregion

    #region EVENTS
    private void OnApplicationStart()
    {
        Debug.Log("son lancement du jeu");
        imageFade.DOFade(1, 1);
    }
    private void ButtonClicked()
    {
        Debug.Log("son button clicked");
    }
    private void QuittingGame()
    {
        Debug.Log("son jeu qui se ferme");
    }
    #endregion

}
