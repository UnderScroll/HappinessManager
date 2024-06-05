using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;
using NaughtyAttributes;
using UnityEditor.SceneManagement;

public class TalkingBuddy : MonoBehaviour
{
    public enum Characters
    {
        Boss,
        CommunityManager,
        Comptable,
        DirCrea,
        EmployéTriste,
        Stagiaire
    };

    UnityEvent OnTalking = new UnityEvent();
    [SerializeField] GameObject dialogueViewPrefab;

    [Header("Dialogues Infos")]
    [SerializeField] YarnProject yarnProject;
    [SerializeField] Characters Character;
    [SerializeField] int NbLinesStage1;
    [SerializeField] int NbLinesStage2;
    [SerializeField] int NbLinesStage3;
    [SerializeField] int NbLinesStage4;
    [SerializeField] int NbLinesStage5;

    [Header("Settings Timer Before Talking")]
    public int TimeBeforeTalking = 5;

    public bool RandomizeTimeBT = false;
    /// <summary>
    /// BT is for Before Talking
    /// /!\ inclus
    /// </summary>
    [ShowIf("RandomizeTimeBT")]
    public int minimalTimeBT = 3;
    /// <summary>
    /// BT is for Before Talking
    /// /!\ exclu
    /// </summary>
    [ShowIf("RandomizeTimeBT")]
    public int maximalTimeBT = 7;

    #region Private Attributes
    private string nextLineToSay = "";
    private bool isTalking = false;
    private Transform dialoguePosition;
    private GameObject actualDialogueObject = null;
    private float actualTime = 0;
    DialogueRunner dialogueRunner;
    private float timeBeforeTalking = 3;
    private GameManager _gameManager;
    private string Line;
    #endregion

    private void Start()
    {
        dialoguePosition = this.transform;
        _gameManager = FindObjectOfType<GameManager>();
        if (_gameManager == null) { Debug.LogError("TalkingBuddy.cs : GameManager not found!"); }
        OnTalking.AddListener(PlayDialogueSound);

        // INIT LINES
        InitLines(_gameManager.CurrentStage);

        RandomizeLine();
        ResetTimer();
        InitTimer();
    }

    private void Update()
    {
        actualTime += Time.deltaTime;
        if (isTalking)
        {
            if (actualTime > timeBeforeTalking)
            {
                StopDialogue();
                ResetTimer();
            }
        }
        else
        {
            if (actualTime > timeBeforeTalking)
            {
                InitDialogue();
                ResetTimer();
            }
        }
    }

    #region Private Methods
    private void ResetTimer()
    {
        actualTime = 0.0f;
    }
    private void RandomizeLine()
    {
        int max = 0;
        switch(_gameManager.CurrentStage)
        {
            case GameManager.Stage.Stage1:
                max = NbLinesStage1;
                break;
                case GameManager.Stage.Stage2:
                max = NbLinesStage2;
                break;
                case GameManager.Stage.Stage3:
                max = NbLinesStage3;
                break;
                case GameManager.Stage.Stage4:
                max= NbLinesStage4;
                break;
                case GameManager.Stage.Stage5:
                max = NbLinesStage5;
                break;
        }
        string lineNb = "bark" + Random.Range(0, max).ToString();
        nextLineToSay = Line + lineNb;
    }
    private void InitDialogue()
    {
        // Spawn view & setup dialogueRunner
        actualDialogueObject = Instantiate(dialogueViewPrefab, dialoguePosition);
        actualDialogueObject.transform.parent = this.transform;
        dialogueRunner = actualDialogueObject.GetComponent<DialogueRunner>();
        dialogueRunner.SetProject(yarnProject);

        // Init Dialogue
        dialogueRunner.StartDialogue(nextLineToSay);
        isTalking = true;
        OnTalking?.Invoke();
    }
    private void InitTimer()
    {
        if (!RandomizeTimeBT)
            timeBeforeTalking = TimeBeforeTalking;
        else
            timeBeforeTalking = Random.Range(minimalTimeBT, maximalTimeBT);
    }

    private void StopDialogue()
    {
        Destroy(actualDialogueObject);
        InitTimer();
        RandomizeLine();
        isTalking = false;
    }
    private string GetCharaName()
    {
        switch (Character)
        {
            case Characters.Boss:
                return "Boss";
            case Characters.CommunityManager:
                return "CM";
            case Characters.Comptable:
                return "Comptable";
            case Characters.EmployéTriste:
                return "ET";
            case Characters.Stagiaire:
                return "Stagiaire";
        }
        return "NoCharaFound";
    }
    private void InitLines(GameManager.Stage _stage)
    {
        Debug.Log("stage = " + _stage);
        Line = GetCharaName() + _stage.ToString();
        Debug.Log(Line);
    }

    /// <summary>
    /// SOUND EVENT !
    /// </summary>
    private void PlayDialogueSound()
    {
        if (Character == Characters.Boss)
        {

        }
        else
        {

        }
    }
    #endregion
}