using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private GameManager _gameManager;

    private string _floorWwiseName = "";

    private void Start()
    {
        if (!TryGetComponent(out _gameManager))
            Debug.LogError("Failed to get GameManager in SoundManager");

        switch (_gameManager.FloorName)
        {
            case "BossFloor": //GameManager Floor Name
                _floorWwiseName = "boss";
                break;
            default:
                Debug.LogError($"No sound for floor {_gameManager.FloorName}");
                break;
        }

        if (_floorWwiseName == "")
            Debug.LogError("Failed to get floor name, or is not implemented yet");
    }

    public void PLayOnFirstLevelLoaded()
    {
        AkSoundEngine.PostEvent($"Play_Music_{_floorWwiseName}", gameObject);
        AkSoundEngine.PostEvent($"Play_Amb_{_floorWwiseName}", gameObject);
    }

    public void PlayOnLaunchingSimulation()
    {
        AkSoundEngine.PostEvent($"Play_Music_SetSwitch_validating", gameObject);
    }

    public void PlayOnBuilding()
    {
        AkSoundEngine.PostEvent($"Play_Music_SetSwitch_build", gameObject);
    }

    public void PlayOnLevelFailed()
    {
        AkSoundEngine.PostEvent("Play_Music_SetSwitch_defeat", gameObject);
    }

    public void PlayOnLevelValidated()
    {
        AkSoundEngine.PostEvent("Play_Music_SetSwitch_victory", gameObject);
    }
}
