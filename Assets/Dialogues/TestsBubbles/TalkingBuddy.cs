using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;
using NaughtyAttributes;

public class TalkingBuddy : MonoBehaviour
{
    UnityEvent OnTalking;
    [SerializeField] GameObject dialogueViewPrefab;
    [SerializeField] Canvas canvas;
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

    bool isBoss = false;

    #region Private Attributes
    private string nextLineToSay = "";
    private bool isTalking = false;
    private Transform dialoguePosition;
    private GameObject actualDialogueObject = null;
    private float actualTime = 0;
    DialogueRunner dialogueRunner;
    private float timeBeforeTalking = 3;
    #endregion

    private void Start()
    {
        dialoguePosition = this.transform;
        canvas.worldCamera = Camera.main;
        OnTalking.AddListener(PlayDialogueSound);


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

    /// <summary>
    /// SOUND EVENT !
    /// </summary>
    private void PlayDialogueSound()
    {
        if (isBoss)
        {
            
        }
        else
        {

        }
    }
    #endregion
}