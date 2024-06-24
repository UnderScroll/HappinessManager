using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class wolalescript : MonoBehaviour
{
    public RawImage endImg;
    public bool IsEndAnimRunning;
    public GameManager GameManager;

    public void Update()
    {
        if (IsEndAnimRunning == false)
        {
            endImg.gameObject.SetActive(true);
            IsEndAnimRunning = true;
            AkSoundEngine.PostEvent("Stop_Music_global", gameObject);
            AkSoundEngine.PostEvent("Play_End", gameObject);
        }

        if (IsEndAnimRunning)
            endImg.transform.position += new Vector3(0, Time.deltaTime * 0.2f, 0);

        if (endImg.transform.position.y >= 5.3)
            GameManager.LoadMainMenu();
    }
}
