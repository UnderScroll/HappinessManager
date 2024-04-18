using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;

public class TalkingBuddy : MonoBehaviour
{
    [SerializeField] GameObject Canvas;
    [SerializeField] DialogueRunner dialogueRunner;

    [Header("Dialogues")]
    [SerializeField] List<string> linesNames;

    public float timeBeforeTalking = 3;
    public float actualTime = 0;
    public string nextLineToSay = "";

    private void Start()
    {
        dialogueRunner.onDialogueComplete.AddListener(InitLine);
        InitLine();
    }

    private void Update()
    {
        actualTime += Time.deltaTime;
        if (actualTime > timeBeforeTalking)
        {
            dialogueRunner.StartDialogue(nextLineToSay);
            actualTime = 0.0f;
        }
    }
    private void InitLine()
    {
        nextLineToSay = linesNames[Random.Range(0, linesNames.Count)];
        actualTime = 0.0f;
        timeBeforeTalking = Random.Range(3, 7);
    }
}


