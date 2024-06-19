using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static ak.wwise;

public class wolalescript : MonoBehaviour
{
    public RawImage endImg;
    public GameObject ui;
    public bool IsEndAnimRunning;
    public GameManager GameManager;

    public void ShowEndCutScene()
    {
        endImg.gameObject.SetActive(true);
        ui.SetActive(false);
        IsEndAnimRunning = true;
        AkSoundEngine.PostEvent("Play_End", gameObject);
        AkSoundEngine.PostEvent("Stop_Music", gameObject);
    }

    public void Update()
    {
        if (IsEndAnimRunning)
            endImg.transform.position += new Vector3(0, Time.deltaTime * 12.0f, 0);

        if (endImg.transform.position.y >= 1230)
            GameManager.LoadMainMenu();
    }
}
