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
            case "Floor_1": //GameManager Floor Name
                _floorWwiseName = "boss";
                break;
            case "Floor_2": //GameManager Floor Name
                _floorWwiseName = "cafeteria";
                break;
            case "Floor_3": //GameManager Floor Name
                _floorWwiseName = "coworking";
                break;
            case "Floor_4": //GameManager Floor Name
                _floorWwiseName = "teambuilding";
                break;
            case "Floor_5": //GameManager Floor Name
                _floorWwiseName = "intern";
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
        AkSoundEngine.PostEvent("Play_UI_Restart_Level", gameObject);
    }

    public void PlayOnLevelFailed()
    {
        AkSoundEngine.PostEvent("Play_Music_SetSwitch_defeat", gameObject);
        AkSoundEngine.PostEvent("Play_Menu_paused", gameObject);
        AkSoundEngine.PostEvent("Play_Menu_Settings_onMenuOpen", gameObject);
    }

    public void PlayOnLevelValidated()
    {
        AkSoundEngine.PostEvent("Play_Music_SetSwitch_victory", gameObject);
        AkSoundEngine.PostEvent("Play_Menu_select", gameObject);
        AkSoundEngine.PostEvent("Play_Menu_Settings_onMenuOpen", gameObject);
    }
}
