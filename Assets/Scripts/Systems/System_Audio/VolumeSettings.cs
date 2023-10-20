using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{

    private enum VolumeType
    {
        MASTER,
        MUSIC,
        AMBIENCE,
        SFX
    }

    [Header("Volume Type")]
    [SerializeField] private VolumeType volumeType;

    [Header("UI Elements")]
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private TMP_Text volumeCounter;


    private void Start()
    {
        if (volumeType == VolumeType.MASTER)
        {
            volumeSlider.value = AudioManager.instance.masterVolume * 100;
        }

        if (volumeType == VolumeType.MUSIC)
        {
            volumeSlider.value = AudioManager.instance.musicVolume * 100;
        }

        if (volumeType == VolumeType.AMBIENCE)
        {
            volumeSlider.value = AudioManager.instance.ambienceVolume * 100;
        }

        if (volumeType == VolumeType.SFX)
        {
            volumeSlider.value = AudioManager.instance.SFXVolume * 100;
        }
    }


    public void UpdateVolumeAmount()
    {
        volumeSlider.value = Mathf.Clamp(volumeSlider.value, 0.2f, 1);
        float roundValue = volumeSlider.value;
        roundValue = Mathf.RoundToInt(roundValue * 100);
        volumeCounter.SetText(roundValue.ToString());


        if (volumeType == VolumeType.MASTER)
        {
            AudioManager.instance.masterVolume = volumeSlider.value;
        }

        if (volumeType == VolumeType.MUSIC)
        {
            AudioManager.instance.musicVolume = volumeSlider.value;
        }

        if (volumeType == VolumeType.AMBIENCE)
        {
            AudioManager.instance.ambienceVolume = volumeSlider.value;
        }

        if (volumeType == VolumeType.SFX)
        {
            AudioManager.instance.SFXVolume = volumeSlider.value;
        }
    }

    
}
