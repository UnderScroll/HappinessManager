using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Yarn.Unity;

public class IntroDialogues : MonoBehaviour
{
    DialogueRunner _dRunner;

    private void Start()
    {
        _dRunner = GetComponent<DialogueRunner>();
    }
    public void PlaySound()
    {
        IEnumerable<string> tags = _dRunner.GetTagsForNode(_dRunner.CurrentNodeName);
        if (tags.Any())
            AkSoundEngine.PostEvent(tags.First(), gameObject);
    }
}
