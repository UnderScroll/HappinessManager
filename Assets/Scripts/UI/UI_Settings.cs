using AK.Wwise;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

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
    [SerializeField] Toggle sensitivityFilter;

    private void Start()
    {
        DefaultSoundValues();
        musicSlider.onValueChanged.AddListener(ChangeMusicVolume);
        sfxSlider.onValueChanged.AddListener(ChangeSFXVolume);
        dialoguesSlider.onValueChanged.AddListener(ChangeDialoguesVolume);
    }

    public void ChangeMusicVolume(float _newValue)
    {
        rtpcMUSIC.SetGlobalValue(_newValue);
        UpdateUIMusic(_newValue);
    }
    public void ChangeSFXVolume(float _newValue)
    {
        rtpcSFX.SetGlobalValue(_newValue);
        UpdateUISFX(_newValue);
    }
    public void ChangeDialoguesVolume(float _newValue)
    {
        rtpcDIALOGUE.SetGlobalValue(_newValue);
        UpdateUIDialogue(_newValue);
    }
    public void SwitchSensitivityFilter()
    {
        if (sensitivityFilter.isOn)
            rtpcSENSITIVITY.SetGlobalValue(1);
        else
            rtpcSENSITIVITY.SetGlobalValue(0);
    }
    public void RestoreToDefault()
    {
        DefaultSoundValues();
    }
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
        sensitivityFilter.isOn = false;

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
