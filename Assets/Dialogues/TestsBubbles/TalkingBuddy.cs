using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;

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
    public int minimalTimeBT = 3;
    /// <summary>
    /// BT is for Before Talking
    /// /!\ exclu
    /// </summary>
    public int maximalTimeBT = 7;

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
        RandomizeLine();
        ResetTimer();
        timeBeforeTalking = Random.Range(minimalTimeBT, maximalTimeBT);
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

    private void StopDialogue()
    {
        Destroy(actualDialogueObject);
        timeBeforeTalking = Random.Range(minimalTimeBT, maximalTimeBT);
        RandomizeLine();
        isTalking = false;
    }
    #endregion
}