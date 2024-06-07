using AK.Wwise;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Settings : MonoBehaviour
{
    [Header("RTPCs")]
    [SerializeField]
    private AK.Wwise.RTPC rtpcMUSIC = null;
    [SerializeField]
    private AK.Wwise.RTPC rtpcSFX = null;
    [SerializeField]
    private AK.Wwise.RTPC rtpcDIALOGUE = null;
    [SerializeField]
    private AK.Wwise.RTPC rtpcSENSITIVITY = null;

    [Header("SoundsSettings")]
    [SerializeField] TextMeshProUGUI musicText;
    [SerializeField] TextMeshProUGUI sfxText;
    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField][Range(0f, 1f)] float startVolume = 1f;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;
    [SerializeField] Slider dialoguesSlider;
    [SerializeField] UI_ONOFFButton SensitivityFilter;

    private void Start()
    {
        DefaultSoundValues();
        musicSlider.onValueChanged.AddListener(ChangeMusicVolume);
        sfxSlider.onValueChanged.AddListener(ChangeSFXVolume);
        dialoguesSlider.onValueChanged.AddListener(ChangeDialoguesVolume);
    }
    #region Graphics
    public void SetFullscreen(bool _on)
    {
        if (_on)
            Screen.SetResolution(1920, 1080, true);
        else
            Screen.SetResolution(1920, 1080, false);
    }
    public void HighContrastMode(bool _on)
    {
        // set up high contrast mode
    }
    #endregion
    #region Sound
    public void ChangeMusicVolume(float _newValue)
    {
        rtpcMUSIC.SetGlobalValue(_newValue);
        UpdateUIMusic(_newValue);
    }
    public void ChangeSFXVolume(float _newValue)
    {
        rtpcSFX.SetGlobalValue(_newValue);
        UpdateUISFX(_newValue);

        //play sound to check new volume
        float SFXTriggerValue = _newValue * 100;
        int SFXTriggerValueInt = Convert.ToInt32(SFXTriggerValue);
        if (SFXTriggerValueInt % 15 == 0)
        {
            Debug.Log(SFXTriggerValueInt);
            AkSoundEngine.PostEvent("Play_Menu_SliderSFX", gameObject);
        }
    }
    public void ChangeDialoguesVolume(float _newValue)
    {
        rtpcDIALOGUE.SetGlobalValue(_newValue);
        UpdateUIDialogue(_newValue);

        //play sound to check new volume
        float DialogueTriggerValue = _newValue * 100;
        int DialogueTriggerValueInt = Convert.ToInt32(DialogueTriggerValue);
        if (DialogueTriggerValueInt % 25 == 0)
        {
            AkSoundEngine.PostEvent("Play_Menu_SliderDialogues", gameObject);
        }
    }
    public void SwitchSensitivityFilter(bool _on)
    {
        if (_on)
        {
            rtpcSENSITIVITY.SetGlobalValue(1);
        }
        else
        {
            rtpcSENSITIVITY.SetGlobalValue(0);
        }
           
    }
    public void RestoreToDefault()
    {
        DefaultSoundValues();
    }
    #endregion
    #region Private Methods
    private void DefaultSoundValues()
    {
        rtpcMUSIC.SetGlobalValue(startVolume);
        rtpcSFX.SetGlobalValue(startVolume);
        rtpcDIALOGUE.SetGlobalValue(startVolume);
        rtpcSENSITIVITY.SetGlobalValue(0);

        UpdateUIMusic(startVolume);
        UpdateUISFX(startVolume);
        UpdateUIDialogue(startVolume);
        SensitivityFilter.Move(false);

        musicSlider.value = startVolume;
        sfxSlider.value = startVolume;
        dialoguesSlider.value = startVolume;
    }
    private void UpdateUIMusic(float _value)
    {
        float valueToPrint = _value * 100;
        musicText.text = valueToPrint.ToString("F1") + "%";
    }
    private void UpdateUISFX(float _value)
    {
        float valueToPrint = _value * 100;
        sfxText.text = valueToPrint.ToString("F1") + "%";
    }
    private void UpdateUIDialogue(float _value)
    {
        float valueToPrint = _value * 100;
        dialogueText.text = valueToPrint.ToString("F1") + "%";
    }
    #endregion
}
