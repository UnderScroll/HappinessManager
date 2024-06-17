using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkSound : MonoBehaviour
{
    public void StepStag()
    {
        AkSoundEngine.PostEvent("Play_StagStep", gameObject);
    }
    public void ClothStag()
    {
        AkSoundEngine.PostEvent("Play_StagCloth", gameObject);
    }
    public void StepCM()
    {
        AkSoundEngine.PostEvent("Play_CMStep", gameObject);
    }
    public void ClothCM()
    {
        AkSoundEngine.PostEvent("Play_CMCloth", gameObject);
    }
    public void StepComptable()
    {
        AkSoundEngine.PostEvent("Play_ComptableStep", gameObject);
    }
    public void ClothComptable()
    {
        AkSoundEngine.PostEvent("Play_ComptableCloth", gameObject);
    }
    public void KikiComptable()
    {
        AkSoundEngine.PostEvent("Play_ComptableKiki", gameObject);
    }
    public void StepET()
    {
        AkSoundEngine.PostEvent("Play_ETStep", gameObject);
    }
    public void ClothET()
    {
        AkSoundEngine.PostEvent("Play_ETCloth", gameObject);
    }
    public void StepDirCrea()
    {
        AkSoundEngine.PostEvent("Play_DirCreStep", gameObject);
    }
    public void ClothDirCre()
    {
        AkSoundEngine.PostEvent("Play_DirCreCloth", gameObject);
    }
}
