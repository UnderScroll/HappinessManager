using DG.Tweening;
using DG.Tweening.Core.Easing;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public UnityEvent OnIntroEnded;

    [Header("IntroScene")]
    [SerializeField] bool skipIntro;
    [SerializeField] private RawImage _gameLogo;
    [SerializeField] private RawImage _cnamLogo;
    [SerializeField] private RawImage _magelisLogo;
    [SerializeField] private RawImage _p20Logo;
    [SerializeField] private RawImage _wwiseLogo;

    [Header("Settings")]
    [SerializeField] private float _secondsBeforeFirstLogo = 1.0f;
    [SerializeField] private float _secondsBetweenLogos = 1.0f;
    [SerializeField] private float _secondsBeforeStartGame = 1.0f;


    [Header("Game Logo")]
    [SerializeField] private float _secondsFadeInDurationGameLogo = 1.0f;
    [SerializeField] private float _secondsBeforeFadeOutGameLogo = 1.0f;
    [SerializeField] private float _secondsFadeOutDurationGameLogo = 1.0f;
    [SerializeField] private Ease _easeFadeInGameLogo = Ease.Linear;
    [SerializeField] private Ease _easeFadeOutGameLogo = Ease.Linear;
    [SerializeField] private float _secondsFadeInDurationGameTitle = 1.0f;
    [SerializeField] private float _secondsFadeOutDurationGameTitle = 1.0f;

    [Header("Cnam Logo")]
    [SerializeField] private float _secondsBeforeFadeOutCnamAndMagelisLogo = 1.0f;
    [SerializeField] private float _secondsFadeInDurationCnamLogo = 1.0f;
    [SerializeField] private float _secondsFadeOutDurationCnamLogo = 1.0f;
    [SerializeField] private Ease _easeFadeInCnamLogo = Ease.Linear;
    [SerializeField] private Ease _easeFadeOutCnamLogo = Ease.Linear;

    [Header("Magelis Logo")]
    [SerializeField] private float _secondsFadeInDurationMagelisLogo = 1.0f;
    [SerializeField] private float _secondsFadeOutDurationMagelisLogo = 1.0f;
    [SerializeField] private Ease _easeFadeInMagelisLogo = Ease.Linear;
    [SerializeField] private Ease _easeFadeOutMagelisLogo = Ease.Linear;

    [Header("P20 Logo")]
    [SerializeField] private float _secondsFadeInDurationP20Logo = 1.0f;
    [SerializeField] private float _secondsBeforeFadeOutP20Logo = 1.0f;
    [SerializeField] private float _secondsFadeOutDurationP20Logo = 1.0f;
    [SerializeField] private Ease _easeFadeInP20Logo = Ease.Linear;
    [SerializeField] private Ease _easeFadeOutP20Logo = Ease.Linear;

    [Header("Wwise Logo")]
    [SerializeField] private float _secondsFadeInDurationWwiseLogo = 1.0f;
    [SerializeField] private float _secondsBeforeFadeOutWwiseLogo = 1.0f;
    [SerializeField] private float _secondsFadeOutDurationWwiseLogo = 1.0f;
    [SerializeField] private Ease _easeFadeInWwiseLogo = Ease.Linear;
    [SerializeField] private Ease _easeFadeOutWwiseLogo = Ease.Linear;


    private IEnumerator Start()
    {

        // Binding events
        OnApplicationStarted.AddListener(OnApplicationStart);
        OnButtonClicked.AddListener(ButtonClicked);
        OnQuittingGame.AddListener(QuittingGame);
        OnIntroEnded.AddListener(IntroEnded);

        // Start
        OnApplicationStarted?.Invoke();

        // INTRO SCENE
        if (!skipIntro)
        {
            // Wwise
            yield return new WaitForSeconds(_secondsBeforeFirstLogo);
            var tweenFadeInWwiseLogo = _wwiseLogo.DOFade(1.0f, _secondsFadeInDurationWwiseLogo).SetEase(_easeFadeInWwiseLogo);
            yield return tweenFadeInWwiseLogo.WaitForCompletion();
            yield return new WaitForSeconds(_secondsBeforeFadeOutWwiseLogo);
            var tweenFadeOutWwiseLogo = _wwiseLogo.DOFade(0.0f, _secondsFadeOutDurationWwiseLogo).SetEase(_easeFadeOutWwiseLogo);
            yield return tweenFadeOutWwiseLogo.WaitForCompletion();

            yield return new WaitForSeconds(_secondsBetweenLogos);

            // CNAM AND MAGELIS LOGOS
            var tweenFadeInCnamLogo = _cnamLogo.DOFade(1.0f, _secondsFadeInDurationCnamLogo).SetEase(_easeFadeInCnamLogo);
            var tweenFadeInMagelisLogo = _magelisLogo.DOFade(1.0f, _secondsFadeInDurationMagelisLogo).SetEase(_easeFadeInMagelisLogo);
            yield return tweenFadeInCnamLogo.WaitForCompletion();
            yield return tweenFadeInMagelisLogo.WaitForCompletion();
            yield return new WaitForSeconds(_secondsBeforeFadeOutCnamAndMagelisLogo);
            var tweenFadeOutCnamLogo = _cnamLogo.DOFade(0.0f, _secondsFadeOutDurationCnamLogo).SetEase(_easeFadeOutCnamLogo);
            var tweenFadeOutMagelisLogo = _magelisLogo.DOFade(0.0f, _secondsFadeOutDurationMagelisLogo).SetEase(_easeFadeOutMagelisLogo);
            yield return tweenFadeOutCnamLogo.WaitForCompletion();
            yield return tweenFadeOutMagelisLogo.WaitForCompletion();

            yield return new WaitForSeconds(_secondsBetweenLogos);

            // P20 LOGO
            var tweenFadeInP20Logo = _p20Logo.DOFade(1.0f, _secondsFadeInDurationP20Logo).SetEase(_easeFadeInP20Logo);
            yield return tweenFadeInP20Logo.WaitForCompletion();
            yield return new WaitForSeconds(_secondsBeforeFadeOutP20Logo);
            var tweenFadeOutP20Logo = _p20Logo.DOFade(0.0f, _secondsFadeOutDurationP20Logo).SetEase(_easeFadeOutP20Logo);
            yield return tweenFadeOutP20Logo.WaitForCompletion();

            yield return new WaitForSeconds(_secondsBetweenLogos);

            //GAME LOGO 
            var tweenFadeInGameLogo = _gameLogo.DOFade(1.0f, _secondsFadeInDurationGameLogo).SetEase(_easeFadeInGameLogo);
            yield return tweenFadeInGameLogo.WaitForCompletion();

            yield return new WaitForSeconds(_secondsBeforeFadeOutGameLogo);

            var tweenFadeOutGameLogo = _gameLogo.DOFade(0.0f, _secondsFadeOutDurationGameLogo).SetEase(_easeFadeOutGameLogo);
            yield return tweenFadeOutGameLogo.WaitForCompletion();

            yield return new WaitForSeconds(_secondsBeforeStartGame);

        }
        // END
        imageFade.DOFade(0, 1);
        OnIntroEnded?.Invoke();
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
        Debug.Log("son lancement de l'appli");
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
    private void IntroEnded()
    {
        Debug.Log("son intro finie start jeu");
    }
    #endregion

}
