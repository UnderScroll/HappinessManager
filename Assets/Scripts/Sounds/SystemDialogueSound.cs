using UnityEngine;

public class SystemDialogueSound : MonoBehaviour
{
    public void SystemIn()
    {
        AkSoundEngine.PostEvent("Play_Menu_System_in", gameObject);
    }
    public void SystemNext()
    {
        AkSoundEngine.PostEvent("Play_Menu_System_next", gameObject);
    }
    public void SystemOut()
    {
        AkSoundEngine.PostEvent("Play_Menu_System_out", gameObject);
    }
}
