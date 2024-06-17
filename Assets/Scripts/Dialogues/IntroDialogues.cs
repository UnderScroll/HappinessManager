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
        if (_dRunner == null)
            _dRunner = GetComponent<DialogueRunner>();

        IEnumerable<string> _tags = _dRunner.GetTagsForNode(_dRunner.CurrentNodeName);
        if (_tags.Any())
            AkSoundEngine.PostEvent(_tags.First(), gameObject);
    }
}
