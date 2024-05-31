using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class UI_Settings : MonoBehaviour
{
    [Header("SoundsSettings")]
    [SerializeField] TextMeshProUGUI musicText;
    [SerializeField] TextMeshProUGUI sfxText;
    [SerializeField][Range(0f, 1f)] float startVolume = 0.5f;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;
    [SerializeField] Toggle sensitivityFilter;

    private void Start()
    {
        DefaultSoundValues();
    }

    public void ChangeMusicVolume(float _newValue)
    {
        AkSoundEngine.SetRTPCValue("RTPC_MUSIC_BUS", _newValue, gameObject);
        UpdateUIMusic(_newValue);
    }
    public void ChangeSFXVolume(float _newValue)
    {
        AkSoundEngine.SetRTPCValue("RTPC_SFX_BUS", _newValue, gameObject);
        UpdateUISFX(_newValue);
    }
    public void SwitchSensitivityFilter()
    {
        if (sensitivityFilter.isOn)
            AkSoundEngine.SetRTPCValue("RTPC_SENSITIVITY_FILTER", 0, gameObject);
        else
            AkSoundEngine.SetRTPCValue("RTPC_SENSITIVITY_FILTER", 1, gameObject);

    }
    public void RestoreToDefault()
    {
        DefaultSoundValues();
    }
    #region Private Methods
    private void DefaultSoundValues()
    {
        AkSoundEngine.SetRTPCValue("RTPC_MUSIC_BUS", startVolume, gameObject);
        AkSoundEngine.SetRTPCValue("RTPC_SFX_BUS", startVolume, gameObject);
        AkSoundEngine.SetRTPCValue("RTPC_SENSITIVITY_FILTER", 1, gameObject);
        UpdateUIMusic(startVolume);
        UpdateUISFX(startVolume);
        sensitivityFilter.isOn = false;

        musicSlider.value = startVolume;
        sfxSlider.value = startVolume;
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
    #endregion
}
