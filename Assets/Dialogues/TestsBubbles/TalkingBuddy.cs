using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;
using NaughtyAttributes;

public class TalkingBuddy : MonoBehaviour
{
    [SerializeField] GameObject dialogueViewPrefab;

    [Header("Dialogues Infos")]
    [SerializeField] List<string> linesNames;

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
    GameManager _gmRef;
    #endregion

    private void Start()
    {
        _gmRef = GetComponent<GameManager>();
        dialoguePosition = this.transform;
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
        nextLineToSay = linesNames[Random.Range(0, linesNames.Count)];
    }
    private void InitDialogue()
    {
        // Spawn view & setup dialogueRunner
        actualDialogueObject = Instantiate(dialogueViewPrefab, dialoguePosition);
        actualDialogueObject.transform.parent = this.transform;
        dialogueRunner = actualDialogueObject.GetComponent<DialogueRunner>();

        // Init Dialogue
        dialogueRunner.StartDialogue(nextLineToSay);
        isTalking = true;
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
    #endregion
}