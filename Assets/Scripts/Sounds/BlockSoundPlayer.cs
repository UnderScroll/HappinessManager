using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSoundPlayer : MonoBehaviour
{
    public string BlockName;

    private string _eventName;

    public void Initialize()
    {
        string blockName = "";
        switch (BlockName)
        {
            case "BasicBlock": //Scriptable object name (Resources/Blocks)
                blockName = "basic_block"; //Name in Wwise event
                break;
            default:
                Debug.LogError($"No sound for {BlockName}");
                break;
        }

        if (blockName == "")
            Debug.LogError("Failed to get block name, or is not implemented yet");

        _eventName = $"Play_Build_{blockName}";
    }

    public void PlayBasicBlockPlace()
    {
        AkSoundEngine.PostEvent($"{_eventName}_place", gameObject);
    }
    public void PlayBasicBlockRemove()
    {
        AkSoundEngine.PostEvent($"{_eventName}_remove", gameObject);
    }
}
