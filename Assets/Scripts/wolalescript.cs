using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class wolalescript : MonoBehaviour
{
    public RawImage endImg;
    public GameObject ui;
    public GameObject structureOrigin;
    public bool IsEndAnimRunning;
    public GameManager GameManager;

    public void ShowEndCutScene()
    {
        endImg.gameObject.SetActive(true);
        ui.SetActive(false);
        IsEndAnimRunning = true;
        structureOrigin.SetActive(false);
        AkSoundEngine.PostEvent("Stop_Music_global", gameObject);
        AkSoundEngine.PostEvent("Play_End", gameObject);
    }

    public void Update()
    {
        if (IsEndAnimRunning)
            endImg.transform.position += new Vector3(0, Time.deltaTime * 30.0f, 0);

        if (endImg.transform.position.y >= 1550)
            GameManager.LoadMainMenu();
    }
}
