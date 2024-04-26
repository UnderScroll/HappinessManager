using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class YarnTestScript : DialogueViewBase
{
    public override void RunLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
    {
        base.RunLine(dialogueLine, onDialogueLineFinished);
    }

    public override void InterruptLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
    {
        base.InterruptLine(dialogueLine, onDialogueLineFinished);
    }

    public override void DismissLine(Action onDismissalComplete)
    {
        base.DismissLine(onDismissalComplete);
    }

    public override void UserRequestedViewAdvancement()
    {
        base.UserRequestedViewAdvancement();
    }

}
